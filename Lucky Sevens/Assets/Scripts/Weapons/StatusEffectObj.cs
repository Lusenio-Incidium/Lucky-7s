using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Status Effects/New Status Effect")]
public class StatusEffectObj : ScriptableObject
{
    public string spellName;
    public int duration;
    public int damage;
    public int slowEffect;
    public float damagespeed;
}
