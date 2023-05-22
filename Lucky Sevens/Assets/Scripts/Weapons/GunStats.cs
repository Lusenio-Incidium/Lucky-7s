using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Guns/stats")]

public class GunStats : ScriptableObject
{
    public MeshFilter gunFilter;
    public MeshRenderer gunModel;
    public GameObject model;

    public int damage;
    public float timeBetweenShots;
    public float range;
    public float reloadTime;
    public float spread;
    public int bulletsPerTap;
    public int magSize;
    public int bulletsLeft;
    public int ammunition;
    public StatusEffectObj statusEffect;
    public Rigidbody bulletPreFab;
    public bool TriggerHold;
    public int currentAmmoCount;
}
