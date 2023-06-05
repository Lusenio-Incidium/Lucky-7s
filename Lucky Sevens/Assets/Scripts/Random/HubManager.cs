using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HubManager : MonoBehaviour
{
    [SerializeField] RandomSelection randomizer;
    [SerializeField] Animator bossDoor;
    [SerializeField] Animator[] tokenAnimators;
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
            if(GameManager.instance.GetLastestLevel() == level)
            {
                playAnimation = false;
                break;
            }
        }
        if (playAnimation || GameManager.instance.GetLastestLevel() <= 0)
        {
            //tokenAnimators[GameManager.instance.GetLastestLevel()].SetTrigger("Place");
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
                    Debug.LogWarning("Deleting: " + x + '\t' + completed.Count);
                    _lightObjects.RemoveAt(x);
                    _actionObejcts.RemoveAt(x);
                }
            }
        }
        
    }

    IEnumerator OpenBossDoor()
    {
        yield return new WaitForSeconds(1);
        CameraShake shake = GameManager.instance.playerCam.GetComponent<CameraShake>();
        shake.SetStrengthAmount(5);
        shake.SetDuration(2);
        shake.start = true;

        bossDoor.SetTrigger("Open");
    }
}
