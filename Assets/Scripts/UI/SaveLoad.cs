using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SaveLoad : MonoBehaviour
{
    public static UnityEvent OnSave = new UnityEvent();
    public static UnityEvent OnLoad = new UnityEvent();

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SaveBtn()
    {
        if (OnSave != null)
        {
            OnSave.Invoke();
        }
        // to save info
        PlayerPrefs.Save();

        // delete all the save info - for testing in case
        //PlayerPrefs.DeleteAll();

        Debug.Log("Save");
    }
    public void LoadBtn()
    {
        if (OnLoad != null)
        {
            OnLoad.Invoke();
        }
        Debug.Log("Load");
    }
}
