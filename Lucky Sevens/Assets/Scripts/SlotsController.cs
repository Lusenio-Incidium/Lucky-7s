
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
    [Header("--- Wheel Stats ---")]
    [SerializeField] GameObject _slot1;
    [SerializeField] GameObject _slot2;
    [SerializeField] GameObject _slot3;
    [Range(1,100)][SerializeField] float _spinSpeed;
    [Range(0.1f, 1)][SerializeField] float _spinStartDelay;
   
    [Header("--- Slot Stats ---")]
    [Range(1, 10)][SerializeField] float spinTime; //How long it spins before it stops
    [Range(1, 10)][SerializeField] float spinDelay; //How long it takes to spin up again
    [Range(0, 100)][SerializeField] int jackpotOdds; //How likely it is to get 3 in a row
    [Range(0, 100)][SerializeField] int jackpotMod; //How much the likelyhood of getting 3 in a row goes up after missing

    [Header("--- Spawn ---")]
    [SerializeField] SpawnInfo[] SpawnConditions;

    float currSpinDelay;
    bool _isSpinning;
    bool canStop;
    bool wheel1Spin;
    bool wheel2Spin;
    bool wheel3Spin;
    SlotResults _wheelOneResult;
    SlotResults _wheelTwoResult;
    SlotResults _wheelThreeResult;
    int currJackpotOdds;

    // Start is called before the first frame update
    void Start()
    {
        _isSpinning = wheel1Spin = wheel2Spin = wheel3Spin = false;
        currSpinDelay = spinDelay;
        currJackpotOdds = jackpotOdds;
    }

    // Update is called once per frame
    void Update()
    {
        AnimateSpin();
        if (!_isSpinning && currSpinDelay <= 0)
        {
            StartCoroutine(Spin());
        }
        else if(_isSpinning && canStop && currSpinDelay <= 0)
        {
            //Detection Code
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
        wheel1Spin = true;
        yield return new WaitForSeconds(_spinStartDelay);
        wheel2Spin = true;
        yield return new WaitForSeconds(_spinStartDelay);
        wheel3Spin = true;


        if(Random.Range(1,100) < currJackpotOdds) //I promise it's still not rigged, just ignore the rigging code ;3
        {
            int jackpot = Random.Range(1, 20);
            _wheelOneResult = _wheelTwoResult = _wheelThreeResult = (SlotResults)jackpot;
            currJackpotOdds = jackpotOdds;
        }
        else
        {
            _wheelOneResult = (SlotResults)Random.Range(1, 20);
            _wheelTwoResult = (SlotResults)Random.Range(1, 20);
            _wheelThreeResult = (SlotResults)Random.Range(1, 20);
            currJackpotOdds += jackpotMod;
            
        }

        yield return new WaitForSeconds(spinTime);
        canStop = true;
    }
    void AnimateSpin()
    {
        if (wheel1Spin)
        {
            _slot1.transform.Rotate(_spinSpeed, 0, 0 * Time.deltaTime);
        }
        if (wheel2Spin)
        {
            _slot2.transform.Rotate(_spinSpeed, 0, 0 * Time.deltaTime);
        }
        if (wheel3Spin)
        {
            _slot3.transform.Rotate(_spinSpeed, 0, 0 * Time.deltaTime);
        }
    }
}
