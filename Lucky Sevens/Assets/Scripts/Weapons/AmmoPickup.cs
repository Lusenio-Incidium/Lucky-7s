using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoPickup : MonoBehaviour, ICollectable
{
    [SerializeField] int ammoAmount;


    public void onCollect()
    {
            GameManager.instance.playerScript.AddBullets(ammoAmount);
            GameManager.instance.UpdateAmmoCount();
            ObjectPoolManager.instance.ReturnObjToInfo(gameObject);
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
           if (GameManager.instance.playerScript.hasGun)
           {
               onCollect();
           }
        }
    }
}
