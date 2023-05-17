using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]

public class GunStats : ScriptableObject
{
    public int damage;
    public float timeBetweenShots;
    public float range;
    public float reloadTime;
    public float spread;
    public int bulletsPerTap;
    public int magSize;
    public int ammunition;
    public StatusEffectObj statusEffect;
    public Rigidbody bulletPreFab;
}
