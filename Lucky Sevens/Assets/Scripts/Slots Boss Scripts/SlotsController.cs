
using System;
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
    Slowdown,
    Bombs,
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
    [Header("--- Wheel Objects ---")]
    [SerializeField] GameObject _leftSlot;
    [SerializeField] GameObject _middleSlot;
    [SerializeField] GameObject _rightSlot;
    [SerializeField] Animator _LeftHatch;
    [SerializeField] Animator _RightHatch;
    [SerializeField] Animator _BottomHatch;
    [SerializeField] Animator _TopHatch;
    [SerializeField] ArenaSpawner spawner1;
    [SerializeField] ArenaSpawner spawner2;
    [SerializeField] ArenaSpawner spawner3;


    [Header("--- Slot Stats ---")]
    [Range(1, 50)][SerializeField] float _hatchOpenTime;
    [Range(1,100)][SerializeField] float _spinSpeed;
    [Range(0.1f, 1)][SerializeField] float _spinStartDelay;
   
    [Range(1, 10)][SerializeField] float spinTimeMax; //How long it spins before it stops
    [Range(1, 10)][SerializeField] float spinTimeMin; 
    [Range(1, 10)][SerializeField] float spinDelayMax; //How long it takes to spin up again
    [Range(1, 10)][SerializeField] float spinDelayMin; //How long it takes to spin up again
    [Range(0.1f, 10)][SerializeField] float wheelStopDelayMax;
    [Range(0.1f, 10)][SerializeField] float wheelStopDelayMin;


    float _currSpinDelay;
    float _currStopDelay;
    bool _isSpinning;
    bool _canStop;
    bool _wheel1Spin;
    bool _wheel2Spin;
    bool _wheel3Spin;
    float _currHatchOpenTime;
    bool _hatchOpen;
    [Range(0, 100)][SerializeField] int jackpotOdds; //How likely it is to get 3 in a row
    [Range(0, 100)][SerializeField] int jackpotMod; //How much the likelyhood of getting 3 in a row goes up after missing

    

    int Health;

    [Header("--- Face Stats ---")]
    [SerializeField] SpawnConditions[] normalFaceStats;
    [SerializeField] SpawnConditions[] jackpotFaceStats;


    SlotResults _wheelOneResult;
    SlotResults _wheelTwoResult;
    SlotResults _wheelThreeResult;
    int _currJackpotOdds;
    bool isStunned;
    int weakPointCount;
    bool waitingForSpawner1;
    bool waitingForSpawner2;
    bool waitingForSpawner3;
    int NumMod;
    // Start is called before the first frame update
    void Start()
    {
        _isSpinning = _wheel1Spin = _wheel2Spin = _wheel3Spin = false;
        _currSpinDelay = UnityEngine.Random.Range(spinDelayMin, spinDelayMax);
        _currJackpotOdds = jackpotOdds;
        isStunned = true;
        Health = 3;
        instance = this;
        weakPointCount = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isStunned)
        {
            SlotsLogic();
        }
        
    }
    public void Begin()
    {
        isStunned = false;
        
    }

    public void UpdateWeakPoints(int count)
    {
        weakPointCount += count;
        if(weakPointCount <= 0)
        {
            StunWheel();
        }
    }
    public void SpawningFinished(int spawnerNum)
    {
        if(spawnerNum ==1)
        waitingForSpawner1 = false;
        else if(spawnerNum == 2)
            waitingForSpawner2 = false;
        else
            waitingForSpawner3 = false;
    }

    public void DamageWheel()
    {
        Health--;
        if(Health == 2)
        {
            
           // Destroy(_slot1);
            _wheel1Spin = false;
            isStunned = false;
        }
        if(Health == 1)
        {


            //Destroy(_slot3); //Sumthin has to be out of order for this to work.
            _wheel3Spin = false;
            isStunned = false;
        }
        if (Health == 0)
        {
            WinnersToken.instance.Spawn();
            

           // Destroy(_slot2);
        }
    }

    void StunWheel()
    {
        isStunned = true;
        _currStopDelay = 0;
        _isSpinning = false;
        _canStop = false;
        _currSpinDelay = UnityEngine.Random.Range(spinDelayMin, spinDelayMax);
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
                _leftSlot.transform.rotation = Quaternion.Euler(((360 / 20) * (int)_wheelOneResult) + (90 - (360 / 20)), 0, 0);
                _currStopDelay = UnityEngine.Random.Range(wheelStopDelayMin, wheelStopDelayMax);
            }

            if (!_wheel1Spin && _currStopDelay <= 0 && _wheel2Spin)
            {
                _wheel2Spin = false;
                _rightSlot.transform.rotation = Quaternion.Euler(((360 / 20) * (int)_wheelTwoResult) + (90 - (360 / 20)), 0, 0);
                _currStopDelay = UnityEngine.Random.Range(wheelStopDelayMin, wheelStopDelayMax);
            }
            if (!_wheel2Spin && _currStopDelay <= 0 && _wheel3Spin) { 
                _rightSlot.transform.rotation = Quaternion.Euler(((360 / 20) * (int)_wheelThreeResult) + (90 - (360 / 20)), 0, 0);
                _wheel3Spin = false;
                _isSpinning = false;
                _currSpinDelay = UnityEngine.Random.Range(spinDelayMin, spinDelayMax);
                SpinAction();
            }
        }
        else if (!waitingForSpawner1 && !waitingForSpawner2 && !waitingForSpawner3)
        {
            _currSpinDelay -= Time.deltaTime;
        }
    }

    void SpinAction()
    {
        if (_wheelOneResult == _wheelTwoResult && _wheelTwoResult == _wheelThreeResult)
        {
            spawner1.SetSpawnConditions(jackpotFaceStats[(int)_wheelOneResult - 1]);
            if (jackpotFaceStats[(int)_wheelOneResult - 1].GetWaitForSpawner())
            {
                waitingForSpawner1 = true;
            }
            return;
        }
        spawner1.SetSpawnConditions(normalFaceStats[(int)_wheelOneResult - 1]);
        spawner2.SetSpawnConditions(normalFaceStats[(int)_wheelTwoResult - 1]);
        spawner3.SetSpawnConditions(normalFaceStats[(int)_wheelThreeResult - 1]);
        if (normalFaceStats[(int)_wheelOneResult - 1].GetWaitForSpawner())
        {
            waitingForSpawner1 = true;
        }
        if (normalFaceStats[(int)_wheelTwoResult - 1].GetWaitForSpawner())
        {
            waitingForSpawner2 = true;
        }
        if (normalFaceStats[(int)_wheelThreeResult - 1].GetWaitForSpawner())
        {
            waitingForSpawner3 = true;
        }
    }

    public IEnumerator OpenHatch()
    {
        if(Health == 3)
        {
            _LeftHatch.SetBool("HatchOpen", true);
        }
        if (Health == 2)
        {
            _RightHatch.SetBool("HatchOpen", true);
        }
        if (Health == 1)
        {
            _BottomHatch.SetBool("HatchOpen", true);
        }
        yield return new WaitForSeconds(_hatchOpenTime);
        SealHatch();
    }

    void SealHatch()
    {
        _hatchOpen = true;

        if(Health >= 2)
        {
            _LeftHatch.SetBool("HatchOpen", false);
        }
        if (Health >= 1)
        {
            _LeftHatch.SetBool("HatchOpen", false);
        }
        if (Health >= 0)
        {
            _LeftHatch.SetBool("HatchOpen", false);
        }
        _hatchOpen = false;
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

        if(UnityEngine.Random.Range(1,100) < _currJackpotOdds) //I promise it's still not rigged, just ignore the rigging code ;3
        {
            int jackpot = UnityEngine.Random.Range(1, 20);
            _wheelOneResult = _wheelTwoResult = _wheelThreeResult = (SlotResults)jackpot;
            _currJackpotOdds = jackpotOdds;
        }
        else
        {
            _wheelOneResult = (SlotResults)UnityEngine.Random.Range(1, 20);
            _wheelTwoResult = (SlotResults)UnityEngine.Random.Range(1, 20);
            _wheelThreeResult = (SlotResults)UnityEngine.Random.Range(1, 20);
            _currJackpotOdds += jackpotMod;
            
        }

        yield return new WaitForSeconds(UnityEngine.Random.Range(spinTimeMin, spinTimeMax));
        _canStop = true;
        
    }
    void AnimateSpin()
    {
        if (_wheel1Spin )
        {
            _leftSlot.transform.Rotate(_spinSpeed, 0, 0 * Time.deltaTime);
        }
        if (_wheel2Spin)
        {
            _rightSlot.transform.Rotate(_spinSpeed, 0, 0 * Time.deltaTime);
        }
        if (_wheel3Spin)
        {
            _rightSlot.transform.Rotate(_spinSpeed, 0, 0 * Time.deltaTime);
        }
    }
}
