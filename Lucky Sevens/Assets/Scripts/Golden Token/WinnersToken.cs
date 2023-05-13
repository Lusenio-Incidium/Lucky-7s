using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class WinnersToken : MonoBehaviour
{
    public static WinnersToken instance;
    [SerializeField] GameObject goldenTokenPrefab;
    [SerializeField] Transform spawnLocation;
    [SerializeField] Transform restLocation;
    [Range(0.1f, 10)][SerializeField] float travelTime;
    [Range(0.1f, 10)][SerializeField] float pauseTime;
    [Range(0.1f, 10)][SerializeField] float riseTime;
    [Range(0.1f, 10)][SerializeField] float youWinDelay;


    [Range(0.1f, 1000)][SerializeField] float spinSpeed;
    [Range(1, 100)][SerializeField] int spinMod;
    GameObject token;
    TokenCollect tokenInfo;
    int step;
    private void Start()
    {
        step = -1;
        instance = this;
        StartCoroutine(SpawnSteps());
    }

    public IEnumerator SpawnSteps()
    {
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
    // Update is called once per frame
    void Update()
    {
        if (step == 0)
        {
            token.transform.position = Vector3.Lerp(token.transform.position, new Vector3(token.transform.position.x, token.transform.position.y + 1, token.transform.position.z), Time.deltaTime * riseTime);
        }
        else if (step == 2)
        {
            token.transform.position = Vector3.Lerp(token.transform.position, restLocation.position, travelTime * Time.deltaTime);
        }
    }
}
