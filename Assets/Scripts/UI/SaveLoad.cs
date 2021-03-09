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

    // Start is called before the first frame update
    void Start()
    {
        playerController = FindObjectOfType<PlayerController>();

        if (TitleButton.loadGame == true)
        {
            playerController.transform.position = new Vector3(PlayerPrefs.GetFloat("PositionX"), PlayerPrefs.GetFloat("PositionY"), PlayerPrefs.GetFloat("PositionZ"));
            playerController.transform.rotation = Quaternion.Euler(PlayerPrefs.GetFloat("RotationX"), PlayerPrefs.GetFloat("RotationY"), PlayerPrefs.GetFloat("RotationZ"));
        }
     

    }

    // Update is called once per frame
    void Update()
    {

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

    public void GoSave()
    {
        // save position
        PlayerPrefs.SetFloat("PositionX", playerController.transform.position.x);
        PlayerPrefs.SetFloat("PositionY", playerController.transform.position.y);
        PlayerPrefs.SetFloat("PositionZ", playerController.transform.position.z);
        Debug.Log("Save! position" + playerController.transform.position.x + " " + playerController.transform.position.y + " " + playerController.transform.position.z);

        // save rotation
        PlayerPrefs.SetFloat("RotationX", playerController.transform.eulerAngles.x);
        PlayerPrefs.SetFloat("RotationY", playerController.transform.eulerAngles.y);
        PlayerPrefs.SetFloat("RotationZ", playerController.transform.eulerAngles.z);
        Debug.Log("Save! rotaition" + playerController.transform.eulerAngles.x + " " + playerController.transform.eulerAngles.y + " " + playerController.transform.eulerAngles.z);

    }

    public void GoLoad()
    {
        playerController.transform.position = new Vector3(PlayerPrefs.GetFloat("PositionX"), PlayerPrefs.GetFloat("PositionY"), PlayerPrefs.GetFloat("PositionZ"));
        Debug.Log("Load! position" + playerController.transform.position.x + " " + playerController.transform.position.y + " " + playerController.transform.position.z);

        playerController.transform.rotation = Quaternion.Euler(PlayerPrefs.GetFloat("RotationX"), PlayerPrefs.GetFloat("RotationY"), PlayerPrefs.GetFloat("RotationZ"));
        Debug.Log("Load! rotaition" + playerController.transform.eulerAngles.x + " " + playerController.transform.eulerAngles.y + " " + playerController.transform.eulerAngles.z);
    }
}
