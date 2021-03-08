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
            Time.timeScale = 0.0f;
        }
    }

    public void ToggleTowerList()
    {
        if (towerListBGCanvas.enabled == false)
        {
            CursorState(true);
            towerListBGCanvas.enabled = true;
        }
        else
        {
            CursorState(false);
            towerListBGCanvas.enabled = false;
        }
    }

    public void CursorState(bool state)
    {
        Cursor.lockState = state ? CursorLockMode.None : CursorLockMode.Locked;
    }
}
