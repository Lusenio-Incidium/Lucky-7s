using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TradeController : MonoBehaviour, IDamage
{
    [SerializeField] Scrollbar plinko;
    [SerializeField] Scrollbar chip;
    [SerializeField] Scrollbar coin;
    [SerializeField] Scrollbar health;
    [SerializeField] Scrollbar speed;
    [SerializeField] Scrollbar sheild;
    [SerializeField] Scrollbar gun;
    [SerializeField] TextMeshProUGUI chipText;
    [SerializeField] TextMeshProUGUI coinText;

    int chipTotal;
    int plinkoTotal;
    int coinTotal;
    int healthTotal;
    int speedTotal;
    int sheildTotal;
    int gunTotal;

    int coinCost;
    int tokenCost;

    // Start is called before the first frame update
    void Start()
    {
        coinCost = 0;
        tokenCost = 0;
    }


    public void takeDamage(int ammount)
    {
        GameManager.instance.Shop();
    }


}
