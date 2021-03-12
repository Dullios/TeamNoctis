using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SaveLoad : MonoBehaviour
{
    //public static UnityEvent OnSave = new UnityEvent();
    //public static UnityEvent OnLoad = new UnityEvent();

    // components for saving and loading 
    [SerializeField] PlayerController playerController;
    [SerializeField] CameraController cameraController;
    [SerializeField] Stats stats;
    [SerializeField] DayCycleManager dayCycleManager;
    // Start is called before the first frame update
    void Start()
    {
        // find components
        playerController = FindObjectOfType<PlayerController>();
        cameraController = FindObjectOfType<CameraController>();
        stats = FindObjectOfType<Stats>();
        dayCycleManager = FindObjectOfType<DayCycleManager>();

        // if there is game data to load, load game
        if (TitleButton.loadGame == true)
        {
            // Player - Position & rotation 
            playerController.transform.position = new Vector3(PlayerPrefs.GetFloat("PositionX"), PlayerPrefs.GetFloat("PositionY"), PlayerPrefs.GetFloat("PositionZ"));
            playerController.transform.rotation = Quaternion.Euler(0, PlayerPrefs.GetFloat("RotationY"), 0);
            cameraController.XRotation = PlayerPrefs.GetFloat("RotationX");

            // Player - Stats
            stats.currnetHP = PlayerPrefs.GetFloat("HP");
            stats.currentStamina = PlayerPrefs.GetFloat("STAMINA");

            // Day Night Cycle
            dayCycleManager.cycleCurrent = PlayerPrefs.GetFloat("DayNight");
        }

    }

    public void GoSave()
    {
        // save position
        PlayerPrefs.SetFloat("PositionX", playerController.transform.position.x);
        PlayerPrefs.SetFloat("PositionY", playerController.transform.position.y);
        PlayerPrefs.SetFloat("PositionZ", playerController.transform.position.z);

        // save rotation - player only y-axis
        PlayerPrefs.SetFloat("RotationY", playerController.transform.eulerAngles.y);

        // save rotation - camera only x-axis
        PlayerPrefs.SetFloat("RotationX", cameraController.transform.eulerAngles.x);


        // save player stats - hp & stamina
        PlayerPrefs.SetFloat("HP", stats.currnetHP);
        PlayerPrefs.SetFloat("STAMINA", stats.currentStamina);

        // save day night cycle
        PlayerPrefs.SetFloat("DayNight", dayCycleManager.cycleCurrent);
    }

    public void GoLoad()
    {
        // load position
        playerController.transform.position = new Vector3(PlayerPrefs.GetFloat("PositionX"), PlayerPrefs.GetFloat("PositionY"), PlayerPrefs.GetFloat("PositionZ"));

        // load rotation - player (0,y,0)
        playerController.transform.rotation = Quaternion.Euler(0, PlayerPrefs.GetFloat("RotationY"), 0);

        // load rotation - player (x,0,0)
        cameraController.XRotation = PlayerPrefs.GetFloat("RotationX");

        // load player stats - HP & Stamina
        stats.currnetHP = PlayerPrefs.GetFloat("HP");
        stats.currentStamina = PlayerPrefs.GetFloat("STAMINA");

        // load day night cycle
        dayCycleManager.cycleCurrent = PlayerPrefs.GetFloat("DayNight");
    }



    //// save button
    //public void SaveBtn()
    //{
    //    if (OnSave != null)
    //    {
    //        OnSave.Invoke();
    //    }
    //    // to save info
    //    PlayerPrefs.Save();

    //    // delete all the save info - for testing in case
    //    //PlayerPrefs.DeleteAll();

    //    Debug.Log("Save");
    //}

    //// load button and also this function is using for load game from title
    //public void LoadBtn()
    //{
    //    if (OnLoad != null)
    //    {
    //        OnLoad.Invoke();
    //    }
    //    Debug.Log("Load");
    //}


    //not using for now - save load


}
