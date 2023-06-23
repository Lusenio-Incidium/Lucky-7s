using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DanceWin : MonoBehaviour
{
    [SerializeField] GameObject[] affectedObjects;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) 
        {
            foreach (GameObject triggered in affectedObjects)
            {
                IMinigame minigame = triggered.GetComponent<IMinigame>();
                if (minigame != null)
                {
                    minigame.onWin();
                }
            }
            GameManager.instance.playerScript.setJumpMode(true);
        }
        
    }
}
