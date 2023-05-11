using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class ArenaSpawner : MonoBehaviour
{
    [Header("--- Spawner Info ---")]
    [Range(1,3)][SerializeField] int SpawnerNum;


    [SerializeField] GameObject prefab;
    [SerializeField] int spawnAmount;
    [SerializeField] int batchSize;
    [SerializeField][Range(0, 5)] float intervalTime;
    [SerializeField][Range(0, 100)] float rangeRadius;
    [SerializeField] SpawnStyles spawnMethod;
    [SerializeField] Transform[] machineSpawnLocations;
    // Start is called before the first frame update

    bool spawning = false;
    // Update is called once per frame
    void Update()
    {
        if (spawnAmount > 0 && !spawning)
        {
            StartCoroutine(spawn());
        }
    }
    public void SetSpawnConditions(GameObject obj, int spawnNum, float spawnBreak, SpawnStyles style, float radius, int batch)
    {
        prefab = obj;
        spawnAmount = spawnNum;
        intervalTime = spawnBreak;
        spawnMethod = style;
        rangeRadius = radius;
        batchSize = batch;
    }
    IEnumerator spawn()
    {
        if (spawnMethod == SpawnStyles.RandomScatter)
            Instantiate(prefab, new Vector3(gameObject.transform.position.x + Random.Range(-1 * rangeRadius, rangeRadius), gameObject.transform.position.y, gameObject.transform.position.z + Random.Range(-1 * rangeRadius, rangeRadius)), prefab.transform.rotation);
        else if (spawnMethod == SpawnStyles.FireAtPlayer) {
            int rand = Random.Range(0, machineSpawnLocations.Length);
            Instantiate(prefab, machineSpawnLocations[rand].position, machineSpawnLocations[rand].rotation);
        }
        else if (spawnMethod == SpawnStyles.BatchScatter)
        {
            for(int x = 0; x < batchSize; x++)
            {
                Instantiate(prefab, new Vector3(gameObject.transform.position.x + Random.Range(-1 * rangeRadius, rangeRadius), gameObject.transform.position.y, gameObject.transform.position.z + Random.Range(-1 * rangeRadius, rangeRadius)), prefab.transform.rotation);
            }
        }
        spawnAmount--;
        if(spawnAmount <= 0)
        {
            SlotsController.instance.SpawningFinished(SpawnerNum);
        }
        spawning = true;
        yield return new WaitForSeconds(intervalTime);
        spawning = false;
    }
}

