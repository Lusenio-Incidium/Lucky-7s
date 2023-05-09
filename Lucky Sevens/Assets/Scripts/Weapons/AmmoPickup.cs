using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoPickup : MonoBehaviour, ICollectable
{
    [SerializeField] int ammoAmount;

    public void onCollect() 
    {
        GameManager.instance.playerAmmo += ammoAmount;
        Destroy(gameObject);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) 
        {
            onCollect();
        }
    }
}
