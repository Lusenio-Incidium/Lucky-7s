using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class BattleManager : MonoBehaviour, IBattle, ICannonKey, IButtonTrigger
{
    [System.Serializable]
    private struct EnemyLineup
    {
        [SerializeField] GameObject enemy;
        [SerializeField] int count;

        /*public GameObject GetEnemy()
        {
            return enemy;
        }*/

        public int GetCount()
        {
            return count;
        }
    }

    private enum Functions {
        None,
        StartBattle,
        ForceEndBattle
    }
    private enum SpawnStyles {
    }


    //[SerializeField] EnemyLineup[] wave;
    [SerializeField] GameObject[] enemyTypes;
    [SerializeField] Transform[] spawnLocations;
    [SerializeField] GameObject[] triggerItems;
    [SerializeField] int spawnDelay;
    [SerializeField] int batchSpawn;
    [SerializeField] int spawnAmount;
    [SerializeField] bool randomlyChooseSpawns;
    [SerializeField] bool randomlyChooseEnemy;
    int enemyCount;
    int enemySelect;
    int spawnSelect;
    bool spawning = false;
    bool battleBegin = false;

    [Header("Trigger Functions")]
    [SerializeField] Functions onButtonPress;
    [SerializeField] Functions onButtonRelease;
    [SerializeField] Functions onBattleBegin;
    [SerializeField] Functions onBattleEnd;
    [SerializeField] Functions onCannonDeath;
    [SerializeField] Functions onTriggerEnter;
    private void Start()
    {
        enemyCount = spawnAmount;
    }
    // Update is called once per frame
    void Update()
    {
        if (battleBegin && !spawning)
        {
            StartCoroutine(Spawn());
        }
    }

    IEnumerator Spawn()
    {
        spawning = true;
        for (int x = 0; x <= batchSpawn && spawnAmount > 0; x++)
        {
            
            GameObject enemy = Instantiate(enemyTypes[Random.Range(0, enemyTypes.Length - 1)], spawnLocations[Random.Range(0, spawnLocations.Length - 1)].position, transform.rotation); //CHANGE ROTATION!!!
            Debug.Log("Spawn");
            enemy.GetComponent<IBattleEnemy>().SetBattleManager(this);
            spawnAmount--;
        }
        yield return new WaitForSeconds(spawnDelay);
        spawning = false;
    }

    public void DeclareDeath(int amount)
    {
        enemyCount -= amount;
        if (enemyCount <= 0)
        {
            EndBattle();
        }
    }

    private void EndBattle()
    {
        if (battleBegin == false)
        {
            return;
        }
        foreach (GameObject obj in triggerItems)
        {
            IBattle affectObj = obj.GetComponent<IBattle>();
            if (affectObj != null)
            {
                affectObj.OnBattleEnd();
            }
            else
            {
                Debug.LogWarning("Err");
            }
        }
        battleBegin = false;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
        {
            return;
        }
        onTriggerEnter = FunctionActions(onTriggerEnter);
    }

    private void BeginBattle()
    {
        if (battleBegin)
        {
            return;
        }
        foreach (GameObject obj in triggerItems)
        {
            IBattle affectObj = obj.GetComponent<IBattle>();
            if (affectObj != null)
            {
                affectObj.OnBattleBegin();
            }
            else
            {
                Debug.LogWarning("Err");
            }
        }
        battleBegin = true;
    }
    private Functions FunctionActions(Functions function)
    {
        switch (function)
        {
            case Functions.None:
                break;
            case Functions.StartBattle:
                if (enemyCount > 0)
                {
                    BeginBattle();
                }
                break;
            case Functions.ForceEndBattle:
                DeclareDeath(enemyCount);
                break;
        }
        return Functions.None;
    }
    public void OnBattleBegin()
    {
        onBattleBegin = FunctionActions(onBattleBegin);
    }

    public void OnBattleEnd()
    {
        onBattleEnd = FunctionActions(onBattleEnd);
    }

    public void OnButtonPress()
    {
        onButtonPress = FunctionActions(onButtonPress);
    }

    public void OnButtonRelease()
    {
        onButtonRelease = FunctionActions(onButtonRelease);
    }

    public void OnCannonDeath()
    {
        onCannonDeath = FunctionActions(onCannonDeath);
    }
}
