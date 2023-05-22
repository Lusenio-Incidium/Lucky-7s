using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Unity.VisualScripting;
using UnityEngine;


public class GunSystem : MonoBehaviour
{


    //Stats
    [Header("----- Gun Stats -----")]
    //keeping these stats serialized until i can figure out how to get the newpickup gun to work like in class
    int dmg;
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
    Rigidbody Bullet;

    //bools to ask game
    bool allowButtonHolding;
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
            if (ammunition == 0)
            {
                GameManager.instance.CharZeroReserve();
            }
            else
            {
                GameManager.instance.CharReloading();
                Reload();
            }

        }

        //shooting

        if (readyToShoot && isShooting && !reloading)
        {
            if (bulletsLeft > 0)
            {
                Shoot();
            }
            else
            {
                GameManager.instance.CharEmtpyMag();
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

    //shoot function
    private void Shoot()
    {
        readyToShoot = false;

        //Raycasting bullets
        RaycastHit hit;
        if (Physics.Raycast(Camera.main.ViewportPointToRay(new Vector2(0.5f, 0.5f)), out hit, range))
        {
            
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
        //int previousAmmoCount = bulletsLeft;

        bulletsLeft--;
        bulletsShot++;
        bulletsLeft = weapons[currentWeapon].bulletsLeft = bulletsLeft;
        GameManager.instance.UpdateAmmoCount();

        if (allowButtonHolding && bulletsLeft > 0)
        {
            Invoke("Shoot", timeBetweenShots);
        }
        else
        {
            Invoke("ResetShot", timeBetweenShots);

        }

        //ammoCounts[currentWeapon] = previousAmmoCount;
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
        GameManager.instance.ammoDisplay.SetActive(true);
        
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
        Bullet = weapons[index].bulletPreFab;

       /* if (ammoCounts.ContainsKey(currentWeapon))
        {
            bulletsLeft = Mathf.Clamp(ammoCounts[currentWeapon], 0, magSize);
        }
        else
        {
            bulletsLeft = magSize;
        }
        weapons[currentWeapon].ammunition = magSize * 4 - bulletsShot;*/
        GameManager.instance.UpdateAmmoCount();
        gunModel.mesh = weapons[currentWeapon].model.GetComponent<MeshFilter>().sharedMesh;
        gunMat.material = weapons[currentWeapon].model.GetComponent<MeshRenderer>().sharedMaterial;
    }
    public void AddBullets(int amount)
    {
        ammunition += amount;
        GameManager.instance.playerAmmo = ammunition;
        GameManager.instance.UpdateAmmoCount();
    }
    public void AddStatus(StatusEffectObj data)
    {
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
