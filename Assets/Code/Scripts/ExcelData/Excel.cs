using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using System;
public class Excel : MonoBehaviour
{
    public static Excel instance;

    //xml ����
    public TextAsset enemyFileXml;

    //�������� �������� �־ ����ü �ϳ��� �Ѱ��� ����ó�� �����ϰ� ����Ҽ� ����
    struct MonParams
    {
        //xml ���Ϸ� ���� ������ ������ ���ؼ� �̵� �Ķ���� ���� �о� ���̰� ����ü ���� ������ �����ϰ� ����ü�� �̿��Ͽ� �� ���Ϳ��� �Ķ���� ���� ������
        public string Round;
        public int MonsterHP;
        public int MonsterGold;
        public int Movement;
    }

    //��ųʸ��� Ű������ �����̸��� ����� �����̹Ƿ� stringŸ������ �ϰ� ������ �����δ� ����ü�� �̿��� MonParams�� ����
    Dictionary<string, MonParams> dicMonsters = new Dictionary<string, MonParams>();
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
        monsterXMLDoc.LoadXml(enemyFileXml.text);

        XmlNodeList monsterNodeList = monsterXMLDoc.GetElementsByTagName("row");

        //��� ����Ʈ�κ��� ������ ��带 �̾Ƴ�
        foreach (XmlNode monsterNode in monsterNodeList)
        {
            MonParams monParams = new MonParams();

            foreach (XmlNode childNode in monsterNode.ChildNodes)
            {
                if (childNode.Name == "Round")
                {
                    //<name>smallspider</name>
                    monParams.Round = childNode.InnerText;
                }

                if (childNode.Name == "MonsterHP")
                {
                    //<level>1</level>    Int16.Parse() �� ���ڿ��� ������ �ٲ���
                    monParams.MonsterHP = Int16.Parse(childNode.InnerText);
                }

                if (childNode.Name == "MonsterGold")
                {
                    monParams.MonsterGold = Int16.Parse(childNode.InnerText);
                }

                if (childNode.Name == "Movement")
                {
                    monParams.Movement = Int16.Parse(childNode.InnerText);
                }              
                print(childNode.Name + ": " + childNode.InnerText);
            }
            dicMonsters[monParams.Round] = monParams;
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