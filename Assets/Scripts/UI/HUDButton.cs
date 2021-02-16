using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Group Name: Team Noctis
/// The Source file name: HUDButton.cs
/// Date last Modified: 2021-01-24
/// Program description
///  - Functions for buttons in the HUD and Pause button
///  - Load title scene when click the Menu button
///  - Can toggle Pause panel
///  - Game over buttons and can togle game over scene ("g")
///  
/// Revision History
/// 2021-01-24: HUDButton
/// 2021-01-29: Added buttons in the game over scene
/// </summary>

public class HUDButton : MonoBehaviour
{
    // Pause Panel
    public GameObject pause = null;
    public GameObject gameover = null;
    public GameObject option = null;
    public GameObject gameCanvas = null;
    public GameObject overCanvas = null;

    // to load scene
    public string menuScene = string.Empty;
    public string reStart = string.Empty;

    // boolean for pausing 
    [SerializeField] bool isPause = false;

    private void Start()
    {
        pause.gameObject.SetActive(false);
        gameover.gameObject.SetActive(false);
        option.gameObject.SetActive(false);
    }

    // temporaily load game over scene with "G" key only
    private void Update()
    {
        //if (Input.GetKeyDown(KeyCode.G))
        //{
        //    if (!isPause)
        //    {
        //        isPause = true;
        //        Time.timeScale = 0;
        //        SwitchToMenu(true);
        //        gameover.gameObject.SetActive(true);
        //    }
        //    else
        //    {
        //        isPause = false;
        //        Time.timeScale = 1;
        //        SwitchToMenu(false);
        //        gameover.gameObject.SetActive(false);
        //    }
        //}
        if (Input.GetKeyDown(KeyCode.P))
        {
            SwitchToMenu(true);
            GoPause();
        }
    }
    
    public void GoGameOver()
    {
        if (!isPause)
        {
            isPause = true;
            Time.timeScale = 0;
            SwitchToMenu(true);
            gameover.gameObject.SetActive(true);
        }
        else
        {
            isPause = false;
            Time.timeScale = 1;
            SwitchToMenu(false);
            gameover.gameObject.SetActive(false);
        }
    }

    // buttons
    public void GoPause()
    {
        if (!isPause)
        {
            SwitchToMenu(true);
            isPause = true;
            Time.timeScale = 0;
            pause.gameObject.SetActive(true);
        }
        else
        {
            isPause = false;
            Time.timeScale = 1;
            SwitchToMenu(false);
            pause.gameObject.SetActive(false);
        }
    }

    public void GoResme()
    {
        if (isPause)
        {
            SwitchToMenu(false);
            isPause = false;
            Time.timeScale = 1;
            pause.gameObject.SetActive(false);

        }
    }

    public void GoOption()
    {
        SwitchToMenu(true);
        option.gameObject.SetActive(true);
    }

    public void GoBack()
    {
        option.gameObject.SetActive(false);
    }

    // load scene
    public void GoMenu()
    {
        if (isPause)
        {
            isPause = false;
            Time.timeScale = 1;
            SceneManager.LoadScene(menuScene);
        }
    }

    public void GoRestart()
    {
            if (isPause)
            {
                isPause = false;
                Time.timeScale = 1;
                SceneManager.LoadScene(reStart);
                gameover.gameObject.SetActive(false);
            }
    }

    public void SwitchToMenu(bool menues)
    {
        Cursor.lockState = menues ? CursorLockMode.None: CursorLockMode.Locked;
        gameCanvas.SetActive(!menues);
        overCanvas.SetActive(menues);
    }
}
