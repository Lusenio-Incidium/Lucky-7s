using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slots : MonoBehaviour, IBoss
{
    [SerializeField] string nameOfBoss;
    float lerpTimer;

    public string bossName { get; set; }
    public void startBoss()
    {
        Debug.Log("Boss Started!");
        BossManager.instance.currHP = BossManager.instance.bossHP;
        bossName = nameOfBoss;
        updateHP();
    }

    private void Update()
    {
        updateHP();
    }
    public void attackPhase(int phase)
    {
        if (phase == 1)
            attackPhase1();
        else if (phase == 2)
            attackPhase2();
        else if (phase == 3)
            attackPhase3();
        else
            Debug.LogError("Phase does not exist in this boss! : " + phase);
    }
    #region attackPhases
    void attackPhase1()
    {
        Debug.Log("Phase 1");
    }

    void attackPhase2()
    {
        Debug.Log("Phase 2");
    }

    void attackPhase3()
    {
        Debug.Log("Phase 3");
    }
    public void stunPhase()
    {
        Debug.Log("Stuned!");
    }
    #endregion
    public float onDamage(float amount, float currHP)
    {


        return currHP -= amount;
    }
    public int phaseUpdate(float hpAmount)
    {
        return 1;
    }

    void updateHP()
    {
        float hpDivide = (BossManager.instance.currHP / BossManager.instance.bossHP);
        lerpTimer += Time.deltaTime;
        float fillPercent = lerpTimer / 2000f;
        GameManager.instance.BossBar.fillAmount = Mathf.Lerp(GameManager.instance.BossBar.fillAmount, hpDivide, fillPercent);  
    }
       
}

