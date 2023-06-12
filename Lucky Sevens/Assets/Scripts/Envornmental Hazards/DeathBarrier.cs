using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathBarrier : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        IDamage damagable = other.GetComponent<IDamage>();
        if (damagable != null)
        {
            damagable.instaKill();
        }
        Debug.Log("Doink");
    }
}
