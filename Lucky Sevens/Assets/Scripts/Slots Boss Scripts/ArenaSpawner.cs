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
    [SerializeField] Transform[] spawnLocations;
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
    public void SetSpawnConditions(SpawnConditions conditions)
    {
        prefab = conditions.GetSpawnObj();
        spawnAmount = conditions.GetSpawnCount();
        intervalTime = conditions.GetSpawnDelay();
        spawnMethod = conditions.GetSpawnStyles();
        rangeRadius = conditions.GetAccuracy();
        batchSize = conditions.GetBatchSize();
        spawnLocations = conditions.GetSpawnLocations();
    }
    IEnumerator spawn()
    {
        if (spawnMethod == SpawnStyles.RandomScatter)
            RandomScatter();
        else if (spawnMethod == SpawnStyles.FireAtPlayer)
            FireAtPlayer();
        else if (spawnMethod == SpawnStyles.BatchScatter)
            BatchScatter();
        else if (spawnMethod == SpawnStyles.FireOnAllFronts)
            FireOnAllFronts();
        else if (spawnMethod == SpawnStyles.YAxisOnPlayer)
            YOnPlayer();
        else if (spawnMethod == SpawnStyles.RoofOverPlayer)
        spawnAmount--;
        if(spawnAmount <= 0)
        {
            SlotsController.instance.SpawningFinished(SpawnerNum);
        }
        spawning = true;
        yield return new WaitForSeconds(intervalTime);
        spawning = false;
    }


    // SPAWN METHODS HERE! Add to Enum in SpawnConditions and spawn() as well
    void RandomScatter()
    {
        Instantiate(prefab, 
            new Vector3(gameObject.transform.position.x + Random.Range(-1 * rangeRadius, rangeRadius), gameObject.transform.position.y, gameObject.transform.position.z + Random.Range(-1 * rangeRadius, rangeRadius)), 
            prefab.transform.rotation);
    }

    void FireAtPlayer()
    {
        int rand = Random.Range(0, spawnLocations.Length);
        Instantiate(prefab, spawnLocations[rand].position, spawnLocations[rand].rotation);
    }

    void BatchScatter()
    {
        for (int x = 0; x < batchSize; x++)
        {
            Instantiate(prefab, 
            new Vector3(gameObject.transform.position.x + Random.Range(-1 * rangeRadius, rangeRadius), gameObject.transform.position.y, gameObject.transform.position.z + Random.Range(-1 * rangeRadius, rangeRadius)), 
            prefab.transform.rotation);
        }
    }

    void FireOnAllFronts()
    {
        foreach (Transform location in spawnLocations)
        {
            Instantiate(prefab, location.position, location.rotation);
        }
    }

    void YOnPlayer()
    {
        Instantiate(prefab, new Vector3(GameManager.instance.player.transform.position.x, rangeRadius, GameManager.instance.player.transform.position.z), prefab.transform.rotation);
    }

    void RoofOverPlayer()
    {
        Instantiate(prefab, new Vector3(GameManager.instance.player.transform.position.x, transform.position.y, GameManager.instance.player.transform.position.z), prefab.transform.rotation);

    }
}

