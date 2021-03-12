using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffBehaviour : MonoBehaviour
{
    public StatType type;
    public float strength;
    public float duration;

    private Stats stats;

    // Start is called before the first frame update
    void Start()
    {
        stats = GetComponent<Stats>();
        //Kill this if no stats available to modify
        if (stats == null)
            Destroy(this);
        //Apply buff to stats
        _Apply();
    }

    // Update is called once per frame
    void Update()
    {
        //Check if Buff is over
        if (duration < 0)
        {
            _Remove();
            Destroy(this);
        }
        duration -= Time.deltaTime;
    }

    private void _Apply()
    {
        switch (type)
        {
            case StatType.HP:
                stats.maxHp += strength;
                stats.currnetHP += strength;
                break;
            case StatType.DAMAGE:
                stats.damage += strength;
                break;
            case StatType.MOVESPEED:
                stats.moveSpeed += strength;
                break;
            case StatType.RANGE:
                stats.attackRadius += strength;
                break;
            case StatType.ATTACKSPEED:
                stats.attackSpeed += strength;
                break;
            default:
                break;
        }
    }

    private void _Remove()
    {
        switch (type)
        {
            case StatType.HP:
                stats.maxHp -= strength;
                if (stats.currnetHP > stats.maxHp)
                    stats.currnetHP = stats.maxHp;
                break;
            case StatType.DAMAGE:
                stats.damage -= strength;
                break;
            case StatType.MOVESPEED:
                stats.moveSpeed -= strength;
                break;
            case StatType.RANGE:
                stats.attackRadius -= strength;
                break;
            case StatType.ATTACKSPEED:
                stats.attackSpeed -= strength;
                break;
            default:
                break;
        }
    }
}
