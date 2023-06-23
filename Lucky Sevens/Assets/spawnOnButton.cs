using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spawnOnButton : MonoBehaviour, IButtonTrigger, IMinigame
{
    public void OnButtonPress() 
    {
        this.gameObject.SetActive(true);
    }

    public void OnButtonRelease() 
    {
        this.gameObject.SetActive(false);
    }

    public void onWin() 
    {
        this.gameObject.SetActive(true);
    }
}
