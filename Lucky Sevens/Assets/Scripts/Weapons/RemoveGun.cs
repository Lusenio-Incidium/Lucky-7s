using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemoveGun : MonoBehaviour
{
    [SerializeField] GunStats GunToRemove;
    [SerializeField] bool destroyEvenIfFail;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")){
            if (GameManager.instance.playerScript.RemoveGun(GunToRemove))
            {
                Destroy(gameObject);
            }
            else
            {
                Debug.Log("Gun not found");
            }
            if (destroyEvenIfFail)
            {
                Destroy(gameObject);
            }
            
        }
    }
}
