using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Unity.VisualScripting;
using UnityEngine;


public class GunSystem : MonoBehaviour
{
    [Header("----- Audio -----")]
    [SerializeField] AudioSource aud;
    [SerializeField] AudioClip gunShotAud;
    [SerializeField] float gunShotAudVol;


    //Stats
    [Header("----- Gun Stats -----")]
    float dmg;
    float timeBetweenShots;
    float range;
    float reloadTime;
    float spread;
    int bulletsPerTap;
    int magSize;
    int bulletsLeft;
    int ammunition;
    int bulletsShot;
    StatusEffectObj statusEffect;
    [SerializeField] MeshFilter gunModel;
    [SerializeField] MeshRenderer gunMat;


    List<GunStats> gunListOrg = new List<GunStats>();


    //bools to ask game
    bool isShooting;
    bool readyToShoot;
    bool reloading;

    public RaycastHit rayHit;

    public List<GunStats> weapons;
    //private Dictionary<int, int> ammoCounts = new Dictionary<int, int>();
    public int currentWeapon = 0;
    public bool hasGun;


    private void Awake()
    {
        hasGun = false;
        bulletsLeft = magSize;
        ammunition = magSize * 5;
        readyToShoot = true;
    }

    private void Update()
    {
        myInput();
    }

    private void myInput()
    {
        //hold to fire or single shot
        if (hasGun)
        {
            if (weapons[currentWeapon].TriggerHold == true)
            {
                isShooting = Input.GetMouseButton(0);
            }
            else
            {
                isShooting = Input.GetKeyDown(KeyCode.Mouse0);
            }
        }
        //reloading
        if (Input.GetKeyDown(KeyCode.R) && bulletsLeft <= magSize && !reloading)
        {
            if (ammunition == 0)
            {
                GameManager.instance.CharZeroReserve();
            }
            else if (bulletsLeft < magSize)
            {
                GameManager.instance.CharReloading();
                Reload();
            }

        }

        //shooting
        if (!GameManager.instance.isPaused)
        {
            if (!reloading && bulletsLeft == 0 && isShooting)
            {
                GameManager.instance.CharEmtpyMag();
            }
            if (readyToShoot && isShooting && !reloading)
            {
                if (bulletsLeft > 0)
                {
                    Shoot();
                }
            }
            if (Input.GetAxis("Mouse ScrollWheel") > 0 && weapons.Count > 0)
            {
                currentWeapon = (currentWeapon + 1) % weapons.Count;
                EquipWeapon(currentWeapon);
            }
            else if (Input.GetAxis("Mouse ScrollWheel") < 0 && weapons.Count > 0)
            {
                currentWeapon = (currentWeapon - 1 + weapons.Count) % weapons.Count;
                EquipWeapon(currentWeapon);
            }
        }
    }

    //shoot function
    private void Shoot()
    {
        readyToShoot = false;


        Vector3 screenCenter = new Vector3(0.5f, 0.5f, 0);
        Ray screenRay = Camera.main.ViewportPointToRay(screenCenter);

        //Quaternion spreadRotation = Quaternion.Euler(UnityEngine.Random.Range(-spread, spread), UnityEngine.Random.Range(-spread, spread), 0f);
        //Vector3 directionSpread = spreadRotation * screenRay.direction;

        for (int i = 0; i < bulletsPerTap; i++)
        {
            float y = UnityEngine.Random.Range(-spread, spread);
            float x = UnityEngine.Random.Range(-spread, spread);

            Vector3 spreadDirection = screenRay.direction + new Vector3(x, y, 0f);

            //Raycasting bullets
            RaycastHit hit;
            if (Physics.Raycast(screenRay.origin, spreadDirection, out hit, range) && !GameManager.instance.isPaused)
            {
                aud.PlayOneShot(weapons[currentWeapon].gunShotAud, weapons[currentWeapon].gunShotAudVol);

                IDamage damageable = hit.collider.GetComponent<IDamage>();
                IStatusEffect effectable = hit.collider.GetComponent<IStatusEffect>();
                if (damageable != null)
                {
                    if (bulletsLeft == 1)
                    {
                        damageable.takeDamage((float)(dmg * 1.5));
                    }
                    else
                    {
                        damageable.takeDamage(dmg);
                    }
                }
                if (effectable != null)
                {
                    effectable.ApplyStatusEffect(statusEffect);
                }
                Instantiate(weapons[currentWeapon].hitEffect, hit.point, Quaternion.LookRotation(hit.normal));
            }
        }

        bulletsLeft--;
        bulletsShot++;
        bulletsLeft = weapons[currentWeapon].bulletsLeft = bulletsLeft;
        GameManager.instance.playerAmmo -= 1;
        GameManager.instance.UpdateAmmoCount();


        if (isShooting && bulletsLeft > 0)
        {
            Invoke("ResetShot", timeBetweenShots);
        }
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
        GameManager.instance.CharReloading();
        Invoke("ReloadDone", reloadTime);
        Invoke("ResetShot", timeBetweenShots);
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
        reloading = false;
        bulletsLeft = weapons[currentWeapon].bulletsLeft = bulletsLeft;
        ammunition = weapons[currentWeapon].ammunition = ammunition;
        GameManager.instance.UpdateAmmoCount();
        GameManager.instance.activeMenu = null;
    }
    public void PickUpWeapon(GunStats gunStat)
    {
        weapons.Add(gunStat);
        if (weapons.Count == 1)
        {
            EquipWeapon(0);
        }
        hasGun = true;
        GameManager.instance.playerAmmo += ammunition + bulletsLeft;
        GameManager.instance.ammoDisplay.SetActive(true);

    }
    public void restartGun()
    {
        weapons.Clear();

        foreach (GunStats gun in gunListOrg)
        {
            weapons.Add(gun);
        }

        if (weapons.Count == 0)
        {
            hasGun = false;
            GameManager.instance.ammoDisplay.SetActive(false);
            gunModel.mesh = null;
            gunMat.material = null;
        }

        GameManager.instance.UpdateAmmoCount();
    }

    public void updateOrig()
    {
        foreach (GunStats gun in weapons)
        {
            gunListOrg.Add(gun);
        }
    }


    public void updateShop(ShopPickup updates)
    {
        ammunition += updates.tokenAmount;
    }

    public void EquipWeapon(int index)
    {
        currentWeapon = index;

        dmg = weapons[index].damage;
        timeBetweenShots = weapons[index].timeBetweenShots;
        range = weapons[index].range;
        reloadTime = weapons[index].reloadTime;
        spread = weapons[index].spread;
        bulletsPerTap = weapons[index].bulletsPerTap;
        magSize = weapons[index].magSize;
        bulletsLeft = weapons[index].bulletsLeft;
        ammunition = weapons[index].ammunition;
        statusEffect = weapons[index].statusEffect;
        GameManager.instance.UpdateAmmoCount();
        gunModel.mesh = weapons[currentWeapon].model.GetComponent<MeshFilter>().sharedMesh;
        gunMat.material = weapons[currentWeapon].model.GetComponent<MeshRenderer>().sharedMaterial;

        Invoke("ResetShot", timeBetweenShots);
    }
    public void AddBullets(int amount)
    {
        if (ammunition - ammunition >= 0)
            ammunition += amount;
        else
        {
            int amount2 = amount - ammunition;
            bulletsLeft -= amount2;
        }
        GameManager.instance.playerAmmo = ammunition;
        GameManager.instance.UpdateAmmoCount();
    }
    public void AddStatus(StatusEffectObj data)
    {
       weapons[currentWeapon].statusEffect = data;
       statusEffect = data;
    }
    public int GetAmmoCount()
    {
        return ammunition;
    }
    public int GetMagCount()
    {
        return bulletsLeft;
    }
    public float GetReloadTime()
    {
        return reloadTime;
    }
    public void SetReadyToShoot(bool boolean)
    {
        readyToShoot = boolean;
    }
    public void ChipPayment(int amount)
    {
        //ammunition = ammunition - amount;
        ammunition -= amount;
    }
}
