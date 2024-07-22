using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Pool;

public enum State
{
    Normal, Combat
}

public class UnitManager : MonoBehaviour
{

    [Header("UnitList"), Tooltip("유닛 리스트")]
    public List<Unit> allUnitList;
    public List<Unit> selectedUnitList;

    [Header("Prefab"), Tooltip("유닛 프리팹")]
    public GameObject[] unitPrefabArray;
    [Header("Spawn"), Tooltip("유닛 스폰지")]
    public Transform unitSpawn;
    [Header("Move"), Tooltip("유닛 속도제어")]
    public float moveSpeed = 5.0f;
    public float rotSpeed = 360.0f;

    //유닛 랜덤
    private Dictionary<string, double> dicRank;
    double SumOfWeights;

    // 네브메쉬패스
    private NavMeshPath myPath;

    private void Awake(){
        allUnitList = new List<Unit>();
        selectedUnitList = new List<Unit>();

        dicRank = new Dictionary<string, double>()
        {
            {"_Common", 30.0f},
            {"_Uncommon", 0.1f},
            {"_Rare", 1.4f},
            {"_Epic", 20.0f},
            {"_Legendary", 0.01f}
        };

        foreach(float value in dicRank.Values){
            SumOfWeights += value;
        }
    }

    public void UnitAnim()
    {
        foreach(Unit unit in allUnitList)
        {
            switch(unit.unitState)
            {
                case State.Normal:
                {
                    unit.unitAnim.Update(Time.deltaTime);
                    break;
                }
                case State.Combat:
                {
                    if(unit.IsAttacking) unit.unitAnim.Update(Time.deltaTime * unit.unitData.attackSpeed);
                    else unit.unitAnim.Update(Time.deltaTime);
                    break;
                }
            }
        }
    }

    public void UnitAI(){
        foreach(Unit unit in allUnitList)
        {
            unit.prevElapsedTime = unit.attackElapsedTime;
            unit.attackElapsedTime += Time.deltaTime * unit.unitData.attackSpeed;
            if(unit.forceMove)
            {
                unit.targetMonster = null;
                unit.unitState = State.Normal;
                unit.forceHold = false;
                unit.attackElapsedTime = 0.0f;
                unit.unitAnim.SetBool("IsAttacking", false);
                continue;
            }

            unit.unitAnim.SetBool("IsAttacking", unit.IsAttacking);
            if(unit.IsAttacking) continue;

            foreach(Monster monster in GameWorld.Instance.MonsterManager.allMonsterList)
            {
                using (ListPool<Monster>.Get(out var rangedMonsters)){
                    if(Vector3.Distance(unit.transform.position, monster.transform.position) <= unit.unitData.attackRange) rangedMonsters.Add(monster);
                    if (rangedMonsters.Count > 0) 
                    {
                        switch (unit.unitState)
                        {
                            case State.Normal:
                            {
                                unit.unitState = State.Combat;
                                unit.targetMonster = rangedMonsters[0];
                                break;
                            }
                            case State.Combat:
                            {
                                if (unit.forceHold) 
                                {
                                    unit.targetMonster = rangedMonsters[0];
                                }
                                else
                                {
                                    if (unit.targetMonster == null) unit.targetMonster = rangedMonsters[0];
                                    if (Vector3.Distance(unit.transform.position, unit.targetMonster.transform.position) > unit.unitData.attackRange) 
                                    {
                                        unit.destination = unit.targetMonster.transform.position;
                                    }
                                    else
                                    { 
                                        unit.destination = unit.transform.position;
                                    }
                                }
                                break;
                            }
                        }
                    }
                }
            }
        }
    }

    public void UnitAttack()
    {
        foreach (Unit unit in allUnitList)
        {
            if(unit.targetMonster != null)
            {
                Vector3 dir = unit.targetMonster.transform.position - unit.transform.position;
                dir.Normalize();
                float rotAngle = Vector3.Angle(unit.transform.forward, dir);
                float rotDir = Vector3.Dot(unit.transform.right, dir) < 0.0f ? -1.0f : 1.0f;
                unit.transform.Rotate(Vector3.up * rotDir * rotAngle);
                if(unit.IsAttackable)
                {
                    unit.attackElapsedTime = 0.0f;
                    unit.unitAnim.CrossFade("Attack", 0.1f);
                }

                if(unit.attackElapsedTime > unit.unitData.bulletSpawnTime && unit.prevElapsedTime < unit.unitData.bulletSpawnTime) GameWorld.Instance.BulletManager.SpawnBullet(unit);
            }
            
            
        }
    }

    public void UnitMove(){
        if(myPath == null) myPath = new NavMeshPath();
        foreach(Unit unit in allUnitList){
            if(!unit.IsAttacking || unit.forceMove) 
            {
                if(NavMesh.CalculatePath(unit.transform.position, unit.destination, NavMesh.AllAreas, myPath)){
                    switch(myPath.status){
                        case NavMeshPathStatus.PathComplete:
                        case NavMeshPathStatus.PathPartial:
                        if(myPath.corners.Length > 1){
                            unit.unitAnim.SetBool("IsMoving", true);
                            Vector3 moveDir = myPath.corners[1] - unit.transform.position;
                            float moveDist = moveDir.magnitude;
                            moveDir.Normalize();

                            float rotAngle = Vector3.Angle(unit.transform.forward, moveDir);
                            float rotDir = Vector3.Dot(unit.transform.right, moveDir) < 0.0f ? -1.0f : 1.0f;

                            float rotateAmount = rotSpeed * Time.deltaTime;
                            if (rotAngle < rotateAmount) rotateAmount = rotAngle;
                            unit.transform.Rotate(Vector3.up * rotDir * rotateAmount);

                            float moveAmount = moveSpeed * Time.deltaTime;
                            if(moveDist < moveAmount) moveAmount = moveDist;
                            unit.transform.Translate(moveDir * moveAmount, Space.World);
                        }
                        else{
                            unit.destination = unit.transform.position;
                            unit.unitAnim.SetBool("IsMoving", false);
                            unit.forceMove = false;
                        }
                            break;
                        case NavMeshPathStatus.PathInvalid:
                            break;
                    }
                }
            }
        }
    }

    public void InputHold()
    {
        foreach (Unit unit in selectedUnitList) 
        {
            unit.forceHold = true;
            unit.forceMove = false;
            unit.destination = unit.transform.position;
        }
    }

    public void InputDestination(Vector3 pos)
    {
        List<Vector3> destinationList = GetDestinationListAround(pos, new float[] { 2.0f, 4.0f, 6.0f}, new int[] { 5, 10, 20 });
        
        int destinationListIdx = 0;

        foreach(Unit unit in selectedUnitList){
            unit.destination = destinationList[destinationListIdx];
            destinationListIdx = (destinationListIdx + 1) % destinationList.Count;
            unit.forceMove = true;
        }
    }

    private List<Vector3> GetDestinationListAround(Vector3 pos, float[] ringRadiusArray, int[] ringPositionCount){
        List<Vector3> destinatnionList = new List<Vector3>();
        destinatnionList.Add(pos);
        for(int i = 0; i < ringRadiusArray.Length; i++){
            destinatnionList.AddRange(GetDestinationListAround(pos, ringRadiusArray[i], ringPositionCount[i]));
        }
        return destinatnionList;
    }

    private List<Vector3> GetDestinationListAround(Vector3 pos, float radius, int positionCount){
        List<Vector3> destinationList = new List<Vector3>();
        for (int i = 0; i < positionCount; i++)
        {
            float angle = i * (360.0f / positionCount);
            float x = Mathf.Sin(angle);
            float z = Mathf.Cos(angle);
            Vector3 dir = new Vector3(x, 0.0f, z);
            Vector3 destination = pos + dir * radius;
            destinationList.Add(destination);
        }
        return destinationList;
    }

    public void InputTargeting(Monster monster)
    {
        foreach(Unit unit in selectedUnitList)
        {
            unit.targetMonster = monster;
            unit.unitState = State.Combat;
        }
    }


    // 스폰
    public void SpawnUnit()
    {
        int randIdx = Random.Range(0, unitPrefabArray.Length);
        Vector3 randomSpawn = RandomSpawn();
        GameObject obj = Instantiate(unitPrefabArray[randIdx], randomSpawn, Quaternion.identity);
        obj.transform.parent = unitSpawn;
        Unit unit = obj.GetComponent<Unit>();

        string rank = GetRandomPick();
        int index = unit.name.IndexOf("(Clone)");
        unit.unitKey = (UnitType)System.Enum.Parse(typeof(UnitType) ,unit.name.Substring(0, index) + rank);
        unit.unitData = GameWorld.Instance.BalanceManager.unitDic[unit.unitKey];

        unit.Init();
        unit.unitAnim.enabled = false;
        unit.unitAnim.Update(Time.deltaTime);
        switch(rank)
        {
            case "_Common":
            break;
            case "_Uncommon":
            unit.outline.outlineColor = Color.green;
            break;
            case "_Rare":
            unit.outline.outlineColor = Color.cyan;
            break;
            case "_Epic":
            unit.outline.outlineColor = Color.magenta;
            break;
            case "_Legendary":
            unit.outline.outlineColor = Color.yellow;
            break;
        }

        allUnitList.Add(unit);
    }

    Vector3 RandomSpawn()
    {
        float radius = Random.Range(0.0f, 2.0f);
        float angle = Random.Range(0.0f, 360.0f);
        float x = radius * Mathf.Sin(angle);
        float z = radius * Mathf.Cos(angle);
        Vector3 randomVector = new Vector3(x, 0.55f, z);
        Vector3 randomPosition = unitSpawn.transform.position + randomVector;
        return randomPosition;
    }

    public string GetRandomPick(){
        
        double randomValue = Random.Range(0.0f, 1.0f);
        randomValue *= SumOfWeights;

        if(randomValue < 0.0) randomValue = 0.0f;
        if(randomValue > SumOfWeights) randomValue = SumOfWeights - 0.00000001;

        double current = 0.0f;
        foreach(KeyValuePair<string, double> item in dicRank)
        {
            current += item.Value;

            if(randomValue < current)
            {
                return item.Key;
            }
        }
        return null;
    }


}