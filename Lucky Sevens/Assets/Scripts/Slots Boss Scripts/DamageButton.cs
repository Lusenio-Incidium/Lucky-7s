using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageButton : MonoBehaviour
{
    bool hit = false;
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player") || hit)
        {
            return;
        }
        hit = true;
        SlotsController.instance.DamageWheel();
    }
}
