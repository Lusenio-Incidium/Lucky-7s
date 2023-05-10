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
    [SerializeField] float range;
    [SerializeField] float reloadTime;
    [SerializeField] float spread;
    [SerializeField] int bulletsPerTap;
    [SerializeField] int magSize;
    [SerializeField] int bulletsLeft;
    [SerializeField] int bulletsShot;
    [SerializeField] int ammunition;
    [SerializeField] StatusEffectObj statusEffect;
    [SerializeField] Rigidbody Bullet;

    //bools to ask game
    [SerializeField] bool allowButtonHolding;
    bool isShooting;
    bool readyToShoot;
    bool reloading;

    public RaycastHit rayHit;

    private void Awake()
    {
        bulletsLeft = magSize;
        ammunition = magSize * 4;
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
            GameManager.instance.Reload(true);
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
        GetMagCount();

        Invoke("ResetShot", timeBetweenShots);
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
    public void ReloadDone()
    {
        int bulletsToReload = magSize - bulletsLeft;

        if (ammunition > 0 && bulletsLeft < magSize)
        {
            int reservedAmmo = (int)MathF.Min(ammunition, bulletsToReload);
            bulletsLeft += reservedAmmo;
            ammunition -= reservedAmmo;
        }
        GetAmmoCount();
        GameManager.instance.Reload(false);
        GameManager.instance.activeMenu = null;
        reloading = false;
    }

    public void AddBullets(int amount)
    {
        ammunition += amount;
        GameManager.instance.playerAmmo = ammunition;
    }
    public int GetAmmoCount()
    {
        return ammunition;
    }
    public int GetMagCount()
    {
        return bulletsLeft;
    }
}
