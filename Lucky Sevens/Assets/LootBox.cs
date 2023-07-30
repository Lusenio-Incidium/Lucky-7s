using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootBox : MonoBehaviour, IInteractable
{
    [SerializeField] GameObject[] lootTable;

    GameObject pickedItem;
    public void onInteract() 
    {
        int itemPicked = Random.Range(0,lootTable.Length);
        pickedItem = lootTable[itemPicked];
        Instantiate(pickedItem,transform.position, transform.rotation);
        Destroy(gameObject);
    }

}
