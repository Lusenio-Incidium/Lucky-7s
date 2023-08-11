using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class InventoryItem
{
    public enum itemType 
    {
        Empty,
        Weapon,
        Consumable,
        Colectable
    }

    public itemType type;
    public string name;
    public string toolTip;
    public Sprite icon;
    public int slotNum;
    public int mod;
    public GunStats gun;
}
