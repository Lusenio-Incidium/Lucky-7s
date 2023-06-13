using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnOnNewRespawn : MonoBehaviour
{
    public GameObject newResPoint;
    public GameObject toNewSpawn;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            toNewSpawn.SetActive(true);
        }
    }
}
