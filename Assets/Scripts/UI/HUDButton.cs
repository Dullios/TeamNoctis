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
///  
/// Revision History
/// 2021-01-24: HUDButton
/// </summary>

public class HUDButton : MonoBehaviour
{
    // Pause Panel
    public GameObject pause = null;

    // to load scene
    public string menuScene = string.Empty;

    // boolean for pausing 
    private bool isPause = false;

    private void Start()
    {
        pause.gameObject.SetActive(false);
    }


    // buttons
    public void GoPause()
    {
        if (!isPause)
        {
            isPause = true;
            Time.timeScale = 0;
            pause.gameObject.SetActive(true);
        }
    }

    public void GoResme()
    {
        if (isPause)
        {
            isPause = false;
            Time.timeScale = 1;
            pause.gameObject.SetActive(false);

        }
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
}
