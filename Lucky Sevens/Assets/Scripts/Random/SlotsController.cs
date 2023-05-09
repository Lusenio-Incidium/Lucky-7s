
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

    float _wheel1Rotation;
    float _wheel2Rotation;
    float _wheel3Rotation;
    [Range(1,100)][SerializeField] float _spinSpeed;
    [Range(0.1f, 1)][SerializeField] float _spinStartDelay;
   
    [Header("--- Slot Stats ---")]
    [Range(1, 10)][SerializeField] float spinTime; //How long it spins before it stops
    [Range(1, 10)][SerializeField] float spinDelay; //How long it takes to spin up again
    [Range(0, 100)][SerializeField] int jackpotOdds; //How likely it is to get 3 in a row
    [Range(0, 100)][SerializeField] int jackpotMod; //How much the likelyhood of getting 3 in a row goes up after missing

    [Header("--- Spawn ---")]
    [SerializeField] GameObject[] SpawnConditions;

    float _currSpinDelay;
    bool _isSpinning;
    bool _canStop;
    bool _wheel1Spin;
    bool _wheel2Spin;
    bool _wheel3Spin;
    SlotResults _wheelOneResult;
    SlotResults _wheelTwoResult;
    SlotResults _wheelThreeResult;
    int _currJackpotOdds;

    // Start is called before the first frame update
    void Start()
    {
        _isSpinning = _wheel1Spin = _wheel2Spin = _wheel3Spin = false;
        _currSpinDelay = spinDelay;
        _currJackpotOdds = jackpotOdds;
        _wheel1Rotation = 90;
        _wheel2Rotation = 90;
        _wheel3Rotation = 90;
    }

    // Update is called once per frame
    void Update()
    {
        AnimateSpin();
        if (!_isSpinning && _currSpinDelay <= 0)
        {
            StartCoroutine(Spin());
        }
        else if(_isSpinning && _canStop && _currSpinDelay <= 0)
        {
            Debug.Log((int)_wheelOneResult);
            _slot1.transform.rotation = Quaternion.Euler(((360 / 20) * (int)_wheelOneResult) + (90 - (360 /20)), 0 ,0);
            _slot2.transform.rotation = Quaternion.Euler(((360 / 20) * (int)_wheelTwoResult) + (90 - (360 / 20)), 0, 0);
            _slot3.transform.rotation = Quaternion.Euler(((360 / 20) * (int)_wheelThreeResult) + (90 - (360 / 20)), 0, 0);
            _wheel1Spin = _wheel2Spin = _wheel3Spin = false;
            _isSpinning = false;
            _currSpinDelay = spinDelay;
        }
        else
        {
            _currSpinDelay -= Time.deltaTime;
        }
    }

    IEnumerator Spin()
    {

        _isSpinning = true;
        _canStop = false;
        _wheel1Spin = true;
        yield return new WaitForSeconds(_spinStartDelay);
        _wheel2Spin = true;
        yield return new WaitForSeconds(_spinStartDelay);
        _wheel3Spin = true;


        if(Random.Range(1,100) < _currJackpotOdds) //I promise it's still not rigged, just ignore the rigging code ;3
        {
            int jackpot = Random.Range(1, 20);
            _wheelOneResult = _wheelTwoResult = _wheelThreeResult = (SlotResults)jackpot;
            _currJackpotOdds = jackpotOdds;
        }
        else
        {
            _wheelOneResult = (SlotResults)Random.Range(1, 20);
            _wheelTwoResult = (SlotResults)Random.Range(1, 20);
            _wheelThreeResult = (SlotResults)Random.Range(1, 20);
            _currJackpotOdds += jackpotMod;
            
        }

        yield return new WaitForSeconds(spinTime);
        _canStop = true;
        
    }
    void AnimateSpin()
    {
        if (_wheel1Spin)
        {
            _slot1.transform.Rotate(_spinSpeed, 0, 0 * Time.deltaTime);
            _wheel1Rotation += (_spinSpeed * Time.deltaTime) * 100;
            if(_wheel1Rotation >= 360)
            {
                _wheel1Rotation -= 360;
            }
            Debug.Log("Rotation: " + _slot1.transform.rotation.eulerAngles.x + "Tracked Angle: " + _wheel1Rotation);
        }
        if (_wheel2Spin)
        {
            _slot2.transform.Rotate(_spinSpeed, 0, 0 * Time.deltaTime);
            _wheel2Rotation += (_spinSpeed * Time.deltaTime) * 100;
            if (_wheel2Rotation >= 360)
            {
                _wheel2Rotation -= 360;
            }
        }
        if (_wheel3Spin)
        {
            _slot3.transform.Rotate(_spinSpeed, 0, 0 * Time.deltaTime);
            _wheel3Rotation += (_spinSpeed * Time.deltaTime) * 100;
            if (_wheel3Rotation >= 360)
            {
                _wheel3Rotation -= 360;
            }
        }
    }
}
