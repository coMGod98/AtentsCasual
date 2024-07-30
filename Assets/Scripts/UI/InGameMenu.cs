using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class InGameMenu : MonoBehaviour
{
    public static bool GameIsPaused = false;
    public GameObject inGameMenu;
    public GameObject optionsMenu;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {

            if (optionsMenu.activeSelf)
            {
                optionsMenu.SetActive(false);                
            }
            else
            {
                inGameMenu.SetActive(!inGameMenu.activeSelf);
                if (inGameMenu.activeSelf)
                {
                    Time.timeScale = 0;
                }
                else
                {
                    Time.timeScale = 1.0f;
                }
            }

        }
    }
    
    public void RestartGame()
    {
        //�����
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public void OpenOptions()
    {
        // �ΰ��� �޴��� �ݰ� �ɼ� �޴��� ���ϴ�.
        inGameMenu.SetActive(false);
        optionsMenu.SetActive(true);
    }
    public void CloseOptions()
    {
        // �ɼ� �޴��� �ݰ� �ΰ��� �޴��� ���ϴ�.
        optionsMenu.SetActive(false);
        inGameMenu.SetActive(true);
    }
    public void GoToMainMenu()
    {
        //���θ޴�
        SceneManager.LoadScene("BeginingScene");
        Time.timeScale = 1.0f;
    }
    public void QuitGame()
    {
        //���α׷�����
        Application.Quit();
    }



}



