using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TradeController : MonoBehaviour, IInteractable
{

    public void onInteract()
    {
        
        GameManager.instance.Shop();
    }
}
