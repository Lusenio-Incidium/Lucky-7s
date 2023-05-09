using System;
using System.Collections;
using System.Collections.Generic;
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
    [SerializeField] float spread;
    [SerializeField] int bulletsPerTap;
    [SerializeField] int magSize;
    [SerializeField] int bulletsLeft;
    [SerializeField] int bulletsShot;
    [SerializeField] int ammunition;
    [SerializeField] StatusEffectObj statusEffect;
    [SerializeField] GameObject Bullet;

    //bools to ask game
    [SerializeField] bool allowButtonHolding;
    bool isShooting;
    bool readyToShoot;
    bool reloading;

    public RaycastHit rayHit;

    private void Awake()
    {
        bulletsLeft = magSize;
        ammunition = magSize * 2;
        readyToShoot = true;
    }
    private void Update()
    {
        myInput();
    }

    private void myInput()
    {
        //hold to fire or single shot
        if (allowButtonHolding == true)
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
            //TODO: Fix this to look for IDamage please and thank you!
            IDamage damageable = hit.collider.GetComponent<IDamage>();
            IStatusEffect effectable = hit.collider.GetComponent<IStatusEffect>();
            if (damageable != null)
            {
                damageable.takeDamage(dmg);
            }
            if (effectable != null)
            {
                effectable.ApplyStatusEffect(statusEffect);
            }
        }
        bulletsLeft--;
        bulletsShot++;

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
        int bulletsToReload = magSize - bulletsLeft;

        if (ammunition > 0 && bulletsLeft < magSize)
        {
            ammunition -= bulletsToReload;
            if (ammunition < magSize)
            {
                bulletsLeft = ammunition;
                ammunition = ammunition -= bulletsLeft;
            }
            else
            {
                bulletsLeft = magSize;
            }
        }

        reloading = false;
    }

    public void AddBullets(int amount)
    {
        ammunition += amount;
    }
}