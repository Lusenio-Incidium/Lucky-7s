using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageButton : MonoBehaviour
{
    bool hit = false;
    [SerializeField] PhysicalButtonPress press;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player") || hit)
        {
            return;
        }
        hit = true;
        StartCoroutine(SlotsController.instance.OpenHatch());

    }

    public void primeButton()
    {
        hit = false;
        press.Reset();
    }
}
