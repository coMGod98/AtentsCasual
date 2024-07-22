using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using EasyUI.Toast;

public class UIManager : MonoBehaviour
{
    private int[] buttonClicks = new int[3];

    [Header("Info")]
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI roundText;
    public TextMeshProUGUI monsterCountText;

    [Header("Panel")]
    public GameObject gameWin;
    public GameObject gameLost;

    [Header("Gold")]
    public TextMeshProUGUI curGold;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        UpdateUI();
        GoldUI();
    }

    private void UpdateUI()
    {
        timerText.text = (GameWorld.Instance.remainTime < 10 ? GameWorld.Instance.remainTime.ToString("F1") : GameWorld.Instance.remainTime.ToString("F0"));
        roundText.text = $"{GameWorld.Instance.curRound.ToString()}/{GameWorld.Instance.totalRounds.ToString()}";
        monsterCountText.text = "" + GameWorld.Instance.MonsterManager.allMonsterList.Count;
    }

    private void GoldUI()
    {
        curGold.text = "" + GameWorld.Instance.playerGolds.ToString();
    }

    public void Button1()
    {
        if (GameWorld.Instance.playerGolds >= buttonClicks[0] * 5)
        {
            buttonClicks[0]++;
            GameWorld.Instance.TakeGold(buttonClicks[0] * 5);
        }
        else
        {
            int neededGold = (buttonClicks[0] * 5) - GameWorld.Instance.playerGolds;
            Toast.Show("��尡 �����մϴ�. <size=25> \n" + neededGold.ToString() + " ��尡 �� �ʿ��մϴ� </size> ", 2f, ToastColor.Magenta, ToastPosition.MiddleCenter);
        }
    }

    public void Button2()
    {
        if (GameWorld.Instance.playerGolds >= buttonClicks[1] * 5)
        {
            buttonClicks[1]++;
            GameWorld.Instance.TakeGold(buttonClicks[1] * 5);
        }
        else
        {
            int neededGold = (buttonClicks[1] * 5) - GameWorld.Instance.playerGolds;
            Toast.Show("��尡 �����մϴ�. <size=25> \n" + neededGold.ToString() + " ��尡 �� �ʿ��մϴ� </size> ", 2f, ToastColor.Magenta, ToastPosition.MiddleCenter);
        }
    }

    public void Button3()
    {
        if (GameWorld.Instance.playerGolds >= buttonClicks[2] * 5)
        {
            buttonClicks[2]++;
            GameWorld.Instance.TakeGold(buttonClicks[2] * 5);
        }
        else
        {
            int neededGold = (buttonClicks[2] * 5) - GameWorld.Instance.playerGolds;
            Toast.Show("��尡 �����մϴ�. <size=25> \n" + neededGold.ToString() + " ��尡 �� �ʿ��մϴ� </size> ", 2f, ToastColor.Magenta, ToastPosition.MiddleCenter);
        }
    }

    public void GameOver()
    {
        gameLost.SetActive(true);
    }

    public void GameVictory()
    {
        gameWin.SetActive(true);
    }
}


