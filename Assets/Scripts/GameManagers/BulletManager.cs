using System;
using System.Collections.Generic;
using UnityEngine;

public class BulletManager : MonoBehaviour
{
    public LayerMask monsterMask;
    public GameObject[] bulletPrefabsArray;
    public List<Bullet> allBulletList = new List<Bullet>();
    public Vector2 Range_x = new Vector2(-125, -75);
    public Vector2 Range_z = new Vector2(-25, 25);

    private float _rotSpeed = 360.0f;

    public void BulletAI()
    {
        for (int i = allBulletList.Count - 1; i >= 0; --i)
        {
            Bullet bullet = allBulletList[i];
            if (bullet.targetMonster == null)
            {
                allBulletList.Remove(bullet);
                Destroy(bullet.gameObject);
            }
            switch (bullet.bulletData.hitCheck)
            {
                case BulletHitCheck.Targeting:
                {
                    if(bullet.targetMonster == null) continue;
                    if(bullet.transform.position == bullet.targetMonster.GetComponentInChildren<Socket>().transform.position)
                    {
                        bullet.targetMonster.InflictDamage(bullet.bulletOwner.unitDamage * bullet.bulletData.damageCoefficient);
                        allBulletList.Remove(bullet);
                        Destroy(bullet.gameObject);
                    }
                    break;
                }
                case BulletHitCheck.Moving:
                {
                    if(bullet.targetMonster == null) continue;
                    Collider[] colliders = Physics.OverlapSphere(bullet.transform.position, bullet.bulletData.hitRange, monsterMask);
                    foreach(Collider col in colliders)
                    {
                        Monster monster = col.GetComponent<Monster>();
                        if (!bullet.hitMonsterList.Contains(monster) && bullet.bulletData.attackableNumber > bullet.hitMonsterList.Count)
                        {
                            bullet.hitMonsterList.Add(monster);
                            monster.InflictDamage(bullet.bulletOwner.unitDamage * bullet.bulletData.damageCoefficient);
                        }
                    }
                    if(bullet.transform.position.x < Range_x.x || bullet.transform.position.x > Range_z.y || 
                    bullet.transform.position.z < Range_z.x || bullet.transform.position.z > Range_z.y)
                    {
                        allBulletList.Remove(bullet);
                        Destroy(bullet.gameObject);
                    }
                    break;
                }
            }
        }
    }


    public void BulletMove()
    {
        foreach(Bullet bullet in allBulletList)
        {
            if (bullet.targetMonster != null)
            {
                switch (bullet.bulletData.shootingType)
                {
                    case BulletShootingType.Follow:
                    {
                        Vector3 dir = bullet.targetMonster.GetComponentInChildren<Socket>().transform.position - bullet.transform.position;
                        float dist = dir.magnitude;
                        dir.Normalize();

                        float rotAngle = Vector3.Angle(bullet.transform.forward, dir);
                        float rotDir = Vector3.Dot(bullet.transform.right, dir) < 0.0f ? -1.0f : 1.0f;

                        float rotateAmount = _rotSpeed * Time.deltaTime;
                        if (rotAngle < rotateAmount) rotateAmount = rotAngle;
                        bullet.transform.Rotate(Vector3.up * rotDir * rotateAmount);

                        float moveAmount = bullet.bulletData.speed * Time.deltaTime;
                        if (dist < moveAmount) moveAmount = dist;
                        bullet.transform.Translate(dir * moveAmount, Space.World);

                        break;
                    }
                    case BulletShootingType.Straight:
                    {
                        bullet.transform.Translate(transform.forward * bullet.bulletData.speed * Time.deltaTime, Space.Self);
                        break;
                    }
                }
            }
        }
    }

    public void SpawnBullet(Unit unit)
    {
        Transform socket = unit.GetComponentInChildren<Socket>().transform;

        GameObject obj = Instantiate(bulletPrefabsArray[unit.unitData.bulletKey - 1], socket.position, socket.rotation);
        Bullet bullet = obj.GetComponent<Bullet>();
        bullet.bulletOwner = unit;
        bullet.bulletKey = unit.unitData.bulletKey;
        bullet.targetMonster = unit.targetMonster;

        bullet.bulletData = GameWorld.Instance.BalanceManager.bulletDic[bullet.bulletKey];
        bullet.transform.localScale = Vector3.one * bullet.bulletData.scale;

        allBulletList.Add(bullet);
    }
}