using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerActivator : MonoBehaviour
{
    [SerializeField] GameObject fakePlatform;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            fakePlatform.GetComponent<BoxCollider>().isTrigger = true;
        }
    }
}
