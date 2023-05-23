using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyButton : MonoBehaviour
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
        SlotsController.instance.StunWheel();
        WinnersToken.instance.Spawn();
    }

    public void ActivateButton()
    {
        hit = false;
        press.Reset();
    }

    public void DeactivateButton()
    {
        hit = true;
        press.Disarm(); //God the more I code the more I realize my naming scheme is stupid af. 
    }
}
