using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleButton : MonoBehaviour
{
    public string LoadingScene;
    public GameObject credit;

    private void Start()
    {
        credit.SetActive(false);
    }

    public void GoLoadingScene()
    {
        SceneManager.LoadScene(LoadingScene);
    }

    public void GoCredits()
    {
        credit.SetActive(true);
    }

    public void BackMenu()
    {
        credit.SetActive(false);
    }
}
