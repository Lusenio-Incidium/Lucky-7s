using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class HealerScript : MonoBehaviour
{
    float timePassed;
    EnemyAI enemyStats;
    float timeToHeal;
    float healAmount;
    NavMeshAgent agent;
    [SerializeField] GameObject HealingEffect;
    public void Start()
    {
        enemyStats = GetComponent<EnemyAI>();
        timeToHeal = enemyStats.GetHealCoolDown();
        healAmount = enemyStats.GetHealAmount();
        agent = enemyStats.GetAgent();
    }
    private void Update()
    {
        if (agent.isActiveAndEnabled)
        {
            if (timePassed == 0)
            {
                timePassed = Time.time;
            }
            else if (Time.time - timePassed > timeToHeal && timeToHeal != 0)
            {
                timePassed = 0;
                StartCoroutine(TimerHealEnemy());
            }
        }
    }
    IEnumerator TimerHealEnemy()
    {
        yield return new WaitForSeconds(timeToHeal);
        HealEnemy();
    }
    public void HealEnemy()
    {
        if (agent.isActiveAndEnabled)
        {
            //enemy.takeDamage(healAmount);
            Instantiate(HealingEffect, transform.position, Quaternion.identity);
            timePassed = 0;
        }
    }
}