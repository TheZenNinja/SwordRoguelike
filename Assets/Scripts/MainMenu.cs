using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject main, help;

    private void Start()
    {
        ShowMain();
    }


    public void Exit()
    {
        Application.Quit();
    }
    public void Play()
    {
        SceneManager.LoadScene("MainScene");
    }
    public void ShowHelp()
    {
        main.SetActive(false);
        help.SetActive(true);
    }
    public void ShowMain()
    {
        main.SetActive(true);
        help.SetActive(false);
    }
}
