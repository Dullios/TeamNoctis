using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerList : MonoBehaviour
{
    [SerializeField] Canvas towerListBGCanvas;

    PlayerController playerController;

    // Start is called before the first frame update
    void Start()
    {
        towerListBGCanvas = GetComponentInParent<Canvas>();
        towerListBGCanvas.enabled = false;

        playerController = FindObjectOfType<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        // tower list 
        if (Input.GetKeyDown(KeyCode.I))
        {
            ToggleTowerList();
            
        }
    }

    public void ToggleTowerList()
    {
        if (towerListBGCanvas.enabled == false)
        {
            CursorState(true);
            towerListBGCanvas.enabled = true;
            Time.timeScale = 0.0f;
        }
        else
        {
            CursorState(false);
            towerListBGCanvas.enabled = false;
            Time.timeScale = 1.0f;
        }
    }

    public void CursorState(bool state)
    {
        if (Application.platform == RuntimePlatform.Android)
            return; //Ignore Cursor States on phone
        Cursor.lockState = state ? CursorLockMode.None : CursorLockMode.Locked;
    }
}
