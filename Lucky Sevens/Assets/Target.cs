using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour, IDamage
{
    // Start is called before the first frame update
    public void Start()
    {
        TargetManager.instance.updateTargets(1);
    }
    public void takeDamage(float dmg, Transform pos = null) 
    {
        TargetManager.instance.updateTargets(-1);
        Destroy(gameObject);
    }

    public void instaKill() 
    {
    
    }
}
