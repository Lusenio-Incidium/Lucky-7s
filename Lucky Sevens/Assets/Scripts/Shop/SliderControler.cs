using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SliderControler : MonoBehaviour
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

    int chipTotal;
    int plinkoTotal;
    int healthTotal;
    int speedTotal;
    int sheildTotal;
    int gunTotal;

    int tokenCost;

    private void Update()
    {
        updateText();
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

}
