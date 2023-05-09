using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public InputActionReference pauseKey;

    //TODO impliment pause
    //public bool paused;
    //public GameObject pauseScreen;

    private void Start()
    {
        //paused = false;
        pauseKey.action.performed += ctx => TogglePause();
    }

    private void TogglePause()
    {
        SceneManager.LoadScene("MainMenu");

        //paused = !paused;
        //Time.timeScale = paused ? 1f : 0f;
    }
}
