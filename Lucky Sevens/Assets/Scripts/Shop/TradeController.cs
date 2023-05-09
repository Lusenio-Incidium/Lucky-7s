using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TradeController : MonoBehaviour, IDamage
{
    




    // Start is called before the first frame update
    void Start()
    {
     
    }


    public void takeDamage(int ammount)
    {
        GameManager.instance.Shop();
    }


}
