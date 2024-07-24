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

    public void ShowDetails()
    {

    }

    public void Buttons(int buttonIndex, int gold)
    {
        if (GameWorld.Instance.playerGolds >= buttonClicks[buttonIndex] * gold)
        {
            buttonClicks[buttonIndex]++;
            GameWorld.Instance.TakeGold(buttonClicks[buttonIndex] * gold);
        }
        else
        {
            int neededGold = (buttonClicks[buttonIndex] * gold) - GameWorld.Instance.playerGolds;
            Toast.Show("골드가 부족합니다. <size=25> \n" + neededGold.ToString() + " 골드가 더 필요합니다 </size> ", 2f, ToastColor.Black, ToastPosition.MiddleCenter);
        }
    }

    public void LButton1()
    {
        Buttons(0, 5);
    }

    public void LButton2()
    {
        Buttons(1, 10);
    }

    public void LButton3()
    {
        Buttons(2, 15);
    }

    public void RButton1()
    {

    }

    public void RButton2()
    {

    }

    public void RButton3()
    {

    }

    public void addButton1()
    {
        GameWorld.Instance.AddGold(100);
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


