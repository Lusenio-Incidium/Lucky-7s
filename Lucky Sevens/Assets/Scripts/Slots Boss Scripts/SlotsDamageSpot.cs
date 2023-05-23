using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlotsDamageSpot : MonoBehaviour, IDamage
{
    [SerializeField] int health;
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
            SlotsController.instance.DamageWheel();
        }
    }

    public void InstaKill()
    {
        takeDamage(health);
    }
}
