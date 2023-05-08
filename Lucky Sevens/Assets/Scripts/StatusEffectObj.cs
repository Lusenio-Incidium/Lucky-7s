using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "NewStatusEffect")]
public class SEffectObj : ScriptableObject
{
    public string name;
    public int duration;
    public int damage;
    public int slowEffect;
    public int damagespeed;
}
