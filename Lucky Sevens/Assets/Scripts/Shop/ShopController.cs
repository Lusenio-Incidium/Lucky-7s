using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class ShopController : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI chipTotText;
    [SerializeField] Button arButton;
    [SerializeField] Button shotgunButton;
    [SerializeField] Button speedButton;

    bool hasShop;
    [SerializeField] GameObject crate;
    int healthTotal;
    int speedTotal;
    int gunTotal;

    int tokenCost;

    bool buyTommy;
    bool buyShotgun;
    bool buyFullHP;
    bool buySpeed;

    private void OnEnable()
    {
        if (!GameManager.instance.hasShotgun)
            shotgunButton.interactable = true;
        if (!GameManager.instance.hasAR)
            arButton.interactable = true;
    }

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
        }
        this.gameObject.SetActive(false);
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


    public void onHealth()
    {
        if (!buyFullHP)
        {
            healthTotal += 120;
            buyFullHP = true;
        }

        else
        {
            healthTotal -= 120;
            buyFullHP = false;
        }
        updateText();
    }

    public void onSpeed()
    {
        if (!GameManager.instance.hasSpeedUpgrade)
        {
            if (!buySpeed)
            {
                speedTotal += 150;
                buySpeed = true;
            }

            else
            {
                speedTotal -= 150;
                buySpeed = false;
            }
        }
     
        updateText();
    }


    public void onGunTommy()
    {
        if (!GameManager.instance.hasAR) 
        {
            if (!buyTommy)
            {
                gunTotal += 150;
                buyTommy = true;
            }

            else
            {
                gunTotal -= 150;
                buyTommy = false;
            }
        }
        
        updateText();
    }

    public void onGunShotgun()
    {
        if (!GameManager.instance.hasShotgun)
        {
            if (!buyShotgun)
            {
                gunTotal += 350;
                buyShotgun = true;
            }

            else
            {
                gunTotal -= 350;
                buyShotgun = false;
            }
        }

        updateText();
    }

    void updateText()
    {
        tokenCost = (healthTotal + speedTotal + gunTotal);
        chipTotText.text = tokenCost.ToString();
    }


    public void onBuy()
    {
        if(tokenCost <= GameManager.instance.playerAmmo) 
        {
            GameManager.instance.playerAmmo -= tokenCost;
            GameManager.instance.gunSystem.AddBullets(-tokenCost);
            Crate cs = crate.GetComponent<Crate>();

            if (buyFullHP) 
            {
                cs.pickup.fullheal = true;
            }

            if (buyShotgun) 
            {
                cs.pickup.shotgun = true;
                shotgunButton.interactable = false;
            }

            if (buyTommy) 
            {
                cs.pickup.ar = true;
                arButton.interactable = false;
            }

            if (buySpeed) 
            {
                cs.pickup.speed = true;
                speedButton.interactable = false;
            }

            crate.SetActive(true);
            GameManager.instance.unPauseState();
        }
        else
        { 
            GameManager.instance.unPauseState();
            GameManager.instance.ErrorMenu("Not enough Chips!");
        }
        buyFullHP = false;
        buyShotgun = false;
        buySpeed = false;
        buyTommy = false;

        healthTotal = 0;
        speedTotal = 0;
        gunTotal = 0;

    }
}
