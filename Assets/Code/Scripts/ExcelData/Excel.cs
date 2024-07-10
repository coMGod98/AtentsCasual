using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using System;

public enum Type
{
    Warrior,Archer,Wizard
}
public enum Rank
{
    Normal,Rare,Epic,Unique,Legendary
}
public class Excel : MonoBehaviour
{
    public static Excel instance;

    //xml ����
    public TextAsset unitDBFileXml;

    //�������� �������� �־ ����ü �ϳ��� �Ѱ��� ����ó�� �����ϰ� ����Ҽ� ����
   struct MonParams
    {
        public Type unitType;
        public Rank unitRank;
        public float attackDealy;
        public float attackRange;
        public float attackPoint;
        public int attackType;
        public int unitGold;
    }

   //��ųʸ��� Ű������ �����̸��� ����� �����̹Ƿ� stringŸ������ �ϰ� ������ �����δ� ����ü�� �̿��� MonParams�� ����
   Dictionary<Type, MonParams> dicMonsters = new Dictionary<Type, MonParams>();
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    private void Start()
    {
        MakeMonsterXML();
    }

    //XML�κ��� �Ķ���� �� �о� ���̱�
    void MakeMonsterXML()
    {
        XmlDocument monsterXMLDoc = new XmlDocument();
        monsterXMLDoc.LoadXml(unitDBFileXml.text);

        XmlNodeList monsterNodeList = monsterXMLDoc.GetElementsByTagName("row");

        //��� ����Ʈ�κ��� ������ ��带 �̾Ƴ�
        foreach (XmlNode monsterNode in monsterNodeList)
        {
            MonParams monParams = new MonParams();

            foreach (XmlNode childNode in monsterNode.ChildNodes)
            {
                if (childNode.Name == "unitType")
                {
                    //<name>smallspider</name>
                    monParams.unitType = (Type) Enum.Parse(typeof(Type), childNode.InnerText);
                }

                if (childNode.Name == "unitRank")
                {
                    //<level>1</level>    Int16.Parse() �� ���ڿ��� ������ �ٲ���
                    monParams.unitRank = (Rank)Enum.Parse(typeof(Rank),childNode.InnerText);
                }

                if (childNode.Name == "attackDealy")
                {
                    monParams.attackDealy = Int16.Parse(childNode.InnerText);
                }

                if (childNode.Name == "attackRange")
                {
                    monParams.attackRange = Int16.Parse(childNode.InnerText);
                }
                if (childNode.Name == "attackPoint")
                {
                    monParams.attackPoint = Int16.Parse(childNode.InnerText);
                }
                if (childNode.Name == "attackType")
                {
                    monParams.attackType = Int16.Parse(childNode.InnerText);
                }
                if (childNode.Name == "unitGold")
                {
                    monParams.unitGold = Int16.Parse(childNode.InnerText);
                }
                print(childNode.Name + ": " + childNode.InnerText);
            }
            dicMonsters[monParams.unitType] = monParams;
        }
    }

    //�ܺηκ��� ������ �̸���, EnemyParams ��ü�� ���� �޾Ƽ� �ش� �̸��� ���� ������ 
    //������(XML ���� �о� �� ������)�� ���޹��� EnemyParams ��ü�� �����ϴ� ������ �ϴ� �Լ�
   /* public void LoadMonsterParamsFromXML(string Round,MonsterObject mParams)
    {
        mParams.MonsterHP = dicMonsters[Round].MonsterHP;
        mParams.MonsterGold = mParams.maxHp = dicMonsters[Round].MonsterGold;
        mParams.Movement = dicMonsters[Round].Movement;
        
       
    }*/


    void Update()
    {

    }
}