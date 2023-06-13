using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnOnNewRespawn : MonoBehaviour
{
    public GameObject triggerToSpawn;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
           triggerToSpawn.SetActive(true);
        }
    }
}
