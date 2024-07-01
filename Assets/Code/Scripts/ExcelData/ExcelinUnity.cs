using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class ExcelinUnity : MonoBehaviour
{



    public TextAsset csvFile;
    public static string[,] data;

    void Start()
    {
        if (csvFile == null)
        {
            Debug.LogError("CSV file is not assigned.");
            return;
        }
        string[] lines = csvFile.text.Split(new char[] { '\n' }); // �� ������ ������ ����
        data = new string[lines.Length, 2]; // 2���� ������ �迭

        for (int i = 0; i < lines.Length; i++)
        {
            if (!string.IsNullOrEmpty(lines[i]))
            {
                string[] row = lines[i].Split(new char[] { ',' }); // ��ǥ ������ ������ ����
                if (row.Length >= 2)
                {
                    data[i, 0] = row[0].Trim(); // ù ��° �� ������ ����
                    data[i, 1] = row[1].Trim(); // �� ��° �� ������ ����
                }
                else
                {
                    Debug.LogWarning("Row does not contain enough columns: " + lines[i]);
                }
               
            }
            
        }
    }
}
// Start is called before the first frame update


    // Update is called once per frame
   

