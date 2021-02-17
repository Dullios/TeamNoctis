using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerList : MonoBehaviour
{
    [SerializeField] Canvas towerListBGCanvas;

    //public CharacterController playerController;
    //public PlayerController playerController;

    // Start is called before the first frame update
    void Start()
    {
        towerListBGCanvas = GetComponentInParent<Canvas>();
        towerListBGCanvas.enabled = false;

        // playerController = GameObject.Find("Player").GetComponent<CharacterController>();
        //playerController = GameObject.Find("Player").GetComponent<PlayerController>();

    }

    // Update is called once per frame
    void Update()
    {
        ToggleTowerList();
    }

    public void ToggleTowerList()
    {
        if (Input.GetKeyDown(KeyCode.I))
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
    }

    public void CursorState(bool state)
    {
        Cursor.lockState = state ? CursorLockMode.None : CursorLockMode.Locked;
    }
}
