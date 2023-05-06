using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlotsCollider_ActiveRead : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        SlotsCollider target = other.GetComponent<SlotsCollider>();
        if(target == null)
        {
            return;
        }
        gameObject.SetActive(false);
    }
}
