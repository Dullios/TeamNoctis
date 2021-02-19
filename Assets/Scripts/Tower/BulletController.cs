using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    public float bulletSpeed = 5.0f;
    public float bulletLife = 10.0f;

    // Update is called once per frame
    void Update()
    {
        transform.position += transform.forward * bulletSpeed * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        Destroy(this);
    }

    private void OnCollisionEnter(Collision collision)
    {
        Destroy(this);
    }
}
