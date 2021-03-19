using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class SaveLoad : MonoBehaviour
{
    //public static UnityEvent OnSave = new UnityEvent();
    //public static UnityEvent OnLoad = new UnityEvent();

    // components for saving and loading 
    [SerializeField] PlayerController playerController;
    [SerializeField] CameraController cameraController;
    [SerializeField] Stats stats;
    [SerializeField] DayCycleManager dayCycleManager;

    // tower lists
    public List<GameObject> towers;
    [SerializeField] GameObject buffTowerPrefab;
    [SerializeField] GameObject normalTowerPrefab;
    // scene
    public string loadScene;

    // Start is called before the first frame update
    void Start()
    {
        towers = new List<GameObject>();

        // find components
        playerController = FindObjectOfType<PlayerController>();
        cameraController = FindObjectOfType<CameraController>();
        stats = FindObjectOfType<Stats>();
        dayCycleManager = FindObjectOfType<DayCycleManager>();

        // if there is game data to load, load game
        if (TitleButton.loadGame == true)
        {
            // Player - Position & rotation 
            playerController.transform.position = new Vector3(PlayerPrefs.GetFloat("PositionX"), PlayerPrefs.GetFloat("PositionY"), PlayerPrefs.GetFloat("PositionZ"));
            playerController.transform.rotation = Quaternion.Euler(0, PlayerPrefs.GetFloat("RotationY"), 0);
            cameraController.XRotation = PlayerPrefs.GetFloat("RotationX");

            // Player - Stats
            stats.currnetHP = PlayerPrefs.GetFloat("HP");
            stats.currentStamina = PlayerPrefs.GetFloat("STAMINA");

            // Day Night Cycle
            dayCycleManager.cycleCurrent = PlayerPrefs.GetFloat("DayNight");

            // load inventory
            if (Inventory.HasInstance) // check if there is inventory
            {
                Inventory.instance.LoadInventory();
            }

            // load tower lists
            string TowerListData = PlayerPrefs.GetString("TowerList");

            // devide datas
            char[] delimeters = new char[] { ',' };
            string[] splitData = TowerListData.Split(delimeters);
            int towerCount = 0;

            for (int i = 0; i < splitData.Length; i+=7)
            {
                if (splitData[i] == "Buff")
                {
                    towers.Add(buffTowerPrefab);
                }
                else if (splitData[i] == "Normal")
                {
                    towers.Add(normalTowerPrefab);
                }

                float posX = float.Parse(splitData[i + 1]);
                float posY = float.Parse(splitData[i + 2]);
                float posZ = float.Parse(splitData[i + 3]);

                float rotX = float.Parse(splitData[i + 4]);
                float rotY = float.Parse(splitData[i + 5]);
                float rotZ = float.Parse(splitData[i + 6]);

                Instantiate(towers[towerCount], new Vector3(posX, posY, posZ), Quaternion.Euler(new Vector3(rotX,rotY,rotZ)));
                towerCount++;
            }

        }

    }

    public void GoSave()
    {
        string saveTowerList = "";

        // save position
        PlayerPrefs.SetFloat("PositionX", playerController.transform.position.x);
        PlayerPrefs.SetFloat("PositionY", playerController.transform.position.y);
        PlayerPrefs.SetFloat("PositionZ", playerController.transform.position.z);

        // save rotation - player only y-axis
        PlayerPrefs.SetFloat("RotationY", playerController.transform.eulerAngles.y);

        // save rotation - camera only x-axis
        PlayerPrefs.SetFloat("RotationX", cameraController.transform.eulerAngles.x);

        // save player stats - hp & stamina
        PlayerPrefs.SetFloat("HP", stats.currnetHP);
        PlayerPrefs.SetFloat("STAMINA", stats.currentStamina);

        // save day night cycle
        PlayerPrefs.SetFloat("DayNight", dayCycleManager.cycleCurrent);
        
        // save inventory
        if(Inventory.HasInstance) // check if there is inventory
        {
            Inventory.instance.SaveInventory(); 
        }

        // find towers
        foreach (GameObject go in GameObject.FindGameObjectsWithTag("Tower"))
        {
            // save tower list
            if (go.name == "BuffTower(Clone)")
            {
                saveTowerList += 
                    "Buff" + "," + 
                    go.transform.position.x + "," + go.transform.position.y + "," + go.transform.position.z + "," + 
                    go.transform.eulerAngles.x + "," + go.transform.eulerAngles.y + "," + go.transform.eulerAngles.z + ",";
                towers.Add(go);
            }
            else if(go.name == "Tower(Clone)")
            {
                saveTowerList += 
                    "Normal" + "," +
                    go.transform.position.x + "," + go.transform.position.y + "," + go.transform.position.z + "," +
                    go.transform.eulerAngles.x + "," + go.transform.eulerAngles.y + "," + go.transform.eulerAngles.z + ","; ;
                towers.Add(go);
            }

            PlayerPrefs.SetString("TowerList", saveTowerList);
            //Debug.Log("Tower list" + saveTowerList);

            //if (towers != null)
            //{
            //    for (int i = 0; i < towers.Count; ++i)
            //    {
            //        // position[]
            //        PlayerPrefs.SetFloat("TowerPositionX", towers[i].transform.position.x);
            //        PlayerPrefs.SetFloat("TowerPositionY", towers[i].transform.position.y);
            //        PlayerPrefs.SetFloat("TowerPositionZ", towers[i].transform.position.z);

            //        Debug.Log(towers[i].transform.position.x + "," + towers[i].transform.position.y + "," + towers[i].transform.position.z);

            //        // rotation
            //        PlayerPrefs.SetFloat("TowerRotationX", towers[i].transform.eulerAngles.x);
            //        PlayerPrefs.SetFloat("TowerRotationY", towers[i].transform.eulerAngles.y);
            //        PlayerPrefs.SetFloat("TowerRotationZ", towers[i].transform.eulerAngles.z);

            //        Debug.Log(towers[i].transform.eulerAngles.x + "," + towers[i].transform.eulerAngles.y + "," + towers[i].transform.eulerAngles.z);
            //    }
            //}
        }

    }

    public void GoLoad()
    {
        TitleButton.loadGame = true;
        Time.timeScale = 1.0f;
        SceneManager.LoadScene(loadScene);


        //// load position
        //playerController.transform.position = new Vector3(PlayerPrefs.GetFloat("PositionX"), PlayerPrefs.GetFloat("PositionY"), PlayerPrefs.GetFloat("PositionZ"));

        //// load rotation - player (0,y,0)
        //playerController.transform.rotation = Quaternion.Euler(0, PlayerPrefs.GetFloat("RotationY"), 0);

        //// load rotation - player (x,0,0)
        //cameraController.XRotation = PlayerPrefs.GetFloat("RotationX");

        //// load player stats - HP & Stamina
        //stats.currnetHP = PlayerPrefs.GetFloat("HP");
        //stats.currentStamina = PlayerPrefs.GetFloat("STAMINA");

        //// load day night cycle
        //dayCycleManager.cycleCurrent = PlayerPrefs.GetFloat("DayNight");

        //// load inventory
        //if (Inventory.HasInstance) // check if there is inventory
        //{
        //    Inventory.instance.LoadInventory();
        //}
    }

}
