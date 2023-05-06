using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomSelection : MonoBehaviour
{
    [SerializeField] GameObject[] _randomObjects;
    [SerializeField] GameObject[] _lightObjects;
    int selection;
    [Header("--- Roll Stats ---")]
    [Range(0.1f,10)][SerializeField] float idleShiftTime;
    [Range(0.1f,10)][SerializeField] float activeShiftTime;
    [Range(1,10)][SerializeField] float rollTime;

    bool rolling;
    int result;
    int highlight;
    float currShiftTime;
    [Header("--- Rigged Settings ---")]
    [SerializeField] bool rigged;
    [SerializeField] int riggedNum;

    void Start()
    {
        result = -1;
    }
    void Update()
    {
        currShiftTime -= Time.deltaTime;
        if(currShiftTime <= 0 && result == -1)
        {
            _lightObjects[highlight].GetComponent<Renderer>().material.color = Color.black;
            highlight++;
            if(highlight >= _lightObjects.Length)
            {
                highlight = 0;
            }
            _lightObjects[highlight].GetComponent<Renderer>().material.color = Color.yellow;
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
}
