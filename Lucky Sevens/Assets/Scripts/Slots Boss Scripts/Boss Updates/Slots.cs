using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slots : MonoBehaviour, IBoss
{
    [SerializeField] string nameOfBoss;
    float lerpTimer;

    GameObject[] cannons;
    GameObject[] hatchs;
    public string bossName { get; set; }

    bool[] cannonsActive;
    bool hasStarted;
    bool reinforce;
    bool cannonRemains;
    bool updating;
    public void startBoss()
    {
        Debug.Log("Boss Started!");
        BossManager.instance.currHP = BossManager.instance.bossHP;
        bossName = nameOfBoss;
        updateHP();
        cannons = GameObject.FindGameObjectsWithTag("Cannon");
        cannonsActive = new bool[cannons.Length];
        hatchs = GameObject.FindGameObjectsWithTag("BossHatch");

        for (int i = 0; i < cannons.Length; i++) 
        {
            cannons[i].GetComponentInChildren<CannonController>().Respawn(false);
            cannonsActive[i] = true;
        }
        hasStarted = true;
        cannonRemains = true;
    }

    private void Update()
    {
        if (hasStarted) 
        {
            updateHP();
            if (!updating)
                StartCoroutine(cannonUpdate());
        }
        
    }

    IEnumerator cannonUpdate()
    {
        updating = true;
        yield return new WaitForSeconds(1f);
        for (int i = 0; i < cannons.Length; i++)
        {
            cannonsActive[i] = cannons[i].GetComponentInChildren<CannonController>().isCannonActive();
        }
        if (!cannonsActive[0])
            cannonRemains = false;
        for (int i = 0; i < cannonsActive.Length; i++)
        {
            if (cannonsActive[i])
                cannonRemains = true;
        }
        if (!cannonRemains)
            stunPhase();
        updating = false;
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

    public void unStun() 
    {
        hatchs[BossManager.instance.currPhase - 1].GetComponent<Animator>().SetBool("HatchOpen", false);
        for(int i = 0; i < cannons.Length; i++) 
        {
            cannons[i].GetComponentInChildren<CannonController>().Respawn(reinforce);
        }
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
        StartCoroutine(hatchDelay());
    }

    void stun() 
    {
        hatchs[BossManager.instance.currPhase - 1].GetComponent<Animator>().SetBool("HatchOpen", true);
    }
    #endregion
    public float onDamage(float amount, float currHP)
    {

        lerpTimer = 0;
        return currHP -= amount;
    }
    public void phaseUpdate()
    {
        BossManager.instance.currPhase += 1;
        reinforce = true;
        if(BossManager.instance.currPhase > BossManager.instance.numOfPhases) 
        {
            WinnersToken.instance.Spawn();
            return;
        }
        cannonRemains = true;
        if(BossManager.instance.currPhase == 3) 
            for(int i = 0; i < cannons.Length; i++) 
                cannons[i].GetComponentInChildren<CannonController>().StartMoving();
        updating = false;
        StopAllCoroutines();
    }

    void updateHP()
    {
        float hpDivide = (BossManager.instance.currHP / BossManager.instance.bossHP);
        lerpTimer += Time.deltaTime;
        float fillPercent = lerpTimer / 2000f;
        GameManager.instance.BossBar.fillAmount = Mathf.Lerp(GameManager.instance.BossBar.fillAmount, hpDivide, fillPercent);  
    }

    public IEnumerator hatchDelay() 
    {
        stun();
        yield return new WaitForSeconds(10f);
        unStun();
    }
       
}

