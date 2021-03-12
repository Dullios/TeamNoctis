using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KeyBind : MonoBehaviour
{
    private Dictionary<string, KeyCode> keys = new Dictionary<string, KeyCode>();

    public Text up, down, left, right;

    private GameObject currentKey;
    // Start is called before the first frame update
    void Start()
    {
        keys.Add("UP", KeyCode.W);
        keys.Add("DOWN", KeyCode.S);
        keys.Add("LEFT", KeyCode.A);
        keys.Add("RIGHT", KeyCode.D);

        up.text = keys["UP"].ToString();
        down.text = keys["DOWN"].ToString();
        left.text = keys["LEFT"].ToString();
        right.text = keys["RIGHT"].ToString();
    }

    private void OnGUI()
    {
        if(currentKey != null)
        {
            Event e = Event.current;
            if (e.isKey)
            {
                keys[currentKey.name] = e.keyCode;
                currentKey.transform.GetChild(0).GetComponent<Text>().text = e.keyCode.ToString();
                currentKey = null;
            }
        }
    }
    public void ChangeKey(GameObject go)
    {
        currentKey = go;
    }
}
