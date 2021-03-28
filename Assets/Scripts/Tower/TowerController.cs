using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerController : MonoBehaviour
{
    public float shotSpeed = 1.0f;
    public float shotRange = 10.0f;
    public GameObject bullet;
    public Transform shotPoint;
    public bool isActivated = true;

    private List<GameObject> enemies;
    private float shotCooldown = 0.0f;

    // Start is called before the first frame update
    protected void Start()
    {
        enemies = new List<GameObject>();
        GetComponent<SphereCollider>().radius = shotRange;
    }

    // Update is called once per frame
    protected void Update()
    {
        if (isActivated)
        {
            //Check if any enemies are available
            if (enemies.Count > 0)
            {//Shoot if not in cooldown
                if (shotCooldown <= 0.0f)
                    OnFire();
                else
                    shotCooldown -= shotSpeed * Time.deltaTime;
            }
        }
    }

    //Behaviour for when tower fires
    protected void OnFire()
    {
        //look at first enemy in list
        shotPoint.LookAt(enemies[0].transform);
        //instantiate bullet with direction of enemy
        Instantiate(bullet, shotPoint);
        //reset cooldown
        shotCooldown = shotSpeed;
    }

    //Add enemies in range to shot list
    protected void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy")
            enemies.Add(other.gameObject);
    }

    //Remove dead or out of range enemies
    protected void OnTriggerExit(Collider other)
    {
        if (other.tag == "Enemy")
            enemies.Remove(other.gameObject);
    }

    protected void OnDestroy()
    {
        enemies.Clear();
        enemies = null;
    }
}
