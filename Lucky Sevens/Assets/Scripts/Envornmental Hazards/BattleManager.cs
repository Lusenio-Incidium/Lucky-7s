using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class BattleManager : MonoBehaviour
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
    //[SerializeField] EnemyLineup[] wave;
    [SerializeField] GameObject[] enemyTypes;
    [SerializeField] Transform[] spawnLocations;
    [SerializeField] GameObject[] triggerItems;
    [SerializeField] int spawnDelay;
    [SerializeField] int batchSpawn;
    [SerializeField] int spawnAmount;
    int enemyCount;
    bool spawning = false;
    bool battleBegin = false;
    private void Start()
    {
        enemyCount = spawnAmount;
    }
    // Update is called once per frame
    void Update()
    {
        if (battleBegin)
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
    }
    private void OnTriggerEnter(Collider other)
    {
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
}
