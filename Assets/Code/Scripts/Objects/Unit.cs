using UnityEngine;
using System.Collections.Generic;


[System.Serializable]
public struct UnitStat
{
    public float AttackDelay;
    public float AttackRange;
    public float AttackPoint;
    public int AttackType;
    public int Gold;
}

public class Unit : MonoBehaviour
{  

    public GameObject unitMarker;
    public UnitStat unitStat;
    public Animator unitAnim;
    
    public State unitState;
    public Vector3 destination;
    public Monster targetMonster; 
    public List<Monster> rangeMonster;

    //public float attackElapsed;

    public void Init(){
        unitState = State.Normal;
        unitAnim = GetComponentInChildren<Animator>();
        destination = transform.position;
        targetMonster = null;
        rangeMonster = new List<Monster>();
        //attackElapsed = 0.0f;
    }
}
