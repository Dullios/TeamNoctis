using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// title scene

public class KeyBind : MonoBehaviour
{
    public Text up, down, left, right, jump;

    private GameObject currentKey;


    // Start is called before the first frame update
    void Start()
    {
        // Debug.Log("This is KeyBind Start.");
        if (GlobalData.HasInstance)
        {
            up.text = GlobalData.instance.keys["UP"].ToString();
            down.text = GlobalData.instance.keys["DOWN"].ToString();
            left.text = GlobalData.instance.keys["LEFT"].ToString();
            right.text = GlobalData.instance.keys["RIGHT"].ToString();
            jump.text = GlobalData.instance.keys["JUMP"].ToString();
        }
    }

    private void OnGUI()
    {
        if(currentKey != null)
        {
            Event e = Event.current;
            if (e.isKey)
            {
                if (GlobalData.HasInstance)
                {
                    GlobalData.instance.keys[currentKey.name] = e.keyCode;
                    currentKey.transform.GetChild(0).GetComponent<Text>().text = e.keyCode.ToString();
                    currentKey.GetComponent<Image>().color = Color.white;
                    currentKey = null;
                }
            }
        }
    }
    public void ChangeKey(GameObject go)
    {
        if(currentKey != null)
        {
            currentKey.GetComponent<Image>().color = Color.white;
        }
        currentKey = go;
        currentKey.GetComponent<Image>().color = Color.grey;
    }
}
