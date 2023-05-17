using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunPickup : MonoBehaviour
{
    public GunStats gunStat;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //other.GetComponent<GunSystem>().PickUpWeapon(gunStat);
            GunSystem gunSystem = other.GetComponent<GunSystem>();
            if (gunSystem != null)
            {
                gunSystem.PickUpWeapon(gunStat);
                Destroy(gameObject);
            }
        }
    }
}
