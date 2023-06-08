using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Security.Principal;
using UnityEngine;
using static ObjectPoolManager;

public class ObjectPoolManager : MonoBehaviour
{
    //Allowing the user to set the number of Objects they wish to spawn in each scene
    //Feel free to add to this or tell me if I missed something
    public static ObjectPoolManager instance;
    [Header("- - - Amount Of Objects - - -")]
    public int amountOfEnemies;
    public int amountOfBullets;
    //Bools to help pick which onces you want to spawn (If your amount of enemies is 5 and you pick tommy, it will spawn 5 tommyguards)
    [Header("- - - Type Of Enemies - - -")]
    public bool tommy;
    public bool melee;
    public bool pistol;
    public bool healer;
    public bool spawner;

    //Orginization so the scene is cluttered
    GameObject _objectPoolEmptyHolder;

    //Fields to put the object you wish to spawn in. (Not needed if you dont want to spawn them in before the scene loads)
    //Feel free to add to this or tell me if I missed something
    [SerializeField] GameObject healerEnemy;
    [SerializeField] GameObject tommyEnemy;
    [SerializeField] GameObject pistolEnemy;
    [SerializeField] GameObject meleeEnemy;
    [SerializeField] GameObject spawnerEnemy;
    [SerializeField] GameObject bullet;

    //Once again orginization
    private GameObject _particleSystemEmpty;
    private GameObject _gameObjectsEmpty;
    private GameObject _enemiesEmpty;

    public enum PoolType
    {
        //Feel free to add to this or tell me if I missed something
        ParticleSystem,
        GameObject,
        Enemies,
        None
    }
    private void Awake()
    {
        // Making the instance and spawning in all of the objects you want to spawn
        if(ObjectPoolManager.instance == null)
        {
            instance = this;
        }
        SetUpEmpties();
        //Spawns in the enemies you checked true
        SpawnSetEnemies();
        //Spawns in the amount of bullets you wished to spawn
        SpawnBullets();
    }

    private void SpawnBullets()
    {
        GameObject parentObject = SetParentObjectType(PoolType.GameObject);
        for (int i = 0; i < amountOfBullets; ++i)
        {
            GameObject temp = null;
            temp = Instantiate(bullet, transform.position, transform.rotation);
            if (i == 0)
            {
                string orgName = temp.name.Substring(0, temp.name.Length - 7);
                PooledObjectInfo info = new PooledObjectInfo() { LookUpString = orgName };
                ObjectPools.Add(info);
            }
            temp.transform.SetParent(parentObject.transform);
            ReturnObjToInfo(temp);
        }
    }
    private void SpawnSetEnemies()
    {
        //If healer is checked off will spawn the healer enemy
        if (healer)
        { 
            GameObject parentObject = SetParentObjectType(PoolType.Enemies);
            for (int i = 0; i < amountOfEnemies; ++i)
            {
                GameObject temp = null;
                temp = Instantiate(healerEnemy, transform.position, transform.rotation);
                if (i == 0)
                {
                    string orgName = temp.name.Substring(0, temp.name.Length - 7);
                    PooledObjectInfo info = new PooledObjectInfo() { LookUpString = orgName };
                    ObjectPools.Add(info);
                }
                temp.transform.SetParent(parentObject.transform);
                ReturnObjToInfo(temp);
            }
        }
        if (tommy)
        {
            GameObject parentObject = SetParentObjectType(PoolType.Enemies);
            for (int i = 0; i < amountOfEnemies; ++i)
            {
                GameObject temp = null;
                temp = Instantiate(tommyEnemy, transform.position, transform.rotation);
                if (i == 0)
                {
                    string orgName = temp.name.Substring(0, temp.name.Length - 7);
                    PooledObjectInfo info = new PooledObjectInfo() { LookUpString = orgName };
                    ObjectPools.Add(info);
                }
                temp.transform.SetParent(parentObject.transform);
                ReturnObjToInfo(temp);
            }
        }
        if (melee)
        {
            GameObject parentObject = SetParentObjectType(PoolType.Enemies);
            for (int i = 0; i < amountOfEnemies; ++i)
            {
                GameObject temp = null;
                temp = Instantiate(meleeEnemy, transform.position, transform.rotation);
                if (i == 0)
                {
                    string orgName = temp.name.Substring(0, temp.name.Length - 7);
                    PooledObjectInfo info = new PooledObjectInfo() { LookUpString = orgName };
                    ObjectPools.Add(info);
                }
                temp.transform.SetParent(parentObject.transform);
                ReturnObjToInfo(temp);
            }
        }
        if (pistol)
        {
            GameObject parentObject = SetParentObjectType(PoolType.Enemies);
            for (int i = 0; i < amountOfEnemies; ++i)
            {
                GameObject temp = null;
                temp = Instantiate(pistolEnemy, transform.position, transform.rotation);
                if (i == 0)
                {
                    string orgName = temp.name.Substring(0, temp.name.Length - 7);
                    PooledObjectInfo info = new PooledObjectInfo() { LookUpString = orgName };
                    ObjectPools.Add(info);
                }
                temp.transform.SetParent(parentObject.transform);
                ReturnObjToInfo(temp);
            }
        }
        if (spawner)
        {
            GameObject parentObject = SetParentObjectType(PoolType.Enemies);
            for (int i = 0; i < amountOfEnemies; ++i)
            {
                GameObject temp = null;
                temp = Instantiate(spawnerEnemy, transform.position, transform.rotation);
                if (i == 0)
                {
                    string orgName = temp.name.Substring(0, temp.name.Length - 7);
                    PooledObjectInfo info = new PooledObjectInfo() { LookUpString = orgName };
                    ObjectPools.Add(info);
                }
                temp.transform.SetParent(parentObject.transform);
                ReturnObjToInfo(temp);
            }
        }
    }

    private void SetUpEmpties()
    {
        _objectPoolEmptyHolder = new GameObject("Pooled Objects");

        _particleSystemEmpty = new GameObject("Particle Effects");
        _particleSystemEmpty.transform.SetParent(_objectPoolEmptyHolder.transform);

        _gameObjectsEmpty = new GameObject("Game Objects");
        _gameObjectsEmpty.transform.SetParent(_objectPoolEmptyHolder.transform);
        
        _enemiesEmpty = new GameObject("Enemies");
        _enemiesEmpty.transform.SetParent(_objectPoolEmptyHolder.transform);


    }
    //List of inactive objects and the string that they are under
    public List<PooledObjectInfo> ObjectPools = new List<PooledObjectInfo>();

    public GameObject SpawnObject(GameObject objToSpawn, Vector3 spawnPos, Quaternion spawnRot,PoolType poolType = PoolType.None)
    {
        //Finding the obj in the list of strings to instantiate
        //Saying for every GameObjectInfo in the list look for one whos string is equal to objToSpawn.name
        PooledObjectInfo info = ObjectPools.Find(p => p.LookUpString == objToSpawn.name);

        //If it can't find it they make a new lookUpString and list
        if (info == null)
        {
            info = new PooledObjectInfo() { LookUpString = objToSpawn.name };
            ObjectPools.Add(info);
        }
        GameObject spawnableObj = null;
        //Check if any objects are inactive
        foreach (GameObject obj in info.InactiveObjects)
        {
            if (obj != null)
            {
                spawnableObj = obj;
                break;
            }
        }
        //If all obj are active it makes a new one and sets it to be active as well as organizes it to it respective empty
        if (spawnableObj == null)
        {
            GameObject parentObject = SetParentObjectType(poolType);
            spawnableObj = Instantiate(objToSpawn, spawnPos, spawnRot);
            if(parentObject != null)
            {
                spawnableObj.transform.SetParent(parentObject.transform);
            }
        }
        else
        {
            //if one is found sets the postion and rotation as well as removes it from the inactive objects list
            spawnableObj.transform.position = spawnPos;
            spawnableObj.transform.rotation = spawnRot;
            info.InactiveObjects.Remove(spawnableObj);
            spawnableObj.SetActive(true);
        }
        return spawnableObj;
    }

    public void ReturnObjToInfo(GameObject obj)
    { 
        //Finds the lookUpstring of the object and gets rid of the clone part in the name
        string goName = obj.name.Substring(0, obj.name.Length - 7); //Removes the clone part of the name when an object is created
        PooledObjectInfo info = ObjectPools.Find(p => p.LookUpString == goName);

        //If not found shows a warning saying that object was never in the pool to start
        if(info == null)
        {
            Debug.LogWarning("Object has not been pooled: " + obj.name);
        }
        else
        {
           //If it was found sets it to false and puts the object back in the inactive list at the string
           obj.SetActive(false);
           info.InactiveObjects.Add(obj);
        }
    }
    private GameObject SetParentObjectType(PoolType poolType)
    {
        //Orginizational Purpose
        //For when you spawn in an object and set the the PoolType (will return which empty it should go in)
        //If null will just put it in the scene and not put it into an empty
        switch (poolType)
        {
            case PoolType.ParticleSystem:
                return _particleSystemEmpty;
            case PoolType.GameObject:
                return _gameObjectsEmpty;
            case PoolType.Enemies:
                return _enemiesEmpty;
            case PoolType.None:
                return null;
            default: 
                return null;
        
        }

    }
}
public class PooledObjectInfo
{
    //Each GameObjects look up string.
    public string LookUpString;
    public List<GameObject> InactiveObjects = new List<GameObject>();
}