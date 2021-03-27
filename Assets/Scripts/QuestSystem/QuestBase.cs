using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class QuestBase
{
    public string questName = "";
    public string questDescription = "";
    public bool questCompleted = false;

    public QuestBase(string _questName, string _questDescription)
    {
        questName = _questName;
        questDescription = _questDescription;
    }

    public virtual void Start()
    {

    }

    public virtual void Update()
    {

    }
}
