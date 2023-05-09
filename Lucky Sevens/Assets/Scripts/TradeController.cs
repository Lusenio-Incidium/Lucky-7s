using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TradeController : MonoBehaviour, IDamage
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void takeDamage(int ammount)
    {
        Debug.Log("I HAVE BEEN HIT");
        GameManager.instance.ComfirmMenu("Are you sure you wish to trade your life for some cookies?");
    }
}
