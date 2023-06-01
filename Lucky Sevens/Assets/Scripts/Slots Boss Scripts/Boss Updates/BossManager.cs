using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossManager : MonoBehaviour
{
    public static BossManager instance;
    public float bossHP;
    public int numOfPhases;

    IBoss boss;
    public float currHP;
    int currPhase;

    private void Awake()
    {
        if(instance == null) 
        {
            instance = this;
            boss = GameObject.FindGameObjectWithTag("Boss").GetComponent<IBoss>();
        }
    }
    //Event to start the boss.
    public void onBossStart() 
    {
        boss.startBoss();
        GameManager.instance.BossBarContainer.SetActive(true);
        GameManager.instance.bossName.text = boss.bossName;
    }

    //This event should be called everytime the boss is damaged.
    public void onBossDamage(float amount) 
    {
        currHP = boss.onDamage(amount,currHP);
        if(currPhase != boss.phaseUpdate(currHP)) 
        {
            currPhase = boss.phaseUpdate(currHP);
            onStunPhase();
        }
    }

    //This event is only called when the boss changes phases. OR when something else calls this function.
    public void onStunPhase() 
    {
        boss.stunPhase();
    }
    //This function is whats called for dealing with attacks.
    public void onAttackPhase() 
    {
        boss.attackPhase(currPhase);
    }
}
