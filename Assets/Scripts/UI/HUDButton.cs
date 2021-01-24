using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HUDButton : MonoBehaviour
{
    public GameObject pause;
    public string menuScene;

    private void Start()
    {
        pause.SetActive(false);
    }

    public void GoPause()
    {
        pause.SetActive(true);
    }

    public void GoResme()
    {
        pause.SetActive(false);
    }

    public void GoMenu()
    {
        SceneManager.LoadScene(menuScene);
    }
}
