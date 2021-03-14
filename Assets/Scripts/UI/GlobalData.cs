using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalData : Singleton<GlobalData>
{
    public Dictionary<string, KeyCode> keys = new Dictionary<string, KeyCode>();

    public string up;
    public string down;
    public string left;
    public string right;

    protected override void Awake()
    {
        
        base.Awake();
        // Debug.Log(gameObject.GetInstanceID() + " 's Global Data Awake.");

        if (instance != null && instance != this)
        {
            // Debug.Log("Destroy Global Data");
            return;
        }

        // Debug.Log( "After Instance Check " +  gameObject.GetInstanceID() + " 's Global Data Awake.");

        keys.Add("UP", KeyCode.W);
        keys.Add("DOWN", KeyCode.S);
        keys.Add("LEFT", KeyCode.A);
        keys.Add("RIGHT", KeyCode.D);
        keys.Add("JUMP", KeyCode.Space);

        DontDestroyOnLoad(gameObject);
    }

    private void Update()
    {
        up = keys["UP"].ToString();
        down = keys["DOWN"].ToString();
        left = keys["LEFT"].ToString();
        right = keys["RIGHT"].ToString();
    }
}
