using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalData : Singleton<GlobalData>
{
    public Dictionary<string, KeyCode> keys = new Dictionary<string, KeyCode>();

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
    }
}
