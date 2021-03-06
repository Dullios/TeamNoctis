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
    public string NewGameScene = string.Empty;

    // credit panel
    public Canvas credit = null;
    public Canvas option = null;

    // boolean
    bool isCreadt = false;
    bool isOption = false;

    // to start new game and load game
    public static bool newGame;
    public static bool loadGame;

    private void Start()
    {
        option.enabled = false;
        credit.enabled = false;

        newGame = false;
        loadGame = false;
    }

    // new game button
    public void GoNewGame()
    {
        SceneManager.LoadScene(NewGameScene);
        newGame = true;
        loadGame = false;
    }

    // load game button
    public void GoLoadGame()
    {
        SceneManager.LoadScene(NewGameScene);
        newGame = false;
        loadGame = true;
    }

    ///////////////////////// buttons
    public void GoCredits()
    {
        isCreadt = true;
        credit.enabled = true;
    }

    public void GoOption()
    {
        isOption = true;
        option.enabled = true;
    }

    public void BackMenu()
    {
        if (isCreadt && !isOption)
        {
            credit.enabled = false;
            isCreadt = false;
        }
        if (isOption && !isCreadt)
        {
            option.enabled = false;
            isOption = false;
        }
    }
}
