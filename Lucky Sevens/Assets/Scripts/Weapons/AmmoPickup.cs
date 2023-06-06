using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoPickup : MonoBehaviour, ICollectable
{
    [SerializeField] int ammoAmount;

    GunSystem gunSystem;

    public void onCollect()
    {
        if (gunSystem.weapons[gunSystem.currentWeapon].tag != "Limited")
        {
            gunSystem.AddBullets(ammoAmount);
            GameManager.instance.UpdateAmmoCount();
            Destroy(gameObject);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            gunSystem = GameManager.instance.gunSystem;
            onCollect();
        }
    }
}
