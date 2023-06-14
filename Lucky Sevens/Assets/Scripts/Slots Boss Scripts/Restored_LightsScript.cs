using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Restored_LightsScript : MonoBehaviour
{
    [SerializeField] GameObject lightObj;

    private void OnTriggerEnter(Collider other)
    {
        lightObj.SetActive(true);
    }
}
