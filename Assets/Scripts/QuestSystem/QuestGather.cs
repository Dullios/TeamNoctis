using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class QuestGather : QuestBase
{
    //Item id, number of item to get, check item table scriptable object to know item id
    [SerializeField] Dictionary<int, int> listOfItem = new Dictionary<int, int>();

    Dictionary<int, int> listOfCheckingItem; //will be used to count current progress
    Dictionary<int, bool> listOfItemCompleted = new Dictionary<int, bool>(); // check if we gathred this item already

    int itemGatehredCount = 0; //Itme count that we need to gather

    public QuestGather(string _questName, string _questDescription, Dictionary<int, int> _listOfItem)
       : base(_questName, _questDescription)
    {
        listOfItem = _listOfItem;
        listOfCheckingItem = new Dictionary<int, int>(_listOfItem);

        //Reset listOfCheckingItem
        foreach (int itemID in listOfCheckingItem.Keys.ToList())
        {
            listOfCheckingItem[itemID] = 0;
        }

        //Reset complete list
        foreach(int itemID in listOfItem.Keys)
        {
            listOfItemCompleted.Add(itemID, false);
        }
    }

    public override void Start()
    {
        base.Start();

        //Add event
        Inventory.instance.OnAddItem.AddListener(OnItemAddCallback);
    }

    public override void Update()
    {
        base.Update();

        string currentProgressText = "";
        foreach(int itemID in listOfItem.Keys)
        {
            //Get item from item table
            Item item = QuestSystem.instance.itemTable.GetItem(itemID);

            //Make current progress text, item name : current gathered num / required num
            currentProgressText += "\n" + item.itemName + ": " + listOfCheckingItem[itemID].ToString() + "/" + listOfItem[itemID].ToString() + ".";
        }

        //concatenate current progress text to qeust description
        QuestSystem.instance.SetDescription(questDescription + currentProgressText);
    }

    public override void End()
    {
        base.End();

        //Remove event
        Inventory.instance.OnAddItem.RemoveListener(OnItemAddCallback);
    }

    void OnItemAddCallback(int _itemID, int _itemCount)
    {
        if(listOfCheckingItem.ContainsKey(_itemID))
        {
            //Increase item count
            listOfCheckingItem[_itemID] += _itemCount;

            //Check if we got all item
            if(listOfItemCompleted[_itemID] == false &&
                listOfCheckingItem[_itemID] >= listOfItem[_itemID])
            {
                listOfItemCompleted[_itemID] = true;
                ++itemGatehredCount;
            }

            //Check if we clear quest
            if(itemGatehredCount >= listOfItem.Count)
            {
                questCompleted = true;
            }
        }
    }
}
