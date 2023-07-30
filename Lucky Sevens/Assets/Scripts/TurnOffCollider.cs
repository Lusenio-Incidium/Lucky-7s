using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnOffCollider : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] GameObject objectToTurnOff;

    // Update is called once per frame
    private void OnTriggerEnter(Collider other)
    {
        objectToTurnOff.GetComponent<BoxCollider>().enabled = false;
    }
}
