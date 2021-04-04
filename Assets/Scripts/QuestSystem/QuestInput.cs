using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class QuestInput : QuestBase
{
    public List<KeyCode> detectingKeys = new List<KeyCode>(); //list of key to detect
    
    int numOfDetectedKey = 0; //number of already detected key
    Dictionary<KeyCode, bool> detectedKeys = new Dictionary<KeyCode, bool>(); //will be used if this key has already detected



    Joystick joystickMove; //joy stick
    List<string> detectingJoystickMoveDirection = new List<string>(); //list of joystick move direction
    int numOfDetectedJoystickMove = 0; //number of already detected joystick move
    Dictionary<string, bool> detectedJoystirckMove = new Dictionary<string, bool>(); //will be used if this key has already detected

    public QuestInput(string _questName, string _questDescription, List<KeyCode> _detectingKeys, 
        Joystick _joystickMove = null, List<string> _detectingJoystickMoveDirection = null)
        :base(_questName, _questDescription)
    {
        detectingKeys = _detectingKeys;

        joystickMove = _joystickMove;
        detectingJoystickMoveDirection = _detectingJoystickMoveDirection;
    }

    public override void Start()
    {
        base.Start();

        if (detectingKeys != null)
        {
            //Initialize detected key
            foreach (KeyCode detectingKey in detectingKeys)
            {
                detectedKeys.Add(detectingKey, false);
            }
        }


        //Initialize joystick move
        if (joystickMove != null)
        {
            foreach (string direction in detectingJoystickMoveDirection)
            {
                detectedJoystirckMove.Add(direction, false);
            }
        }
    }

    public override void Update()
    {
        base.Update();

        //Detecting keys
        if (detectingKeys != null)
        {
            foreach (KeyCode key in detectingKeys)
            {
                if (Input.GetKeyDown(key))
                {
                    bool hasDetected = false;
                    //check if this key has already detected
                    if (detectedKeys.TryGetValue(key, out hasDetected))
                    {
                        //if not detected
                        if (hasDetected == false)
                        {
                            //check that it's detected
                            detectedKeys[key] = true;
                            ++numOfDetectedKey;
                        }
                    }
                }
            }
        }


        //Detecting move joystick
        if (joystickMove != null)
        {
            foreach (string direction in detectingJoystickMoveDirection)
            {
                //CHeck if we already detected this direction
                bool hasDetected = false;
                if (detectedJoystirckMove.TryGetValue(direction, out hasDetected))
                {
                    if (hasDetected == false)
                    {
                        //if direction is forward
                        if (direction == "Forward")
                        {
                            float z = joystickMove.Vertical;
                            if (z >= 0.5f)
                            {
                                detectedJoystirckMove[direction] = true;
                                ++numOfDetectedJoystickMove;
                            }
                        }
                        //if direction is backward
                        else if (direction == "Backward")
                        {
                            float z = joystickMove.Vertical;
                            if (z <= -0.5f)
                            {
                                detectedJoystirckMove[direction] = true;
                                ++numOfDetectedJoystickMove;
                            }
                        }
                        //if direction is left
                        else if (direction == "Right")
                        {
                            float x = joystickMove.Horizontal;
                            if (x >= 0.5f)
                            {
                                detectedJoystirckMove[direction] = true;
                                ++numOfDetectedJoystickMove;
                            }
                        }
                        //if direction is forward
                        else if (direction == "Left")
                        {
                            float x = joystickMove.Horizontal;
                            if (x <= -0.5f)
                            {
                                detectedJoystirckMove[direction] = true;
                                ++numOfDetectedJoystickMove;
                            }
                        }
                    }
                }
            }
        }


        if (detectingKeys != null)
        {
            //If we detected all keys
            if (numOfDetectedKey >= detectingKeys.Count)
            {
                //quest complete
                questCompleted = true;
            }
        }

        //If we dtected all joystick
        if(joystickMove != null)
        {
            if(numOfDetectedJoystickMove >= detectedJoystirckMove.Count)
            {
                questCompleted = true;
            }
        }

    }
}
