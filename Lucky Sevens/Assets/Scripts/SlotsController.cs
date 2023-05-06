using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public enum SlotResults {
    Enemy = 1,
    Dice,
    Health,
    Coins,
    Chips,
    Empty1,
    Empty2,
    Empty3,
    Empty4,
    Empty5,
    Empty6,
    Empty7,
    Empty8,
    Empty9,
    Empty10,
    Empty11,
    Empty12,
    Empty13,
    Empty14,
    Empty15,
}


public class SlotsController : MonoBehaviour
{
    struct SpawnInfo {
        [SerializeField] GameObject SpawnObject;

    }
    [Header("--- Wheel Input ---")]
    [SerializeField] GameObject _slot1;
    [SerializeField] GameObject _slot2;
    [SerializeField] GameObject _slot3;
   
    [Header("--- Slot Stats ---")]
    [Range(1, 10)][SerializeField] float spinTime; //How long it spins before it stops
    [Range(1, 10)][SerializeField] float spinDelay; //How long it takes to spin up again
    [Range(0, 100)][SerializeField] int jackpotOdds; //How likely it is to get 3 in a row
    [Range(0, 100)][SerializeField] int jackpodMod; //How much the likelyhood of getting 3 in a row goes up after missing

    [Header("--- Spawn ---")]
    [SerializeField] SpawnInfo[] SpawnConditions;

    float currSpinDelay;
    bool _isSpinning;
    bool canStop;
    SlotResults _wheelOneResult;
    SlotResults _wheelTwoResult;
    SlotResults _wheelThreeResult;
    int currJackpotOdds;
    float slot1TimerStache;
    float slot2TimerStache;
    float slot3TimerStache;
    float slot1Timer;
    float slot2Timer;
    float slot3Timer;

    // Start is called before the first frame update
    void Start()
    {
        slot1TimerStache = _slot1.GetComponent<Animator>().GetCurrentAnimatorClipInfo(0).Length;
        slot2TimerStache = _slot2.GetComponent<Animator>().GetCurrentAnimatorClipInfo(0).Length;
        slot3TimerStache = _slot3.GetComponent<Animator>().GetCurrentAnimatorClipInfo(0).Length;
        _slot1.GetComponent<Animator>().enabled = false;
        _slot2.GetComponent<Animator>().enabled = false;
        _slot3.GetComponent<Animator>().enabled = false;
        _isSpinning = false;

        currSpinDelay = spinDelay;
        currJackpotOdds = jackpotOdds;
    }

    // Update is called once per frame
    void Update()
    {
        if (_slot1.GetComponent<Animator>().enabled)
        {
            slot1Timer -= Time.deltaTime;
            if (slot1Timer < 0)
            {
                slot1Timer = slot1TimerStache;
            }
        }
        if (_slot2.GetComponent<Animator>().enabled)
        {
            slot2Timer -= Time.deltaTime;
            if (slot2Timer < 0)
            {
                slot2Timer = slot2TimerStache;
            }
        }
        if (_slot3.GetComponent<Animator>().enabled)
        {
            slot3Timer -= Time.deltaTime;
            if (slot3Timer < 0)
            {
                slot3Timer = slot3TimerStache;
            }
        }

        if (!_isSpinning && currSpinDelay <= 0)
        {
            StartCoroutine(Spin());
        }
        else if(_isSpinning && canStop && currSpinDelay <= 0)
        {
            if ((int)(slot1Timer * 1000 / 50) + 1 == (int)_wheelOneResult)
            {
                Debug.Log((int)_wheelOneResult);
                _slot1.GetComponent<Animator>().enabled = false;
            }
            if ((int)(slot2Timer * 1000 / 50) + 1 == (int)_wheelTwoResult && _slot1.GetComponent<Animator>().enabled == false)
            {
                Debug.Log((int)_wheelTwoResult);
                _slot2.GetComponent<Animator>().enabled = false;
            }
            if ((int)(slot3Timer * 1000 / 50) + 1== (int)_wheelThreeResult && _slot2.GetComponent<Animator>().enabled == false)
            {
                Debug.Log((int)_wheelThreeResult);
                _slot3.GetComponent<Animator>().enabled = false;
                _isSpinning = false;
                currSpinDelay = spinDelay;
            }
        }
        else
        {
            currSpinDelay -= Time.deltaTime;
        }
    }

    IEnumerator Spin()
    {
        
        _isSpinning = true;
        canStop = false;
        _slot1.GetComponent<Animator>().enabled = true;
        _slot2.GetComponent<Animator>().enabled = true;
        _slot3.GetComponent<Animator>().enabled = true;
        if(Random.Range(1,100) < currJackpotOdds)
        {
            int jackpot = Random.Range(1, 20);
            _wheelOneResult = _wheelTwoResult = _wheelThreeResult = (SlotResults)jackpot;
        }
        _wheelOneResult = (SlotResults)Random.Range(1, 20);
        _wheelTwoResult = (SlotResults)Random.Range(1, 20);
        _wheelThreeResult = (SlotResults)Random.Range(1, 20);
        yield return new WaitForSeconds(spinTime);
        canStop = true;
    }
}
