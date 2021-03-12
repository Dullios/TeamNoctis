using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ItemTable))]
public class ItemTableEditor : Editor 
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        DrawDefaultInspector();

        ItemTable itemTable = (ItemTable)target;

        if (itemTable)
        {
            if (GUILayout.Button("AssignItemIDs"))
            {
                itemTable.AssignItemIDs();
            }
        }
    }
}
