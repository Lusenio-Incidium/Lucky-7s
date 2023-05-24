using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlotsDamageSpot : MonoBehaviour, IDamage
{
    [SerializeField] int health;
    [SerializeField] GameObject boom;
    bool destroyed = false;

    public void takeDamage(int count)
    {
        if (destroyed)
        {
            return;
        }
        health -= count;
        if (health <= 0)
        {
            destroyed = true;
            StartCoroutine(SlotsController.instance.DamageWheel());
            Instantiate(boom, transform.position, transform.rotation);
        }
    }

    public void InstaKill()
    {
        takeDamage(health);
    }
}
