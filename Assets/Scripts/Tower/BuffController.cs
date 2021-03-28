using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum StatType
{
    HP,
    DAMAGE,
    MOVESPEED,
    RANGE,
    ATTACKSPEED
}

public class BuffController : TowerController
{
    public float buffRange = 10.0f;
    public StatType statBuffed;
    public float buffStrength = 5.0f;
    public float buffDuration = 3.0f;

    private PlayerController player = null;
    private float cooldown = 3.0f;
    private float timer = 3.0f;

    // Start is called before the first frame update
    new private void Start()
    {
        base.Start();
        GetComponent<SphereCollider>().radius = buffRange;
    }

    // Update is called once per frame
    new private void Update()
    {
        if (isActivated && player != null)
        {
            shotPoint.Rotate(0, 30 * Time.deltaTime, 0, Space.World);

            if (timer == cooldown)
            {
                _ApplyBuff();
            }
        }
        //Slow down buff check
        timer -= Time.deltaTime;
        if (timer < 0)
            timer = cooldown;
    }

    new private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            player = other.gameObject.GetComponent<PlayerController>();
        }
    }

    //Remove dead or out of range enemies
    new private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        { 
            player = null;
        }
    }

    //Applies a buff to the player if no buff is currently present
    private void _ApplyBuff()
    {
        BuffBehaviour buff;
        if (player.gameObject.TryGetComponent<BuffBehaviour>(out buff))
        {//Buff already found
            buff.strength = buffStrength;
            buff.duration = buffDuration;
        }
        else
        {
            buff = player.gameObject.AddComponent<BuffBehaviour>();
            buff.strength = buffStrength;
            buff.duration = buffDuration;
        }

        switch (statBuffed)
        {
            case StatType.HP:
                buff.type = StatType.HP;
                break;
            case StatType.DAMAGE:
                buff.type = StatType.DAMAGE;
                break;
            case StatType.MOVESPEED:
                buff.type = StatType.MOVESPEED;
                break;
            case StatType.RANGE:
                buff.type = StatType.RANGE;
                break;
            case StatType.ATTACKSPEED:
                buff.type = StatType.ATTACKSPEED;
                break;
            default:
                break;
        }
    }
}
