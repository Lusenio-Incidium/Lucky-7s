using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spawner : MonoBehaviour
{
    [SerializeField] GameObject prefab;
    [SerializeField] [Range(0,5)] float intervalTime;
    [SerializeField] Transform[] spawnPos;
    [SerializeField] int prefabMaxNum;

    int prefabsSpawnCount;
    bool playerInRange;
    bool isSpawning;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (playerInRange && !isSpawning && prefabsSpawnCount < prefabMaxNum)
        {
            StartCoroutine(spawn());
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
        Instantiate(prefab, spawnPos[Random.Range(0, spawnPos.Length)].position, prefab.transform.rotation);

        prefabsSpawnCount++;
        yield return new WaitForSeconds(intervalTime);
        isSpawning = false;
    }
}
