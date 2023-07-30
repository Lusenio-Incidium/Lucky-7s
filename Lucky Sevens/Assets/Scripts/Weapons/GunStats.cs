using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Guns/stats")]

public class GunStats : ScriptableObject
{
    [Header("-----Inventory Info-----")]
    public string gunName;
    public string gunToolTip;
    public Sprite gunSprite;
    [Header("-----Gun-----")]
    public GameObject model;
    public string tag;
    [Header("-----Stats-----")]
    public int damage;
    public float timeBetweenShots;
    public float range;
    public float reloadTime;
    public float spread;
    public int bulletsPerTap;
    public int magSize;
    public int bulletsLeft;
    public int currentAmmoCount;
    public int ammunition;
    [Header("-----Recoil/PushBack-----")]
    public float pushBackForce;
    public float recoilAmount;
    public float adsReducution;
    [Header("-----Audio-----")]
    public AudioClip gunShotAud;
    [Range(0, 1)] public float gunShotAudVol;
    [Header("-----Effects-----")]
    public StatusEffectObj statusEffect;
    public GameObject hitEffect;
    public GameObject explosion;
    [Header("-----TriggerType-----")]
    public bool TriggerHold;
    public bool destroyOnEmpty;

    [Header("-----HipFire-----")]
    public Vector3 position;
    public Quaternion rotation;
    public float shakeIntensity;

    [Header("-----ADS-----")]
    public Vector3 aimPosition;
    public Quaternion aimRotation;
}
