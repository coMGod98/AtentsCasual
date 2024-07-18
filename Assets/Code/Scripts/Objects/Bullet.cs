using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Bullet : MonoBehaviour
{
    public int bulletKey;
    public Monster targetMonster;
    public List<Monster> hitMonsterList;
    public Unit bulletOwner;

    public BulletData bulletData;
    
}
