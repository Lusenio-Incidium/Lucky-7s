using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ProBuilder.MeshOperations;

public class GunSystem : MonoBehaviour
{
    //audio
    [Header("----- Audio -----")]
    [SerializeField] AudioSource aud;
    [SerializeField] AudioClip gunShotAud;
    [SerializeField][Range(0, 1)] float gunShotAudVol;


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
    float recoilAmount;
    float adsReduction;
    bool destroyOnEmpty;
    bool isRecoiling = false;
    private Coroutine recoilCoroutine;
    private Coroutine adsCoroutine;
    private Vector3 currentADSPos;
    private Quaternion currentADSRotation;
    private Coroutine returnCoroutine;
    bool isReturning;

    [SerializeField] CameraController cameraController;
    [Header("-----HipFire-----")]
    [SerializeField] Vector3 originolPosition;
    [SerializeField] Quaternion originolRotation;
    [Header("-----ADS-----")]
    [SerializeField] Vector3 aimPosition;
    [SerializeField] Quaternion aimRotation;
    StatusEffectObj statusEffect;
    [Header("-----Weapon Model-----")]
    [SerializeField] MeshFilter gunModel;
    [SerializeField] MeshRenderer gunMat;
    GameObject explosion;


    List<GunStats> gunListOrg = new List<GunStats>();


    //bools to ask game
    bool isShooting;
    bool reloading;

    public RaycastHit rayHit;

    [Header("-----Weapon Info-----")]
    public List<GunStats> weapons;
    public int currentWeapon = 0;
    public bool hasGun;
    public bool readyToShoot;
    public bool currentlyShooting;
    [SerializeField] float shakeIntensity;
    [SerializeField] float recoilDistance;
    private bool isAnimating = false;

    ReticleSpread reticleSpread;
    private void Start()
    {
        if (MainMenuManager.instance != null)
        {
            gunShotAudVol = MainMenuManager.instance.SFXVolume / 10f;
        }
        cameraController = GetComponentInChildren<CameraController>();
        originolPosition = gunModel.transform.localPosition;
        originolRotation = gunModel.transform.localRotation;
        aimPosition = gunModel.transform.localPosition;
        aimRotation = gunModel.transform.localRotation;
    }


    private void Awake()
    {
        hasGun = false;
        bulletsLeft = magSize;
        ammunition = magSize * 5;
        readyToShoot = true;
        currentlyShooting = false;
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
            weapons[currentWeapon].gunShotAudVol = GameManager.instance.playerScript.GetJumpVol();
            reticleSpread = GameManager.instance.activeRetical.GetComponent<ReticleSpread>();
            if (weapons[currentWeapon].TriggerHold == true)
            {
                isShooting = Input.GetMouseButton(0);
            }
            else
            {
                isShooting = Input.GetKeyDown(KeyCode.Mouse0);
            }
            if (bulletsLeft == 0)
                currentlyShooting = false;
            else
                currentlyShooting = isShooting;


            //ADS

            if (Input.GetMouseButton(1))
            {
                float reducedSpread = reticleSpread.currentSize * adsReduction;
                if (adsCoroutine == null)
                {
                    adsCoroutine = StartCoroutine(ADSAnimation());
                    if (returnCoroutine != null)
                    {
                        StopCoroutine(returnCoroutine);
                        returnCoroutine= null;
                    }
                }
                reticleSpread.currentSize = reducedSpread;
                StartADSAnimation();
            }
            else
            {
                if (adsCoroutine != null)
                {
                    StopCoroutine(adsCoroutine);
                    adsCoroutine = null;
                    isReturning= false;
                }
                if (!isReturning)
                {
                    returnCoroutine = StartCoroutine(ReturnAnimation());
                }
                StartReturnAnimation();
            }

        }
        //reloading
        if (Input.GetKeyDown(KeyCode.R) && bulletsLeft <= magSize && !reloading)
        {
            if (GameManager.instance.playerAmmo == 0)
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
        if (GameManager.instance.activeMenu == null)
        {
            if (!reloading && bulletsLeft == 0 && isShooting)
            {
                Reload();
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

        for (int i = 0; i < bulletsPerTap; i++)
        {
            float y = UnityEngine.Random.Range((-reticleSpread.currentSize / reticleSpread.maxSize) / 15, (reticleSpread.currentSize / reticleSpread.maxSize) / 15);
            float x = UnityEngine.Random.Range((-reticleSpread.currentSize / reticleSpread.maxSize) / 15, (reticleSpread.currentSize / reticleSpread.maxSize) / 15);

            Vector3 spreadDirection = screenRay.direction + new Vector3(x, y, 0f);

            //Raycasting bullets
            aud.PlayOneShot(weapons[currentWeapon].gunShotAud, weapons[currentWeapon].gunShotAudVol);

            RaycastHit hit;
            if (Physics.Raycast(screenRay.origin, spreadDirection, out hit, range) && !GameManager.instance.isPaused)
            {
                if (destroyOnEmpty)
                {
                    Instantiate(explosion, hit.point, Quaternion.LookRotation(hit.normal));
                }

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

                PlayerController pc = GameManager.instance.player.GetComponent<PlayerController>();
                if (pc != null)
                {
                    Vector3 pushDir = -transform.forward;
                    pc.TakePush(pushDir * weapons[currentWeapon].pushBackForce);
                }
            }
        }

        bulletsLeft--;
        bulletsShot++;
        bulletsLeft = weapons[currentWeapon].bulletsLeft = bulletsLeft;
        
        GameManager.instance.UpdateAmmoCount();

        GameManager.instance.ammoUsedTotal += 1;

        if (destroyOnEmpty == true)
        {
            if (bulletsLeft == 0 && ammunition == 0)
            {
                DestroyCurrentWeapon();
            }
        }

        if (isShooting && bulletsLeft > 0)
        {
            isRecoiling = true;
            Invoke("ResetShot", timeBetweenShots);
        }

        cameraController.ApplyRecoil(recoilAmount);
        StartCoroutine(ShakeGun());
    }

    private void StartADSAnimation()
    {
        if (isAnimating)
        {
            return;
        }

        if (returnCoroutine != null)
        {
            StopCoroutine(returnCoroutine);
            returnCoroutine = null;
        }
        adsCoroutine = StartCoroutine(ADSAnimation());
    }
    
    private void StartReturnAnimation()
    {
        if (isAnimating)
        {
            return;
        }

        if (adsCoroutine != null)
        {
            StopCoroutine(adsCoroutine);
            adsCoroutine = null;
        }
        returnCoroutine = StartCoroutine(ReturnAnimation());
    }
    private IEnumerator ADSAnimation()
    {
        isAnimating = true;
        float elapsedTime = 0f;
        float duration = 0.1f;
        Vector3 initialPosition = gunModel.transform.localPosition;
        Quaternion initialRotation = gunModel.transform.localRotation;

        while (elapsedTime < duration)
        {
            gunModel.transform.localPosition = Vector3.Lerp(initialPosition, aimPosition, elapsedTime / duration);
            gunModel.transform.localRotation = Quaternion.Slerp(initialRotation, aimRotation, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        gunModel.transform.localPosition = aimPosition;
        gunModel.transform.localRotation = aimRotation;

        adsCoroutine = null;
        returnCoroutine = null;
        isAnimating = false;
    }

    private IEnumerator ReturnAnimation()
    {
        isAnimating = true;
        isReturning = true;
        float elapsedTime = 0f;
        float duration = 0.3f;
        Vector3 initialPosition = gunModel.transform.localPosition;
        Quaternion initialRotation = gunModel.transform.localRotation;

        while (elapsedTime < duration)
        {
            gunModel.transform.localPosition = Vector3.Lerp(initialPosition, originolPosition, elapsedTime / duration);
            gunModel.transform.localRotation = Quaternion.Slerp(initialRotation, originolRotation, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        gunModel.transform.localPosition = originolPosition;
        gunModel.transform.localRotation = originolRotation;
        isReturning = false;
        returnCoroutine = null;
        isAnimating= false;
    }
    private IEnumerator ShakeGun()
    {
        Quaternion originalRotation = gunModel.transform.localRotation;
        float elapsedTime = 0f;

        while (elapsedTime < 0.2f)
        {
            float shakeX = Random.Range(-1f, 1f) * shakeIntensity;
            float shakeY = Random.Range(-1f, 1f) * shakeIntensity;

            gunModel.transform.localRotation = originalRotation * Quaternion.Euler(shakeX, shakeY, 0f);

            elapsedTime += Time.deltaTime;
            yield return null;
        }
        gunModel.transform.localRotation = originalRotation;
    }
    public bool HasWeaponWithTag(string weaponTag)
    {
        foreach(GunStats weapon in weapons)
        {
            if (weapon.tag == weaponTag)
            {
                return true;
            }
        }
        return false;
    }

    public void DestroyCurrentWeapon()
    {
        weapons.RemoveAt(currentWeapon);
        currentWeapon = Mathf.Clamp(currentWeapon, 0, weapons.Count - 1);

        if (weapons.Count == 0)
        {
            hasGun = false;
            GameManager.instance.ammoDisplay.SetActive(false);
            gunModel.mesh = null;
            gunMat.material = null;
        }
        else
        {
            EquipWeapon(currentWeapon);
        }
    }

    //while not shooting
    private void ResetShot()
    {
        readyToShoot = true;

        if (!isRecoiling)
        {
            isRecoiling = false;
            if (recoilCoroutine != null)
            {
                StopCoroutine(recoilCoroutine);
            }
            MoveGunBackInstant();
        }
    }
    private void MoveGunBackInstant()
    {
        if (!Input.GetMouseButton(1))
        {
            gunModel.transform.localPosition = originolPosition;
        }
        else
        {
            gunModel.transform.localPosition = aimPosition;
        }
    }

    //reloading function
    private void Reload()
    {
        reloading = true;
        GameManager.instance.CharReloading();
        MoveGunBackInstant();
        Invoke("ReloadDone", reloadTime);
        Invoke("ResetShot", timeBetweenShots);
    }

    //after reloading is complete
    public void ReloadDone()
    {
        int bulletsToReload = magSize - bulletsLeft;
        if (!destroyOnEmpty)
        {
            if (GameManager.instance.playerAmmo > 0 && bulletsLeft < magSize)
            {
                int reservedAmmo = (int)Mathf.Min(GameManager.instance.playerAmmo, bulletsToReload);
                bulletsLeft += reservedAmmo;
                GameManager.instance.playerAmmo -= reservedAmmo;
            }
        }
        else
        { 
            ammunition -= 1;
            int reservedAmmo = (int)Mathf.Min(ammunition, bulletsToReload);
            bulletsLeft += reservedAmmo;
        }

        reloading = false;
        bulletsLeft = weapons[currentWeapon].bulletsLeft = bulletsLeft;
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
        GameManager.instance.playerAmmo += gunStat.ammunition;
        if (gunStat.destroyOnEmpty) 
        {
            ammunition += gunStat.ammunition;
        }
        GameManager.instance.ammoDisplay.SetActive(true);

        GameManager.instance.ammoGatheredTotal += gunStat.ammunition;
        GameManager.instance.ammoGatheredTotal += gunStat.magSize;
        GameManager.instance.UpdateAmmoCount();
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
        statusEffect = weapons[index].statusEffect;
        recoilAmount = weapons[index].recoilAmount;
        adsReduction = weapons[index].adsReducution;
        destroyOnEmpty = weapons[index].destroyOnEmpty;
        shakeIntensity = weapons[index].shakeIntensity;
        if (destroyOnEmpty)
        {
            explosion = weapons[index].explosion;
        }
        GameManager.instance.UpdateAmmoCount();
        gunModel.mesh = weapons[currentWeapon].model.GetComponent<MeshFilter>().sharedMesh;
        gunMat.material = weapons[currentWeapon].model.GetComponent<MeshRenderer>().sharedMaterial;
        originolPosition = weapons[currentWeapon].position;
        originolRotation = weapons[currentWeapon].rotation;
        aimPosition = weapons[currentWeapon].aimPosition;
        aimRotation = weapons[currentWeapon].aimRotation;
        if (weapons[currentWeapon].tag == "Pistol" || weapons[currentWeapon].tag == "AR")
        {
            GameManager.instance.activeRetical.SetActive(false);
            GameManager.instance.activeRetical = GameManager.instance.arPistolRetical;
            GameManager.instance.activeRetical.SetActive(true);
        }
        else if (weapons[currentWeapon].tag == "Shotgun")
        {
            GameManager.instance.activeRetical.SetActive(false);
            GameManager.instance.activeRetical = GameManager.instance.shotgunRetical;
            GameManager.instance.activeRetical.SetActive(true);
        }
        else if (weapons[currentWeapon].tag == "Limited")
        {
            GameManager.instance.activeRetical.SetActive(false);
            GameManager.instance.activeRetical = GameManager.instance.arPistolRetical;
            GameManager.instance.activeRetical.SetActive(true);
        }

        gunModel.transform.position = originolPosition;
        gunModel.transform.rotation = originolRotation;
        gunModel.transform.position = aimPosition;
        gunModel.transform.rotation = aimRotation;
        Invoke("ResetShot", timeBetweenShots);

    }
    public void AddBullets(int amount)
    {
        GameManager.instance.playerAmmo += amount;

        GameManager.instance.UpdateAmmoCount();

        GameManager.instance.ammoGatheredTotal += amount;
    }
    public void AddStatus(StatusEffectObj data)
    {
        weapons[currentWeapon].statusEffect = data;
        statusEffect = data;
    }

    public int GetAmmo() 
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
}
