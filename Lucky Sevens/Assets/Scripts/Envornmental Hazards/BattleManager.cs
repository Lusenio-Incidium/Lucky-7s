
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public class BattleManager : MonoBehaviour, IBattle, ICannonKey, IButtonTrigger, IDialouge
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
    int currentIndex;

    [Header("Trigger Functions")]
    [SerializeField] Functions onButtonPress;
    [SerializeField] Functions onButtonRelease;
    [SerializeField] Functions onBattleBegin;
    [SerializeField] Functions onBattleEnd;
    [SerializeField] Functions onCannonDeath;
    [SerializeField] Functions onTriggerEnter;
    [SerializeField] Functions onDialougeContinue;
    private void Start()
    {
        currentIndex = 0;
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
            if (randomlyChooseSpawns)
            {
                int locationRAM = UnityEngine.Random.Range(0, spawnLocations.Length - 1);
                GameObject enemy = Instantiate(enemyTypes[Random.Range(0, enemyTypes.Length - 1)], spawnLocations[locationRAM].position, spawnLocations[locationRAM].rotation); //CHANGE ROTATION!!!
                enemy.GetComponent<IBattleEnemy>().SetBattleManager(this);
            }
            else
            {
                GameObject enemy = Instantiate(enemyTypes[Random.Range(0, enemyTypes.Length - 1)], spawnLocations[currentIndex].position, spawnLocations[currentIndex].rotation);
                enemy.GetComponent<IBattleEnemy>().SetBattleManager(this);
                currentIndex++;
                if (currentIndex >= spawnLocations.Count())
                {
                    currentIndex = 0;
                }
                   

            }
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
        }
        battleBegin = false;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
        {
            return;
        }
        Debug.Log("Trigger Entered.");
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
            Debug.Log(battleBegin);
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

    public void OnDialougeContinue()
    {
        onDialougeContinue = FunctionActions(onDialougeContinue);
    }
}
