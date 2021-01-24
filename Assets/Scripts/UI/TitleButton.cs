using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Group Name: Team Noctis
/// The Source file name: TitleButton.cs
/// Date last Modified: 2021-01-24
/// Program description
///  - Functions for buttons in the title scene
///  - Load loading scene when click the start game button
///  - Can toggle Credit panel
///  
/// Revision History
/// 2021-01-24: TitleButton
/// </summary>

public class TitleButton : MonoBehaviour
{
    // to load scene
    public string LoadingScene = string.Empty;

    // credit panel
    public GameObject credit = null;

    private void Start()
    {
        credit.gameObject.SetActive(false);
    }

    // load scene

    public void GoLoadingScene()
    {
        SceneManager.LoadScene(LoadingScene);
    }


    ///////////////////////// buttons
    public void GoCredits()
    {
        credit.gameObject.SetActive(true);
    }

    public void BackMenu()
    {
        credit.gameObject.SetActive(false);
    }
}
