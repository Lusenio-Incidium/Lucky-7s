using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Headshot : MonoBehaviour, IDamage
{
    [SerializeField] GameObject baseObj;
    public void takeDamage(float dmg, Transform loc) 
    {
        Debug.Log("Critical!");
        baseObj.GetComponent<IDamage>().takeDamage(dmg*2, loc);
    }

    public void instaKill() 
    {
        
    }
}
