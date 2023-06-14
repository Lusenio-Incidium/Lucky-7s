using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HubManager : MonoBehaviour
{
    [SerializeField] RandomSelection randomizer;
    [SerializeField] Animator bossDoor;
    [SerializeField] GameObject[] tokens;
    [Header("Audio")]
    [SerializeField] AudioSource HeavyDoor;
    [SerializeField] AudioClip DoorOpen;
    [Range(0, 1)][SerializeField] float HeavyDoorVol;
    List<IRandomizeHighlight> _lightObjects;
    List<IRandomizeAction> _actionObejcts;

    // THIS SCRIPT HAS NO SCALABILITY. MODIFY OR REWRITE BEFORE ADDING MORE LEVELS!
    void Start()
    {
        List<int> completed = GameManager.instance.GetCompletedLevels();
        bool playAnimation = true;
        _lightObjects = randomizer.GetLights();
        _actionObejcts = randomizer.GetActions();
        foreach (int level in completed)
        {
            if (GameManager.instance.GetLastestLevel() == level)
            {
                playAnimation = false;
            }
                        tokens[level - 1].SetActive(true);
        }
        if (playAnimation && GameManager.instance.GetLastestLevel() > 0)
        {
            tokens[GameManager.instance.GetLastestLevel() - 1].SetActive(true);
            tokens[GameManager.instance.GetLastestLevel() - 1].GetComponent<Animator>().enabled = true;
        }
        GameManager.instance.UpdateCompleteLevels();
        if(completed.Count >= 4)
        {
            randomizer.enabled = false;

            StartCoroutine(OpenBossDoor());
            return;
        }
        foreach (int level in completed)
        {
            for(int x = 0; x < _lightObjects.Count; x++)
            {
                if(_lightObjects[x].GetPosition() == level)
                {
                    _lightObjects.RemoveAt(x);
                    _actionObejcts.RemoveAt(x);
                }
            }
        }
        
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            StartCoroutine(OpenBossDoor());
        }
    }

    IEnumerator OpenBossDoor()
    {
        HeavyDoorVol = GameManager.instance.playerScript.GetJumpVol();  
        yield return new WaitForSeconds(2.5f);
        HeavyDoor.PlayOneShot(DoorOpen, HeavyDoorVol);
        GameManager.instance.playerScript.fadeOut(); 
        CameraShake shake = GameManager.instance.playerCam.GetComponent<CameraShake>();
        shake.SetStrengthAmount(5);
        shake.SetDuration(2);
        shake.start = true;

        bossDoor.SetTrigger("Open");
    }
}
