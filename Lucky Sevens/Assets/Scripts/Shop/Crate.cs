using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crate : MonoBehaviour
{
    // Start is called before the first frame update
    public ShopPickup pickup;

    private void Start()
    {
        this.gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) 
        {
            //Move data from pickup into the player/GameManager, whichever we decide to do.
            GameManager.instance.playerScript.shopRegister(pickup);
            GameManager.instance.gunSystem.updateShop(pickup);
        }
        //Instead of creating a new create every time, this one will just hide until its activated by the shop system.
        this.gameObject.SetActive(false);
        //pickup reset
        pickup.healthAmount = 0;
        pickup.speedAmount = 0;
        pickup.tokenAmount = 0;
        pickup.plinkoAmount = 0;
        pickup.shieldAmount = 0;
        pickup.addShotgun = false;
        pickup.addTommy = false;
        
    }
}
