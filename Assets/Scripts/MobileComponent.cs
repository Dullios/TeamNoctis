using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobileComponent : MonoBehaviour
{
    // Shuts off gameobject if running not running mobile
    void Start()
    {
        if (Application.platform != RuntimePlatform.Android)
        {
            gameObject.SetActive(false);
        }
    }

    public void SetActive(bool active)
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            gameObject.SetActive(active);
        }

    }
}

