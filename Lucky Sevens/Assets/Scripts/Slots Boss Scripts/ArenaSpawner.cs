using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ArenaSpawner : MonoBehaviour
{
    [SerializeField] GameObject prefab;
    [SerializeField] int spawnAmount;
    [SerializeField][Range(0, 5)] float intervalTime;
    [SerializeField][Range(0, 100)] float rangeRadius;
    [SerializeField] SpawnStyles spawnMethod;
    [SerializeField] Transform machineSpawnLocation;
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
    public void SetSpawnConditions(GameObject obj, int spawnNum, float spawnBreak, SpawnStyles style)
    {
        prefab = obj;
        spawnAmount = spawnNum;
        intervalTime = spawnBreak;
    }
    IEnumerator spawn()
    {
        if (spawnMethod == SpawnStyles.SpawnRandomAcrossField)
            Instantiate(prefab, new Vector3(gameObject.transform.position.x + Random.Range(-1 * rangeRadius, rangeRadius), gameObject.transform.position.y, gameObject.transform.position.z + Random.Range(-1 * rangeRadius, rangeRadius)), prefab.transform.rotation);
        else if (spawnMethod == SpawnStyles.SpawnAbovePlayer)
            Instantiate(prefab, new Vector3(GameManager.instance.player.transform.position.x, GameManager.instance.player.transform.position.y + 5, GameManager.instance.player.transform.position.z), prefab.transform.rotation);
        spawnAmount--;
        if(spawnAmount <= 0)
        {
            SlotsController.instance.SpawningFinished();
        }
        spawning = true;
        yield return new WaitForSeconds(intervalTime);
        spawning = false;
    }
}

