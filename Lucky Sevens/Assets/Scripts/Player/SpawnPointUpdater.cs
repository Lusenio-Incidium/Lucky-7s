using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPointUpdater : MonoBehaviour
{
    [SerializeField] bool useObjectPos;
    [SerializeField] bool singleTrigger = false;
    [SerializeField] Vector3 newSpawnPos;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (useObjectPos)
            {
                GameManager.instance.playerSpawnPos.transform.position = transform.position;
            }
            else
            {
                GameManager.instance.playerSpawnPos.transform.position = newSpawnPos;
            }
            if (singleTrigger)
            {
                Destroy(gameObject);
            }
        }
    }
}
