using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoToNewArea : MonoBehaviour
{
    public GameObject newSpawnPos;
    public GameObject oldSpawnPos;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            oldSpawnPos.SetActive(false);
            newSpawnPos.SetActive(true);
        }
    }
}
