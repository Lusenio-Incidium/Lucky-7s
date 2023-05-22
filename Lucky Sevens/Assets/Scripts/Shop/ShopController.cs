using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class ShopController : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI plinkoText;
    [SerializeField] TextMeshProUGUI chipText;
    [SerializeField] TextMeshProUGUI healthText;
    [SerializeField] TextMeshProUGUI speedText;
    [SerializeField] TextMeshProUGUI sheildText;
    [SerializeField] TextMeshProUGUI gunText;
    [SerializeField] Scrollbar plinko;
    [SerializeField] Scrollbar chip;
    [SerializeField] Scrollbar health;
    [SerializeField] Scrollbar speed;
    [SerializeField] Scrollbar sheild;
    [SerializeField] Scrollbar gun;
    [SerializeField] TextMeshProUGUI chipTotText;
    [SerializeField] TextMeshProUGUI coinTotText;

    bool hasShop;
    [SerializeField] GameObject crate;
    int chipTotal;
    int plinkoTotal;
    int healthTotal;
    int speedTotal;
    int sheildTotal;
    int gunTotal;

    int tokenCost;

    private void Awake()
    {
        updateCrate();

        if (crate == null)
        {
            hasShop = false;
        }
        else
        {
            hasShop = true;
            //auto shutdown the shop menu
            this.gameObject.SetActive(false);
        }

    }

    private void Update()
    {
        if (hasShop)
            updateText();
    }

    public void updateCrate() 
    {
        crate = GameObject.FindGameObjectWithTag("Crate");
        if(crate == null) 
        {
            hasShop = false;
        }
        else 
        {
            hasShop = true;
        }
    }

    public void onPlinko()
    {
        plinkoText.text = (plinko.value * 10).ToString("0");

        plinkoTotal = (int)(plinko.value * 10) * 5;
        updateText();
    }

    public void onChip()
    {
        chipText.text = (chip.value * 10).ToString("0");

        chipTotal = (int)(chip.value * 10);
        updateText();
    }


    public void onHealth()
    {
        healthText.text = (health.value * 3).ToString("0");
        healthTotal = (int)(health.value * 3) * 5;
        updateText();
    }

    public void onSpeed()
    {
        speedText.text = (speed.value * 5).ToString("0");
        speedTotal = (int)(speed.value * 5) * 10;
        updateText();
    }

    public void onSheild()
    {
        sheildText.text = (sheild.value * 5).ToString("0");
        sheildTotal = (int)(sheild.value * 5) * 100;
        updateText();
    }

    public void onGun()
    {
        gunText.text = (gun.value).ToString("0");
        gunTotal = (int)(gun.value) * 200;
        updateText();
    }

    void updateText()
    {
        tokenCost = (plinkoTotal + chipTotal + healthTotal + speedTotal + sheildTotal + gunTotal);
        chipTotText.text = tokenCost.ToString();
    }

    public void onBuy()
    {
        Crate cs = crate.GetComponent<Crate>();

        cs.pickup.healthAmount = (int)(health.value * 3);
        cs.pickup.speedAmount = (int)(speed.value * 5);
        cs.pickup.plinkoAmount = (int)(plinko.value * 10);
        cs.pickup.shieldAmount = (int)(sheild.value * 5);
        cs.pickup.tokenAmount = (int)(chip.value * 10);

        if(gun.value > 0) 
        {
            cs.pickup.addPistol = true;
        }

        crate.SetActive(true);

        GameManager.instance.unPauseState();

    }
}
