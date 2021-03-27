using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class QuestInput : QuestBase
{
    public List<KeyCode> detectingKeys = new List<KeyCode>(); //list of key to detect
    
    int numOfDetectedKey = 0; //number of already detected key
    Dictionary<KeyCode, bool> detectedKeys = new Dictionary<KeyCode, bool>(); //will be used if this key has already detected

    public QuestInput(string _questName, string _questDescription, List<KeyCode> _detectingKeys)
        :base(_questName, _questDescription)
    {
        detectingKeys = _detectingKeys;
    }

    public override void Start()
    {
        base.Start();

        //Initialize detected key
        foreach (KeyCode detectingKey in detectingKeys)
        {
            detectedKeys.Add(detectingKey, false);
        }
    }

    public override void Update()
    {
        base.Update();

        foreach(KeyCode key in detectingKeys)
        {
            if(Input.GetKeyDown(key))
            {
                bool hasDetected = false;
                //check if this key has already detected
                if(detectedKeys.TryGetValue(key, out hasDetected))
                {
                    //if not detected
                    if(hasDetected == false)
                    {
                        //check that it's detected
                        detectedKeys[key] = true;
                        ++numOfDetectedKey;
                    }
                }
            }
        }

        //If we detected all keys
        if(numOfDetectedKey >= detectingKeys.Count)
        {
            //quest complete
            questCompleted = true;
        }

    }
}
