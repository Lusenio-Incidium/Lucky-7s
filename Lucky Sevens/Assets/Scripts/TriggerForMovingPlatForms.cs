using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerForMovingPlatForms : MonoBehaviour
{
    [SerializeField] ItemMover platFormScript;
    [SerializeField] bool triggered;
    // Start is called before the first frame update
    void Start()
    {
        platFormScript = GetComponentInChildren<ItemMover>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            if (!triggered)
            {
                platFormScript.OnButtonPress();
                triggered = true;
            }
        }
    }
}
