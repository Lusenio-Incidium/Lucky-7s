using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockJump : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) 
        {
            GameManager.instance.playerScript.setJumpMode(false);
        }
    }
}
