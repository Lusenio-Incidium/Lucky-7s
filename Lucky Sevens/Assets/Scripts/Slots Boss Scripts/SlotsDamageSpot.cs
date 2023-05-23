using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlotsDamageSpot : MonoBehaviour, IDamage
{
    [SerializeField] int health;


    public void takeDamage(int count)
    {
        health -= count;
        if (health <= 0)
        {
            SlotsController.instance.DamageWheel();
        }
    }

    public void InstaKill()
    {
        takeDamage(health);
    }
}
