using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
public enum WinConditions
{
    HandledExternally = 0,
    SurviveXTime,
    KillXEnemies,
    KillAllEnemies
}
public class WinnersToken : MonoBehaviour
{
    [Header("----- Object Linking -----")]
    public static WinnersToken instance;
    [SerializeField] GameObject goldenTokenPrefab;
    [SerializeField] Transform spawnLocation;
    [SerializeField] Transform restLocation;
    [Header("----- Coin Timing -----")]
    [Range(0.1f, 10)][SerializeField] float travelTime;
    [Range(0.1f, 10)][SerializeField] float pauseTime;
    [Range(0.1f, 10)][SerializeField] float riseTime;


    [Header("----- Coin Spin -----")]
    [Range(0.1f, 1000)][SerializeField] float spinSpeed;
    [Range(1, 100)][SerializeField] int spinMod;


    [Header("----- Win Conditions -----")]
    [SerializeField] WinConditions winStyle;
    [SerializeField] int timeOrKills;
    [Range(0.1f, 10)][SerializeField] float youWinDelay;
    int enemiesKilled;
    GameObject token;
    TokenCollect tokenInfo;
    int step;
    bool isSpawned;
    private void Start()
    {
        step = -1;
        instance = this;
        isSpawned = false;
        if(winStyle == WinConditions.KillXEnemies)
        {
            timeOrKills = (int)timeOrKills;
        }
    }

    private void Update()
    {
        if (step == 0)
        {
            token.transform.position = Vector3.Lerp(token.transform.position, new Vector3(token.transform.position.x, token.transform.position.y + 1, token.transform.position.z), Time.deltaTime * riseTime);
        }
        else if (step == 2)
        {
            token.transform.position = Vector3.Lerp(token.transform.position, restLocation.position, travelTime * Time.deltaTime);
        }

        if (winStyle == WinConditions.SurviveXTime)
        {
            if ((int)GameManager.instance.timeElapsed == timeOrKills)
            {
                Spawn();
            }
        }
    }

    public void UpdateEnemyCount(int killed)
    {
        enemiesKilled += killed;
        if (winStyle == WinConditions.KillXEnemies)
        {
            if (enemiesKilled >= timeOrKills)
            {
                Spawn();
            }
        }
        else if (winStyle == WinConditions.KillAllEnemies)
        {
            if (GameManager.instance.enemiesRemaining <= 0)
            {
                Spawn();
            }
        }
    }
    public void Spawn()
    {
        if(!isSpawned)
            StartCoroutine(SpawnSteps());
    }
    public IEnumerator SpawnSteps()
    {
        isSpawned = true;
        token = Instantiate(goldenTokenPrefab, spawnLocation.transform.position, goldenTokenPrefab.transform.rotation);
        tokenInfo = token.GetComponent<TokenCollect>();
        if(tokenInfo == null)
        {
            Debug.LogError("WinnersToken Err - Instantiated Object doesn't have TokenCollect. Check your Prefab.");
            yield return null;
        }
        tokenInfo.SetSpinSpeed(spinSpeed);
        tokenInfo.SetMod(spinMod);
        tokenInfo.SetYouWinDelay(youWinDelay);
        step = 0;
        tokenInfo.SpeedUp();
        yield return new WaitForSeconds(riseTime);
        tokenInfo.SpeedDown();
        step++;
        yield return new WaitForSeconds(pauseTime);
        tokenInfo.SpeedUp();
        step++;
        yield return new WaitForSeconds(travelTime);
        tokenInfo.SpeedDown();
        step = 2;
        token.GetComponent<SphereCollider>().enabled = true;
    }
}
