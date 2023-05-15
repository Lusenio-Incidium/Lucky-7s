using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RandomSelection : MonoBehaviour
{
    List<IRandomizeAction> _actionObjects;
    List<IRandomizeHighlight> _lightObjects;
    int selection;
    [Header("--- Roll Stats ---")]
    [Range(0.1f,10)][SerializeField] float idleShiftTime; //How long it takes for the light to change when not rolling
    [Range(0.1f,10)][SerializeField] float activeShiftTime; //How long it takes for the light to change when rolling
    [Range(1,10)][SerializeField] float rollTimeMin; //Min roll time
    [Range(1, 10)][SerializeField] float rollTimeMax; //Max roll time

    [SerializeField] bool randomizeOrder; //Will it go 1 -> 2 -> 3 -> 4 or will it choose a random one every shift.
    bool rolling;
    int result;
    int highlight;
    float currShiftTime;
    bool actionRun;
    bool critErr;

    [Header("--- Rigged Settings ---")]
    [SerializeField] bool rigged;
    [SerializeField] int riggedNum;
    void Start()
    {
        _lightObjects = new List<IRandomizeHighlight>();
        _actionObjects = new List<IRandomizeAction>();
        actionRun = false;
        result = -1;
        if(rollTimeMin > rollTimeMax)
        {
            GameManager.instance.ErrorMenu("CRITICAL RANDOMIZER ERR - rollTimeMin is greater than rollTimeMax");
            critErr = true;
        }
        GameObject[] temp = GameObject.FindGameObjectsWithTag("RandomizerLight");
        foreach (GameObject obj in temp)
        {
            IRandomizeHighlight tempRandomHighlight = obj.GetComponent<IRandomizeHighlight>();
            if (tempRandomHighlight != null)
            {
                bool inserted = false;
                for(int x = 0; x < _lightObjects.Count; x++)
                {
                    if (_lightObjects[x].GetPosition() == tempRandomHighlight.GetPosition())
                    {
                        Debug.LogWarning("RANDOMIZER ERROR - Two Highlight Objects share the same position!");
                        inserted = true;
                        break;
                    }
                    if (_lightObjects[x].GetPosition() > tempRandomHighlight.GetPosition())
                    {
                        _lightObjects.Insert(x, tempRandomHighlight);
                        inserted = true;
                        break;
                    }
                }
                if (!inserted)
                {
                    _lightObjects.Add(tempRandomHighlight);
                }
            }
             else
             {
                 Debug.LogWarning("RANDOMIZER ERR - Object \"" + obj.name +"\" has tag \"RandomizerLight\" and does not have IRandomizeHighlight");
             }
        }
        temp = GameObject.FindGameObjectsWithTag("RandomizerActions");
        foreach (GameObject obj in temp)
        {
            IRandomizeAction tempRandomHighlight = obj.GetComponent<IRandomizeAction>();
            if (tempRandomHighlight != null)
            {
                bool inserted = false;
                for (int x = 0; x < _actionObjects.Count; x++)
                {
                    if (_actionObjects[x].GetPosition() == tempRandomHighlight.GetPosition())
                    {
                        Debug.LogWarning("RANDOMIZER ERROR - Two Action Objects share the same position!");
                        inserted = true;
                        break;
                    }
                    if (_actionObjects[x].GetPosition() > tempRandomHighlight.GetPosition())
                    {
                        _actionObjects.Insert(x, tempRandomHighlight);
                        inserted = true;
                        break;
                    }
                }
                if (!inserted)
                {
                    _actionObjects.Add(tempRandomHighlight);
                }
            }
            else
            {
                Debug.LogWarning("RANDOMIZER ERR - Object \"" + obj.name + "\" has tag \"RandomizerActions\" and does not have IRandomizeAction");
            }
        }

        if(_actionObjects.Count != _lightObjects.Count)
        {
            GameManager.instance.ErrorMenu("CRITICAL RANDOMIZER ERR - There are not equal numbers of Action Objects and Light Objects");
            critErr = true;
        }
    }
    void Update()
    {
        if (critErr)
        {
            return;
        }
        currShiftTime -= Time.deltaTime;
        if (result != -1)
        {
            if (!actionRun)
            {
                _actionObjects[result].OnSelect();
                actionRun = true;
            }
            _lightObjects[result].OnSelect();
        }
        else if (currShiftTime <= 0 && result == -1)
        {
            if (!randomizeOrder)
                Shift();
            else
                RandomSelect();
            if (rolling != true)
            {
                currShiftTime = idleShiftTime;
            }
            else
            {
                currShiftTime = activeShiftTime;
            }
        }
    }

    void Shift()
    {
        _lightObjects[highlight].OffHighlight();
        highlight++;
        if (highlight >= _lightObjects.Count)
        {
            highlight = 0;
        }
        _lightObjects[highlight].OnHighlight();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(!other.CompareTag("Player") || result != -1)
        {
           return;
        }
        StartCoroutine(Roll());
    }

    void RandomSelect()
    {
        int currNum = highlight;
        _lightObjects[highlight].OffHighlight();
        highlight = Random.Range(0, _lightObjects.Count);
        if(highlight == currNum)
        {
            highlight++;
            if (highlight >= _lightObjects.Count)
            {
                highlight = 0;
            }
        }
        _lightObjects[highlight].OnHighlight();
    }
    IEnumerator Roll()
    {
        rolling = true;
        yield return new WaitForSeconds(Random.Range(rollTimeMin, rollTimeMax));
        
        if (rigged && riggedNum < _lightObjects.Count && riggedNum >= 0)
        {
            if (randomizeOrder)
            {
                _lightObjects[highlight].OffHighlight();
                result = highlight = riggedNum;
            }
            else
            {
                Debug.LogWarning("RANDOMIZER ERROR - In order to rig randomizeOrder must be enabled.");
                result = highlight;
            }
        }
        else
        {
            result = highlight;
        }
    }
}
