using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Group Name: Team Noctis
/// The Source file name: Option.cs
/// Date last Modified: 2021-01-29
/// Program description
///  - toggle mute
///  
/// Revision History
/// 2021-01-29: create script 
/// </summary>

public class Mute : MonoBehaviour
{
   
    static Mute instance = null;

    public Sprite muteOn = null;
    public Sprite muteOff = null;

    public Button toggleButton = null;

    bool isMute = false;


    #region Static
    private void Awake()
    {
        if(instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            GameObject.DontDestroyOnLoad(gameObject);
        }
    }

    #endregion

    public void GoToggleMute()
    {
        if(PlayerPrefs.GetInt("Mute",0) ==0)
        {
            isMute = true;
            PlayerPrefs.SetInt("Mute", 1);
            AudioListener.volume = 1.0f;
            toggleButton.GetComponent<Image>().sprite = muteOn; 
        }
        else
        {
            isMute = false;
            PlayerPrefs.SetInt("Mute", 0);
            AudioListener.volume = 0.0f;
            toggleButton.GetComponent<Image>().sprite = muteOff;
        }
    }
}
