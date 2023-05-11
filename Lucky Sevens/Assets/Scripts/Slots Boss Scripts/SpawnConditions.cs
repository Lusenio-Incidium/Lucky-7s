using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum SpawnStyles
{
    RandomScatter,
    FireAtPlayer,
    BatchScatter,
}

[System.Serializable]
public class SpawnConditions
{
    [SerializeField] GameObject spawnObject;
    [SerializeField] int spawnCount;
    [Range(1,10)][SerializeField] int batchSize;
    [SerializeField][Range(0, 50)] float spawnDelay;
    [SerializeField] SpawnStyles spawnMethod;
    [SerializeField] bool waitForSpawner;
    [SerializeField] float accuracy;
    public GameObject GetSpawnObj()
    {
        return spawnObject;
    }
    public int GetSpawnCount()
    {
        return spawnCount;
    }
    public float GetSpawnDelay()
    {
        return spawnDelay;
    }
    public SpawnStyles GetSpawnStyles()
    {
        return spawnMethod;
    }
    public bool GetWaitForSpawner()
    {
        return waitForSpawner;
    }
    public float GetAccuracy()
    {
        return accuracy;
    }
    public int GetBatchSize()
    {
        return batchSize;
    }
}
