using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class ShopController : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI plinkoText;
    [SerializeField] TextMeshProUGUI healthText;
    [SerializeField] TextMeshProUGUI speedText;
    [SerializeField] TextMeshProUGUI sheildText;
    [SerializeField] TextMeshProUGUI PistolgunText;
    [SerializeField] TextMeshProUGUI TommygunText;
    [SerializeField] Scrollbar plinko;
    [SerializeField] Scrollbar health;
    [SerializeField] Scrollbar speed;
    [SerializeField] Scrollbar sheild;
    [SerializeField] Toggle gunPistol;
    [SerializeField] Toggle gunTommy;
    [SerializeField] TextMeshProUGUI chipTotText;

    bool hasShop;
    [SerializeField] GameObject crate;
    int chipTotal;
    int plinkoTotal;
    int healthTotal;
    int speedTotal;
    int sheildTotal;
    int gunTotal;

    int tokenCost;
    bool buyTommy;

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

    public void onGunPistol()
    {
        PistolgunText.text = "1";

        updateText();
    }

    public void onGunTommy()
    {
        if (!buyTommy) 
        {
            gunTotal += 100;
            buyTommy = true;
        }

        else 
        {
            gunTotal -= 100;
            buyTommy = false;
        }
            
        updateText();
    }

    void updateText()
    {
        tokenCost = (plinkoTotal + healthTotal + speedTotal + sheildTotal + gunTotal);
        chipTotText.text = tokenCost.ToString();
    }

    public void resetMenu() 
    {
        plinko.value = 0;
        health.value = 0;
        speed.value = 0;
        sheild.value = 0;

        gunPistol.isOn = false;
        gunTommy.isOn = false;
    }

    public void onBuy()
    {
        if(tokenCost <= GameManager.instance.playerAmmo) 
        {
            GameManager.instance.playerAmmo -= tokenCost;
            GameManager.instance.gunSystem.AddBullets(-tokenCost);
            Crate cs = crate.GetComponent<Crate>();

            cs.pickup.healthAmount = (int)(health.value * 3);
            cs.pickup.speedAmount = (int)(speed.value * 5);
            cs.pickup.plinkoAmount = (int)(plinko.value * 10);
            cs.pickup.shieldAmount = (int)(sheild.value * 5);

            if (gunPistol.isOn)
            {
                cs.pickup.addPistol = true;
            }

            if (gunTommy.isOn)
            {
                cs.pickup.addTommy = true;
            }

            crate.SetActive(true);
            GameManager.instance.unPauseState();
        }
        else
        { 
            GameManager.instance.unPauseState();
            GameManager.instance.ErrorMenu("Not enough Chips!");
        }

        resetMenu();
    }
}
