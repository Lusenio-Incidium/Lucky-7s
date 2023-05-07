using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomSelection : MonoBehaviour
{
    //[SerializeField] GameObject[] _randomObjects;
    List<IRandomizeHighlight> _lightObjects;
    int selection;
    [Header("--- Roll Stats ---")]
    [Range(0.1f,10)][SerializeField] float idleShiftTime;
    [Range(0.1f,10)][SerializeField] float activeShiftTime;
    [Range(1,10)][SerializeField] float rollTime;

    bool rolling;
    int result;
    int highlight;
    float currShiftTime;
    bool actionRun;


    [Header("--- Rigged Settings ---")]
    [SerializeField] bool rigged;
    [SerializeField] int riggedNum;

    void Start()
    {
        _lightObjects = new List<IRandomizeHighlight>();
        actionRun = false;
        result = -1;
        GameObject[] temp = GameObject.FindGameObjectsWithTag("RandomizerLight");
        Debug.Log(temp.Length);
        foreach (GameObject obj in temp)
        {
            Debug.Log(obj.GetComponent<IRandomizeHighlight>());
            IRandomizeHighlight tempRandomHighlight = obj.GetComponent<IRandomizeHighlight>();
            if (tempRandomHighlight != null)
            {
                bool inserted = false;
                for(int x = 0; x < _lightObjects.Count; x++)
                {
                    if (_lightObjects[x].GetPosition() == tempRandomHighlight.GetPosition())
                    {
                        Debug.LogError("RANDOMIZER ERROR - Two objects share the same position!");
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
                 Debug.LogError("RANDOMIZER ERR - Object \"" + obj.name +"\" has tag \"RandomizerLight\" and does not have IRandomizeHighlight");
             }
        }
        //if(_randomObjects == null)
        //{
        //    _randomObjects = GameObject.FindGameObjectsWithTag("RandomizerActions");
        //}
    }
    void Update()
    {
        Debug.Log(_lightObjects.Count);
        currShiftTime -= Time.deltaTime;
        if(currShiftTime <= 0 && result == -1)
        {
            Shift();
            if (rolling != true)
            {
                currShiftTime = idleShiftTime;
            }
            else
            {
                currShiftTime = activeShiftTime;
            }
        }
        else if (result != -1)
        {
            _lightObjects[result].OnSelect();
        }
        if (Input.GetButtonDown("Submit") && result == -1)
        {
            StartCoroutine(Roll());
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
    IEnumerator Roll()
    {
        rolling = true;
        yield return new WaitForSeconds(rollTime);
        if(rigged && riggedNum !>= _lightObjects.Count)
        {
            result = riggedNum;
        }
        else
        {
            result = highlight;
        }
    }
}
