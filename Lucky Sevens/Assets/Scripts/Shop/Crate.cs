using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crate : MonoBehaviour
{
    // Start is called before the first frame update
    public ShopPickup pickup;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) 
        {
            //Move data from pickup into the player/GameManager, whichever we decide to do.
        }
        //Instead of creating a new create every time, this one will just hide until its activated by the shop system.
        this.gameObject.SetActive(false);
    }
}
