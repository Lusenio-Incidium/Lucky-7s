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

    private void OnTriggerStay(Collider other)
    {
        if (agent.isActiveAndEnabled)
        {
            EnemyAI fellowEnemy = other.GetComponent<EnemyAI>();
            if (timePassed == 0)
            {
                timePassed = Time.time;
            }
            else if (fellowEnemy && Time.time - timePassed > timeToHeal)
            {
                if (fellowEnemy.GetEnemyHP() < fellowEnemy.GetOrigEnemyHP())
                    StartCoroutine(TimerHealEnemy(fellowEnemy));
            }
        }
    }
    IEnumerator TimerHealEnemy(EnemyAI fellowEnemy)
    {
        timePassed = Time.time;
        yield return new WaitForSeconds(5);
        HealEnemy(fellowEnemy);
    }
    public void HealEnemy(EnemyAI enemy)
    {
        enemy.takeDamage(healAmount);
        Instantiate(HealingEffect, transform.position, Quaternion.identity);
        timePassed = Time.time;
    }
}