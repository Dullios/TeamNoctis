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
    public GameObject option = null;

    // boolean
    bool isCreadt = false;
    bool isOption = false;

    private void Start()
    {
        credit.gameObject.SetActive(false);
        option.gameObject.SetActive(false);
    }

    // new game button
    public void GoNewGame()
    {
        SceneManager.LoadScene(LoadingScene);
    }

    // load game button
    public void GoLoadGame()
    {
        SceneManager.LoadScene(LoadingScene);

    }

    ///////////////////////// buttons
    public void GoCredits()
    {
        isCreadt = true;
        credit.gameObject.SetActive(true);
    }

    public void GoOption()
    {
        isOption = true;
        option.gameObject.SetActive(true);
    }

    public void BackMenu()
    {
        if (isCreadt && !isOption)
        {
            credit.gameObject.SetActive(false);
            isCreadt = false;
        }
        if (isOption && !isCreadt)
        {
            option.gameObject.SetActive(false);
            isOption = false;
        }
    }
}
