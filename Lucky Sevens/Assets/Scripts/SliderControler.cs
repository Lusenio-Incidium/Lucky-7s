using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SliderControler : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI plinkoText;
    [SerializeField] TextMeshProUGUI chipText;
    [SerializeField] TextMeshProUGUI coinText;
    [SerializeField] TextMeshProUGUI healthText;
    [SerializeField] TextMeshProUGUI speedText;
    [SerializeField] TextMeshProUGUI sheildText;
    [SerializeField] TextMeshProUGUI gunText;
    [SerializeField] Scrollbar plinko;
    [SerializeField] Scrollbar chip;
    [SerializeField] Scrollbar coin;
    [SerializeField] Scrollbar health;
    [SerializeField] Scrollbar speed;
    [SerializeField] Scrollbar sheild;
    [SerializeField] Scrollbar gun;

    public void onPlinko() 
    {
        plinkoText.text = (plinko.value * 10).ToString("0");
    }

    public void onChip()
    {
        chipText.text = (chip.value * 10).ToString("0");
    }

    public void onCoin()
    {
        coinText.text = (coin.value * 10).ToString("0");
    }

    public void onHealth()
    {
        healthText.text = (health.value * 3).ToString("0");
    }

    public void onSpeed()
    {
        speedText.text = (speed.value * 5).ToString("0");
    }

    public void onSheild()
    {
        sheildText.text = (sheild.value * 5).ToString("0");
    }

    public void onGun()
    {
        gunText.text = (gun.value).ToString("0");
    }


}
