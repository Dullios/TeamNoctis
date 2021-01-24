using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
/// <summary>
/// Name: Junho Kim
/// Student#: 101136986
/// The Source file name: Loading.cs
/// Date last Modified: 2020-11-13
/// Program description
///  - after 3 secs, move to play game scene
///  - add a slider to let users know the progress of loading
///  
/// Revision History
/// 2020-11-13: Loading Scene 
/// </summary>
/// 
public class Loading : MonoBehaviour
{
    #region Variables
    // Loading Bar made with Unity Slider
    [SerializeField]
    Slider slider = null;

    public string sceneName;

    // Loading status - at first loading is not done yet.
    bool IsDone = false;

    // Loading time
    float fTime = 0.0f;

    // make code not compile at the same time.
    AsyncOperation async_operation;

    #endregion

    #region Unity_Method
    void Start()
    {
        // GameScene is waiting for the showing
        StartCoroutine(StartLoad(sceneName));
    }

    void Update()
    {
        // Loading time is increasing
        fTime += Time.deltaTime;

        // Loading bar goes up
        slider.value = fTime;

        // after 3 seconds
        if (fTime >= 3.0f)
        {
            // reset
            fTime = 0.0f;

            // open the Scene in the StartCoroutine, GameScene
            async_operation.allowSceneActivation = true;
        }
    }

    #endregion

    #region Custom_Method
    // Before Start Scene, Load Loading Scene
    public IEnumerator StartLoad(string strSceneName)
    {
        async_operation = SceneManager.LoadSceneAsync(strSceneName);
        async_operation.allowSceneActivation = false;

        // After Load loading scene
        if (IsDone == false)
        {
            // boolean stauts changes to ture
            IsDone = true;

            // if progress bar is smaller than max progress = 1.0f
            while (async_operation.progress < 0.9f)
            {
                // Set Loading bar value
                slider.value = async_operation.progress;
                yield return true;
            }
        }
    }
    #endregion
}

