using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnOnSpikes : MonoBehaviour
{
    public GameObject spikes;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
           spikes.SetActive(true);
        }
    }
}
