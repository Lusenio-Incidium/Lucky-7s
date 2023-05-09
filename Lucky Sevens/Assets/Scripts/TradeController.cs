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

    // Update is called once per frame
    void Update()
    {
        updateText();
    }

    public void takeDamage(int ammount)
    {
        GameManager.instance.Shop();
    }

    public void setChip() 
    {
        chipTotal = (int)(chip.value * 10);

    }

    public void setPlinko() 
    {
        plinkoTotal = (int)(plinko.value * 10) * 5;
        updateText();
    }
    public void setCoin()
    {
        coinTotal = (int)(coin.value * 10) * 10;
        updateText();
    }
    public void setHealth()
    {
        healthTotal = (int)(health.value * 3) * 5;
        updateText();
    }
    public void setSpeed()
    {
        speedTotal = (int)(speed.value * 5) * 10;
        updateText();
    }
    public void setSheild()
    {
        sheildTotal = (int)(sheild.value * 5) * 100;
        updateText();
    }
    public void setGun()
    {
        gunTotal = (int)(gun.value) * 200;
        updateText();
    }

    void updateText() 
    {
        tokenCost = (plinkoTotal + coinTotal);
        coinCost = (chipTotal + healthTotal + speedTotal + sheildTotal + gunTotal);

        chipText.text = tokenCost.ToString();
        coinText.text = coinCost.ToString();
    }
}
