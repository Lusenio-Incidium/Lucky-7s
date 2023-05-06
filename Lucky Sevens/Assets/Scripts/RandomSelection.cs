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

    [Header("--- Results Stats ---")]
    [Range(0.1f, 10)][SerializeField] float blinkTime;

    bool blinking;
    [Header("--- Rigged Settings ---")]
    [SerializeField] bool rigged;
    [SerializeField] int riggedNum;

    void Start()
    {
        result = -1;
        rolling = false;
        blinking = false;

    }
    void Update()
    {
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
        else if (!blinking && result != -1)
        {
            StartCoroutine(Blink());
        }
        if (Input.GetButtonDown("Submit") && result == -1)
        {
            Debug.Log("Hoi am Tem");
            StartCoroutine(Roll());
        }
    }

    void Shift()
    {
        _lightObjects[highlight].GetComponent<Renderer>().material.color = Color.black;
        highlight++;
        if (highlight >= _lightObjects.Length)
        {
            highlight = 0;
        }
        _lightObjects[highlight].GetComponent<Renderer>().material.color = Color.yellow;
    }

    IEnumerator Blink()
    {
        blinking = true;
        yield return new WaitForSeconds(blinkTime);
        blinking = false;
        if(_lightObjects[highlight].GetComponent<Renderer>().material.color == Color.green)
        {
            _lightObjects[highlight].GetComponent<Renderer>().material.color = Color.black;
        }
        else
        {
            _lightObjects[highlight].GetComponent<Renderer>().material.color = Color.green;
        }
    }
    IEnumerator Roll()
    {
        rolling = true;
        yield return new WaitForSeconds(rollTime);
        if(rigged && riggedNum !>= _randomObjects.Length)
        {
            result = riggedNum;
        }
        else
        {
            result = highlight;
            _lightObjects[highlight].GetComponent<Renderer>().material.color = Color.green;
        }
    }
}
