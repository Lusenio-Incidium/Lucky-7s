using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slots : MonoBehaviour, IBoss
{
    public void startBoss() 
    {
        Debug.Log("Boss Started!");
        GameManager.instance.BossBarContainer.SetActive(true);
        GameManager.instance.BossBar.fillAmount = Mathf.Lerp(BossManager.instance.returnHP(),BossManager.instance.returnMaxHP(), 0.5f);
    }
    public void attackPhase(int phase) 
    {
        if (phase == 1)
            attackPhase1();
        else if(phase == 2)
            attackPhase2();
        else if(phase == 3)
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
    public int onDamage(int amount, int currHP) 
    {


        return currHP -= amount;
    }
    public int phaseUpdate(int hpAmount) 
    {
        return 1;
    }
}
