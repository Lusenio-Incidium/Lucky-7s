using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Shop/Crate")]
public class ShopPickup : ScriptableObject
{
    public int tokenAmount;
    public int plinkoAmount;
    public int healthAmount;
    public int speedAmount;
    public int shieldAmount;
    public bool addPistol;
    public bool addTommy;
    public bool addShotgun;
}
