using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class QuestSystem : Singleton<QuestSystem>
{
    //[SerializeReference] can serizlize child class
    [SerializeReference] List<QuestBase> listOfQuest;

    QuestBase currentQuest = null;

    [Header("References")]
    [SerializeField] TMP_Text questNameText;
    [SerializeField] TMP_Text questDescriptionText;
    [SerializeField] TMP_Text questStatusText;

    // Start is called before the first frame update
    void Start()
    {
        //Added input quest
        List<KeyCode> detectingKeys = new List<KeyCode>();
        detectingKeys.Add(KeyCode.W);
        detectingKeys.Add(KeyCode.A);
        detectingKeys.Add(KeyCode.S);
        detectingKeys.Add(KeyCode.D);
        QuestInput questInput = new QuestInput("Movement", "Use W, A, S, D to move", detectingKeys);
        listOfQuest.Add(questInput);

        //Added input quest
        List<KeyCode> detectingKeys2 = new List<KeyCode>();
        detectingKeys2.Add(KeyCode.Space);
        QuestInput questInput2 = new QuestInput("Jump", "Use Space to jump", detectingKeys2);
        listOfQuest.Add(questInput2);

        //start first quest
        StartNextQuest();
    }

    // Update is called once per frame
    void Update()
    {
        if(currentQuest != null)
        {
            currentQuest.Update();

            if(currentQuest.questCompleted == true)
            {
                StartNextQuest();
            }
        }
    }

    void StartNextQuest()
    {
        if (listOfQuest.Count != 0)
        {
            //Get next quest
            currentQuest = listOfQuest[0];
            listOfQuest.RemoveAt(0);

            //Set texts
            questNameText.text = currentQuest.questName;
            questDescriptionText.text = currentQuest.questDescription;
            questStatusText.text = "Progressing";

            //Start quest
            currentQuest.Start();
        }
        else
        {
            currentQuest = null;

            questNameText.text = "All quest done";
            questDescriptionText.text = "All quest done";
            questStatusText.text = "Completed";
        }
    }
}
