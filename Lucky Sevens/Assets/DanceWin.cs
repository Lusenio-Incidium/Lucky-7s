using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DanceWin : MonoBehaviour
{
    [SerializeField] GameObject toSpawn;

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("player"))
            toSpawn.SetActive(true);
    }
}
