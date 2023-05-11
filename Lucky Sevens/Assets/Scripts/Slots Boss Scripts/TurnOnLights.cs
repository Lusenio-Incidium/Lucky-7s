using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnOnLights : MonoBehaviour
{
    [SerializeField] GameObject lightTrigger;
    bool hit = false;
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player") || hit)
        {
            return;
        }
        hit = true;
        lightTrigger.SetActive(true);
    }
}
