using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
public class ExcelinUnity : MonoBehaviour
{



    public TextAsset csvFile; // CSV ������ ����Ƽ���� �ε��ϱ� ���� ����

    void Start()
    {
        string[] data = csvFile.text.Split(new char[] { '\n' }); // �� ������ ������ ����

        foreach (string line in data)
        {
            string[] row = line.Split(new char[] { ',' }); // ��ǥ ������ ������ ����

            // ����: ù ��° ���� �� ��° �� ������ ���
            Debug.Log("Column 1: " + row[0] + ", Column 2: " + row[1]);
        }
    }
}
// Start is called before the first frame update


    // Update is called once per frame
   

