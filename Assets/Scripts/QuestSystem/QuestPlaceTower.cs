using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestPlaceTower : QuestBase
{
    BuildingComponent buildingComponent;

    public QuestPlaceTower(string _questName, string _questDescription, BuildingComponent _buildingComponent) 
        : base(_questName, _questDescription)
    {
        buildingComponent = _buildingComponent;
    }

    public override void Start()
    {
        base.Start();

        // add event
        buildingComponent.OnPlaceTower.AddListener(OnPlaceTowerCallback);
    }

    public override void Update()
    {
        base.Update();
    }

    public override void End()
    {
        base.End();

        //Remove event
        buildingComponent.OnPlaceTower.RemoveListener(OnPlaceTowerCallback);
    }

    private void OnPlaceTowerCallback()
    {
        questCompleted = true;
    }

}
