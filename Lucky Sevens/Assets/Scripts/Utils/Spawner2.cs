using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner2 : MonoBehaviour,IDamage
{   // Update is called once per frame
    [SerializeField] GameObject[] thingsToSpawn;
    [SerializeField][Range(0, 5)] float intervalTime;
    [SerializeField] Transform[] spawnPos;
    [SerializeField] int maxNumber;
    [SerializeField] float spawnerHP;
    [SerializeField] bool infSpawn;
    [SerializeField] bool immortalSpawn;

    int spawnCount;
    bool playerInRange;
    bool isSpawning;
    EnemyAI spawnerEnemy;
    private void Start()
    {
        spawnerEnemy = GetComponent<EnemyAI>();
        if(spawnerEnemy != null)
        {
            immortalSpawn = false;
        }
    }
    void Update()
    {
        if (spawnerEnemy == null)
        {
            if (playerInRange && !isSpawning && spawnCount < maxNumber)
            {
                StartCoroutine(spawn());
            }
        }
        else
        {
            if (playerInRange && !isSpawning && spawnCount < maxNumber && spawnerEnemy.GetAgent().isActiveAndEnabled)
            {
                StartCoroutine(spawn());
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
        }
    }

    IEnumerator spawn()
    {
        isSpawning = true;
        Instantiate(thingsToSpawn[Random.Range(0, thingsToSpawn.Length)], spawnPos[Random.Range(0, spawnPos.Length)].position, thingsToSpawn[Random.Range(0,thingsToSpawn.Length)].transform.rotation);
        if (!infSpawn)
        {
            spawnCount++;
        }
        yield return new WaitForSeconds(intervalTime);
        isSpawning = false;
    }

    public void takeDamage(float dmg)
    {
        if (!immortalSpawn)
        {
            spawnerHP -= dmg;
            if (spawnerHP <= 0)
            {
                Destroy(gameObject);
            }
        }
    }
}
