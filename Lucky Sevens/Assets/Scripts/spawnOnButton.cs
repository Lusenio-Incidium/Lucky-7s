using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spawnOnButton : MonoBehaviour, IButtonTrigger, IMinigame
{
    public void OnButtonPress() 
    {
        gameObject.SetActive(true);
    }

    public void OnButtonRelease() 
    {
        
    }

    public void onWin() 
    {
        gameObject.SetActive(true);
    }
}
