using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class QuestSystem : Singleton<QuestSystem>
{
    //[SerializeReference] can serizlize child class
    [SerializeReference] List<QuestBase> listOfQuest;

    QuestBase currentQuest = null;

    [Header("Item Table")]
    public ItemTable itemTable;

    [Header("Text References")]
    [SerializeField] TMP_Text questNameText;
    [SerializeField] TMP_Text questDescriptionText;
    [SerializeField] TMP_Text questStatusText;

    // components
    BuildingComponent buildingComponent;

    // Canvas
    Canvas questCanvas;

    PlayerController playerController;
    CameraController cameraController;

    // Start is called before the first frame update
    void Start()
    {
        playerController = FindObjectOfType<PlayerController>();
        cameraController = FindObjectOfType<CameraController>();

        //Added input quest
        List<KeyCode> detectingKeys = new List<KeyCode>();
        detectingKeys.Add(KeyCode.W);
        detectingKeys.Add(KeyCode.A);
        detectingKeys.Add(KeyCode.S);
        detectingKeys.Add(KeyCode.D);
        List<string> detectingJoystickMoveDirection = new List<string>();
        detectingJoystickMoveDirection.Add("Forward");
        detectingJoystickMoveDirection.Add("Backward");
        detectingJoystickMoveDirection.Add("Right");
        detectingJoystickMoveDirection.Add("Left");
        QuestInput questInput = new QuestInput("Movement", "Use W, A, S, D to move or Use left joystick to move", detectingKeys, 
            playerController.joystickMove, detectingJoystickMoveDirection);
        listOfQuest.Add(questInput);

        //Add look around quest

        List<string> detectingJoystickLookDirection = new List<string>();
        detectingJoystickLookDirection.Add("Forward");
        detectingJoystickLookDirection.Add("Backward");
        detectingJoystickLookDirection.Add("Right");
        detectingJoystickLookDirection.Add("Left");
        QuestInput questInput4 = new QuestInput("Look around", "Use right joystick to look around", null,
            cameraController.joystickLook, detectingJoystickLookDirection);
        listOfQuest.Add(questInput4);

        //Added input quest
        List<KeyCode> detectingKeys2 = new List<KeyCode>();
        detectingKeys2.Add(KeyCode.Space);
        QuestInput questInput2 = new QuestInput("Jump", "Use Space to jump", detectingKeys2);
        listOfQuest.Add(questInput2);

        //Added gather quest, (item id, item number), check item table scriptable object to know item id
        Dictionary<int, int> listOfItem = new Dictionary<int, int>();
        listOfItem.Add(0, 3);
        listOfItem.Add(1, 3);
        QuestGather questGather = new QuestGather("Gather resources", "Hold left mouse button to collect resources.", listOfItem);
        listOfQuest.Add(questGather);

        // Added open Tower List Quest
        List<KeyCode> detectingKeys3 = new List<KeyCode>();
        detectingKeys3.Add(KeyCode.I);
        QuestInput questInput3 = new QuestInput("Tower List", "Use i to Open", detectingKeys3);
        listOfQuest.Add(questInput3);

        // Added place tower quest
        buildingComponent = FindObjectOfType<BuildingComponent>();
        QuestPlaceTower questPlaceTower = new QuestPlaceTower("Place Tower", "Click any tower on the List and Place to the Ground", buildingComponent);
        listOfQuest.Add(questPlaceTower);


        // Get Canvas Component
        questCanvas = GetComponent<Canvas>();

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
                currentQuest = null;
                questStatusText.text = "Completed";

                Invoke(nameof(StartNextQuest), 2.0f);
            }
        }
    }

    void StartNextQuest()
    {
        if (listOfQuest.Count != 0)
        {
            //Call end function of current quest
            if (currentQuest != null)
            {
                currentQuest.End();
            }

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

            // disable canvas after 2 secs
            Invoke("DisableCanvas", 2.0f);
        }
    }

    public void DisableCanvas()
    {
        // disable canvas after finishing all quests
        questCanvas.enabled = false;
    }

    public void SetDescription(string newText)
    {
        questDescriptionText.text = newText;
    }
}
