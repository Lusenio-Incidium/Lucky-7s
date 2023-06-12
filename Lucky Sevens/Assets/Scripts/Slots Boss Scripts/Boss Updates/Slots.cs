using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slots : MonoBehaviour, IBoss
{
    [SerializeField] string nameOfBoss;
    float lerpTimer;

    GameObject[] cannons;
    GameObject[] hatchs;
    [SerializeField] GameObject[] wheels;
    [SerializeField] ArenaSpawner[] spawners;
    [SerializeField] SpawnConditions[] faces = new SpawnConditions[20];
    public string bossName { get; set; }

    bool[] cannonsActive;
    bool hasStarted;
    bool reinforce;
    bool cannonRemains;
    bool updating;
    bool stunned;
    bool attacking;
    int[] slotResult = new int[3];
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
        attackPhase(BossManager.instance.currPhase);
    }

    private void Update()
    {
        if (hasStarted) 
        {
            updateHP();
            if (!updating)
                StartCoroutine(cannonUpdate());
            if(!stunned && !attacking)
                for (int i = 0; i < wheels.Length; i++)
                    wheels[i].transform.Rotate(5, 0, 0);
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
        slotResults();
        Debug.Log("Attack Phase");
        StartCoroutine(attackLogic(phase));
    }

    public void unStun() 
    {
        hatchs[BossManager.instance.currPhase - 1].GetComponent<Animator>().SetBool("HatchOpen", false);
        stunned = false;
        cannonRemains = true;
        for (int i = 0; i < cannons.Length; i++)
            cannons[i].GetComponentInChildren<CannonController>().Respawn(reinforce);

        attacking = false;
        attackPhase(BossManager.instance.currPhase);
    }

    void slotResults() 
    {
        for(int i = 0; i < slotResult.Length; i++) 
            slotResult[i] = Random.Range(1, 20);
    }

    #region attackPhases
    void attackPhase1()
    {
        Debug.Log("Phase 1");
        for (int i = 0; i < slotResult.Length; i++)
        {
            if (slotResult[i] <= faces.Length - 1)
            {
                int result = slotResult[i] - 1;
                Debug.Log(result);
                spawners[i].SetSpawnConditions(faces[result]);
            }
            else if (slotResult[i] % 10 <= faces.Length - 1 && slotResult[i] % 10 > 0)
            {
                int result = (slotResult[i] % 10) - 1;
                Debug.Log(result);
                spawners[i].SetSpawnConditions(faces[result]);
            }
        }
    }

    void attackPhase2()
    {
        Debug.Log("Phase 2");
        for (int i = 1; i < slotResult.Length; i++)
        {
            if (slotResult[i] <= faces.Length - 1)
            {
                int result = slotResult[i] - 1;
                Debug.Log(result);
                spawners[i].SetSpawnConditions(faces[result]);
            }
            else if (slotResult[i] % 10 <= faces.Length - 1 && slotResult[i] % 10 > 0)
            {
                int result = (slotResult[i] % 10) - 1;
                Debug.Log(result);
                spawners[i].SetSpawnConditions(faces[result]);
            }
        }
    }

    void attackPhase3()
    {
        Debug.Log("Phase 3");
        for (int i = 2; i < slotResult.Length; i++)
        {
            if (slotResult[i] <= faces.Length - 1)
            {
                int result = slotResult[i] - 1;
                Debug.Log(result);
                spawners[i].SetSpawnConditions(faces[result]);
            }
            else if (slotResult[i] % 10 <= faces.Length - 1 && slotResult[i] % 10 > 0)
            {
                int result = (slotResult[i] % 10) - 1;
                Debug.Log(result);
                spawners[i].SetSpawnConditions(faces[result]);
            }
        }
    }
    public void stunPhase()
    {
        stunned = true;
        
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
            hasStarted = false;
            GameManager.instance.BossBarContainer.SetActive(false);
        }
        updating = false;
        StopAllCoroutines();
        wheels[BossManager.instance.currPhase - 2].SetActive(false);
        attackPhase(BossManager.instance.currPhase);
        if (BossManager.instance.currPhase == 3)
            for (int i = 0; i < cannons.Length; i++)
                cannons[i].GetComponentInChildren<CannonController>().StartMoving();
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

    IEnumerator attackDelay() 
    {
        yield return new WaitForSeconds(10f);
        attacking = false;
        attackPhase(BossManager.instance.currPhase);
    }
    IEnumerator attackLogic(int phase)
    {
        yield return new WaitForSeconds(5f);
        attacking = true;
        for (int i = 0; i < wheels.Length; i++)
            wheels[i].transform.rotation = Quaternion.Euler(((360 / 20) * slotResult[i]) + (90 - (360 / 20)), 0, 0);
        if (phase == 1)
            attackPhase1();
        else if (phase == 2)
            attackPhase2();
        else if (phase == 3)
            attackPhase3();
        StartCoroutine(attackDelay());
    }

}
