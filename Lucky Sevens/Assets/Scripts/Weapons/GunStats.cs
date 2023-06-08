using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Guns/stats")]

public class GunStats : ScriptableObject
{
    public GameObject model;
    public string tag;

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
    public float pushBackForce;
    public float recoilAmount;
    public float adsReducution;
    public AudioClip gunShotAud;
    [Range(0, 1)] public float gunShotAudVol;
    public StatusEffectObj statusEffect;
    public GameObject hitEffect;
    public GameObject explosion;
    public bool TriggerHold;
    public bool destroyOnEmpty;

    public Vector3 position;
    public Transform gunTransform;
}
