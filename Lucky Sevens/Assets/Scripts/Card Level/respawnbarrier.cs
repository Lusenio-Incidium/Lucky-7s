using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class respawnbarrier : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.instance.playerScript.spawnPlayer();
        }
    }
}
