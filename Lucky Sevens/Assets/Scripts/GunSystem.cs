using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunSystem : MonoBehaviour
{
    //Stats
    [Header("----- Gun Stats -----")]
    [SerializeField] int dmg;
    [SerializeField] float timeBetweenShots;
    [SerializeField] float timeBetweenShooting;
    [SerializeField] float range;
    [SerializeField] float reloadTime;
    [SerializeField] int magSize;
    [SerializeField] bool triggerHold;
    int bulletsLeft;

    //bools to ask game
    bool allowButtonHolding;
    bool isShooting;
    bool readyToShoot;
    bool reloading;

    private void Update()
    {
        myInput();
    }

    private void myInput()
    {
        //hold to fire or single shot
        if (allowButtonHolding)
        {
            isShooting = Input.GetKey(KeyCode.Mouse0);
        }
        else
        {
            isShooting = Input.GetKeyDown(KeyCode.Mouse0);
        }

        //reloading
        if (Input.GetKeyDown(KeyCode.R) && bulletsLeft < magSize && !reloading)
        {
            Reload();
        }

        //shooting
        if (readyToShoot && isShooting && !reloading && bulletsLeft > 0)
        {
            Shoot();
        }
    }
    
    //shoot function
    private void Shoot()
    {
        readyToShoot = false;
        bulletsLeft--;
        Invoke("ResetShot", timeBetweenShooting);
    }

    //while not shooting
    private void ResetShot()
    {
        readyToShoot = true;
    }

    //reloading function
    private void Reload()
    {
        reloading = true;
        Invoke("ReloadDone", reloadTime);
    }

    //after reloading is complete
    private void ReloadDone()
    {
        bulletsLeft = magSize;
        reloading = false;
    }
}
