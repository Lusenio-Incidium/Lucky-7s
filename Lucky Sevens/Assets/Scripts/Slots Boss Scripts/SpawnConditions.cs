using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum SpawnStyles
{
    SpawnFromSlotsBottom,
    SpawnParabolaScatter,
    SpawnParabolaPlayerTarget,
    SpawnAbovePlayer,
    SpawnRandomAcrossField
}

[System.Serializable]
public class SpawnConditions
{
    [SerializeField] GameObject spawnObject;
    [SerializeField] int spawnCount;
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
    public bool GetSpawner()
    {
        return waitForSpawner;
    }

    public float GetAccuracy()
    {
        return accuracy;
    }
}
