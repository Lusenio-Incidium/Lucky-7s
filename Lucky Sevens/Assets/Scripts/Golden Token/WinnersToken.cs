using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum WinConditions
{
    HandledExternally = 0,
    SurviveXTime,
    KillXEnemies,
    KillAllEnemies,
    TouchTriggerBox
}
public class WinnersToken : MonoBehaviour
{
    [Header("----- Object Linking -----")]
    public static WinnersToken instance;
    [SerializeField] GameObject goldenTokenPrefab;
    [SerializeField] Transform spawnLocation;
    [SerializeField] Transform restLocation;
    [Header("----- Coin Pathing -----")]
    [Range(0.1f, 10)][SerializeField] float travelTime;
    [Range(1, 100)][SerializeField] float tokenSpeed;
    [Range(0.1f, 10)][SerializeField] float pauseTime;
    [Range(0.1f, 10)][SerializeField] float riseTime;
    [Range(0.1f, 10)][SerializeField] float riseHeight;
    [Range(0.1f, 10)][SerializeField] float travelArcHeight;

    float step1Height;
    float distanceMid;
    [Header("----- Coin Spin -----")]
    [Range(0.1f, 1000)][SerializeField] float spinSpeed;
    [Range(1, 100)][SerializeField] int spinMod;


    [Header("----- Win Conditions -----")]
    [SerializeField] WinConditions winStyle;
    [Range(0,4)][SerializeField] int level;
    [SerializeField] int timeOrKills;
    [Range(0.1f, 10)][SerializeField] float youWinDelay;
    int enemiesKilled;
    GameObject token;
    TokenCollect tokenInfo;
    int step;
    bool isSpawned;
    private void Awake()
    {
        step = -1;
        instance = this;
        isSpawned = false;
        step1Height = spawnLocation.transform.position.y + riseHeight;
        distanceMid = (Mathf.Sqrt(Mathf.Pow(restLocation.position.x - spawnLocation.position.x, 2) + Mathf.Pow(restLocation.position.z - spawnLocation.position.z, 2))) / 2; //Distance Formula divided by 2
    }

    private void Update()
    {
        
        if (step == 0)
        {
            token.transform.position = Vector3.Lerp(token.transform.position, new Vector3(token.transform.position.x, riseHeight + spawnLocation.position.y, token.transform.position.z), Time.deltaTime * riseTime);
        }
        else if (step == 2)
        {
            //Parabola script was created by Rod Moye
            float distCur = Vector3.Distance(token.transform.position, restLocation.position);

            Vector3 endPosition = new Vector3(restLocation.position.x, restLocation.position.y + (distCur / 2f), restLocation.position.z);
            token.transform.position = Vector3.MoveTowards(token.transform.position, endPosition, Time.deltaTime * tokenSpeed);
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
            yield return null;
        }
        tokenInfo.SetSpinSpeed(spinSpeed);
        tokenInfo.SetMod(spinMod);
        tokenInfo.SetYouWinDelay(youWinDelay);
        tokenInfo.SetLevel(level);
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

    public WinConditions condition() 
    {
        return winStyle;
    }


    private void OnTriggerEnter(Collider other)
    {
        if(winStyle == WinConditions.TouchTriggerBox && other.CompareTag("Player"))
        {
            Spawn();
        }
    }
}
