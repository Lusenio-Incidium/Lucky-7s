
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
    public static SlotsController instance;
    [Header("--- Wheel Stats ---")]
    [SerializeField] GameObject _slot1;
    [SerializeField] GameObject _slot2;
    [SerializeField] GameObject _slot3;

    float _currSpinDelay;
    float _currStopDelay;
    bool _isSpinning;
    bool _canStop;
    bool _wheel1Spin;
    bool _wheel2Spin;
    bool _wheel3Spin;

    [Header("--- Slot Stats ---")]
    [Range(1,100)][SerializeField] float _spinSpeed;
    [Range(0.1f, 1)][SerializeField] float _spinStartDelay;
   
    [Range(1, 10)][SerializeField] float spinTimeMax; //How long it spins before it stops
    [Range(1, 10)][SerializeField] float spinTimeMin; 
    [Range(1, 10)][SerializeField] float spinDelayMax; //How long it takes to spin up again
    [Range(1, 10)][SerializeField] float spinDelayMin; //How long it takes to spin up again
    [Range(0.1f, 10)][SerializeField] float wheelStopDelayMax;
    [Range(0.1f, 10)][SerializeField] float wheelStopDelayMin;




    [Range(0, 100)][SerializeField] int jackpotOdds; //How likely it is to get 3 in a row
    [Range(0, 100)][SerializeField] int jackpotMod; //How much the likelyhood of getting 3 in a row goes up after missing

    

    int Health;

    [Header("--- Spawn ---")]
    [SerializeField] GameObject Face1;
    [SerializeField] GameObject Face2;
    [SerializeField] GameObject Face3;
    [SerializeField] GameObject Face4;
    [SerializeField] GameObject Face5;
    [SerializeField] GameObject Face6;
    [SerializeField] GameObject Face7;
    [SerializeField] GameObject Face8;
    [SerializeField] GameObject Face9;
    [SerializeField] GameObject Face10;
    [SerializeField] GameObject Face11;
    [SerializeField] GameObject Face12;
    [SerializeField] GameObject Face13;
    [SerializeField] GameObject Face14;
    [SerializeField] GameObject Face15;
    [SerializeField] GameObject Face16;
    [SerializeField] GameObject Face17;
    [SerializeField] GameObject Face18;
    [SerializeField] GameObject Face19;
    [SerializeField] GameObject Face20;


    SlotResults _wheelOneResult;
    SlotResults _wheelTwoResult;
    SlotResults _wheelThreeResult;
    int _currJackpotOdds;
    bool isStunned;

    // Start is called before the first frame update
    void Start()
    {
        _isSpinning = _wheel1Spin = _wheel2Spin = _wheel3Spin = false;
        _currSpinDelay = Random.Range(spinDelayMin, spinDelayMax);
        _currJackpotOdds = jackpotOdds;
        isStunned = false;
        Health = 3;
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isStunned)
        {
            SlotsLogic();
        }
        
    }

    public void DamageWheel()
    {
        Health--;
        if(Health == 2)
        {
            _slot1.GetComponent<Rigidbody>().useGravity = true;
           // Destroy(_slot1);
            _wheel1Spin = false;
            isStunned = false;
        }
        if(Health == 1)
        {
            _slot3.GetComponent<Rigidbody>().useGravity = true;

            //Destroy(_slot3); //Sumthin has to be out of order for this to work.
            isStunned = false;
        }
        if (Health == 0)
        {
            //Instert Win Condition
            _slot2.GetComponent<Rigidbody>().useGravity = true;

           // Destroy(_slot2);
        }
    }

    public void StunWheel()
    {
        isStunned = true;
        _currStopDelay = 0;
        _isSpinning = false;
        _canStop = false;
        _currSpinDelay = Random.Range(spinDelayMin, spinDelayMax);
    }

    void SlotsLogic()
    {
        AnimateSpin();
        if (!_isSpinning && _currSpinDelay <= 0)
        {
            StartCoroutine(Spin());
        }
        else if (_isSpinning && _canStop && _currSpinDelay <= 0)
        {
            _currStopDelay -= Time.deltaTime;
            Debug.Log((int)_wheelOneResult);
            if (_wheel1Spin && Health >= 3)
            {
                _wheel1Spin = false;
                _slot1.transform.rotation = Quaternion.Euler(((360 / 20) * (int)_wheelOneResult) + (90 - (360 / 20)), 0, 0);
                _currStopDelay = Random.Range(wheelStopDelayMin, wheelStopDelayMax);
            }

            if (!_wheel1Spin && _currStopDelay <= 0 && _wheel2Spin)
            {
                _wheel2Spin = false;
                _slot2.transform.rotation = Quaternion.Euler(((360 / 20) * (int)_wheelTwoResult) + (90 - (360 / 20)), 0, 0);
                _currStopDelay = Random.Range(wheelStopDelayMin, wheelStopDelayMax);
            }
            if (!_wheel2Spin && _currStopDelay <= 0 && _wheel3Spin) { 
                _slot3.transform.rotation = Quaternion.Euler(((360 / 20) * (int)_wheelThreeResult) + (90 - (360 / 20)), 0, 0);
                _wheel3Spin = false;
                _isSpinning = false;
                _currSpinDelay = Random.Range(spinDelayMin, spinDelayMax);
            }
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
        if (Health >= 3)
        {
            _wheel1Spin = true;
            yield return new WaitForSeconds(_spinStartDelay);
        }
        _wheel2Spin = true;
        if (Health >= 2)
        {
            yield return new WaitForSeconds(_spinStartDelay);
            _wheel3Spin = true;
        }

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

        yield return new WaitForSeconds(Random.Range(spinTimeMin, spinTimeMax));
        _canStop = true;
        
    }
    void AnimateSpin()
    {
        if (_wheel1Spin )
        {
            _slot1.transform.Rotate(_spinSpeed, 0, 0 * Time.deltaTime);
        }
        if (_wheel2Spin)
        {
            _slot2.transform.Rotate(_spinSpeed, 0, 0 * Time.deltaTime);
        }
        if (_wheel3Spin)
        {
            _slot3.transform.Rotate(_spinSpeed, 0, 0 * Time.deltaTime);
        }
    }
}
