using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
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
    [SerializeField] int bulletsLeft;

    //bools to ask game
    bool allowButtonHolding;
    bool isShooting;
    bool readyToShoot;
    bool reloading;

    public RaycastHit rayHit;

    private void Awake()
    {
        bulletsLeft = magSize;
        readyToShoot = true;
    }
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

        //Raycasting bullets
        RaycastHit hit;
        if (Physics.Raycast(Camera.main.ViewportPointToRay(new Vector2(0.5f, 0.5f)), out hit, range))
        {
            if (rayHit.collider.CompareTag("Enemy"))
            {
                IDamage damageable = hit.collider.GetComponent<IDamage>();
                if (damageable != null)
                {
                    damageable.takeDamage(dmg);
                }
            }
        }
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
