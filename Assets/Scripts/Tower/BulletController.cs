using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    public float bulletSpeed = 5.0f;
    public float bulletLife = 5.0f;
    public float bulletDamage = 1.0f;

    // Update is called once per frame
    void Update()
    {
        transform.position += transform.forward * bulletSpeed * Time.deltaTime;
        //Bullet Lifetime
        bulletLife -= Time.deltaTime;
        if (bulletLife < 0) { Destroy(this); };
    }

    private void OnTriggerEnter(Collider other)
    {
        try
        {
            other.gameObject.GetComponent<Stats>().HpModify(-bulletDamage);
        }
        catch (System.Exception)
        {
            Debug.Log("Bullet entered an incompatible object type");
        }
        Destroy(this);
    }

    private void OnCollisionEnter(Collision collision)
    {
        try
        {
            collision.gameObject.GetComponent<Stats>().HpModify(-bulletDamage);
        }
        catch (System.Exception)
        {
            Debug.Log("Bullet hit an incompatible object type");
        }
        Destroy(this);
    }
}
