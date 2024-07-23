using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestAOE : MonoBehaviour
{
    public Button button;
    public GameObject mouseFollowPrefab;
    private bool isFollowing = false;

    // Start is called before the first frame update
    void Start()
    {
        // ��ư Ŭ�� �̺�Ʈ ����
        button.onClick.AddListener(OnButtonClick);
    }

    // Update is called once per frame
    void Update()
    {
        if (isFollowing)
        {
            FollowMouse();
        }
    }

    // ��ư Ŭ�� �� ȣ��Ǵ� �޼���
    void OnButtonClick()
    {
        // isFollowing �÷��׸� true�� �����Ͽ� Update �޼��忡�� ���콺 ��ġ�� ���󰡵��� ��
        isFollowing = true;
        // mouseFollowPrefab Ȱ��ȭ (��Ȱ��ȭ ������ �� �����Ƿ�)
        mouseFollowPrefab.SetActive(true);
    }

    // ���콺�� ���󰡵��� �ϴ� �޼���
    void FollowMouse()
    {
        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z = Camera.main.nearClipPlane; // ī�޶��� ����� �Ÿ� ����

        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(mousePosition);
        mouseFollowPrefab.transform.position = new Vector3(worldPosition.x, worldPosition.y, 0); // Z ���� 0���� ����
    }
}
