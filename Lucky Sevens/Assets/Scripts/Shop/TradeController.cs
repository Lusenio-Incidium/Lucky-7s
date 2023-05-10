using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TradeController : MonoBehaviour, IInteractable
{
    public void onInteract()
    {
        GameManager.instance.Shop();
    }
}
