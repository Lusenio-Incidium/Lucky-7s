using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum SpawnStyles
{
    RandomScatter,
    FireAtPlayer,
    BatchScatter,
    FireOnAllFronts,
    YAxisOnPlayer,
    RoofOverPlayer
}

[System.Serializable]
public class SpawnConditions
{
    [SerializeField] public GameObject spawnObject;
    [SerializeField] public int spawnCount;
    [Range(1,10)][SerializeField]public  int batchSize;
    [SerializeField][Range(0, 50)] public float spawnDelay;
    [SerializeField] public SpawnStyles spawnMethod;
    [SerializeField] public bool waitForSpawner;
    [SerializeField] public float accuracy;
    [SerializeField] public Transform[] SpawnLocations;
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
    public Transform[] GetSpawnLocations()
    {
        return SpawnLocations;
    }

    public void SetSpawnLocations(Transform[] locations)
    {
        SpawnLocations = locations;
    }
}
