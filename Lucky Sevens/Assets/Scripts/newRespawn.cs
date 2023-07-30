using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class newRespawn : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject newResPoint;
    public GameObject oldResSpawn;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Destroy(oldResSpawn);
            newResPoint.SetActive(true);
            GameManager.instance.playerSpawnPos = GameObject.FindGameObjectWithTag("Player Spawn Pos");
            GameManager.instance.player.transform.position = GameManager.instance.playerSpawnPos.transform.position;
            GameManager.instance.player.transform.rotation = GameManager.instance.playerSpawnPos.transform.rotation;
        }
    }
    
}
