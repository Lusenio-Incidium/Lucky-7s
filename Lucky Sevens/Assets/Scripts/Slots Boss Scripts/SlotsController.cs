
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
    [Header("Wheels")]

    [SerializeField] GameObject _leftSlot;
    [SerializeField] GameObject _middleSlot;
    [SerializeField] GameObject _rightSlot;
    [Header("Animators")]

    [SerializeField] Animator _LeftHatch;
    [SerializeField] Animator _RightHatch;
    [SerializeField] Animator _BottomHatch;
    [SerializeField] Animator _TopHatch;
    [SerializeField] Animator _Lever;

    [Header("Spawners")]

    [SerializeField] ArenaSpawner spawner1;
    [SerializeField] ArenaSpawner spawner2;
    [SerializeField] ArenaSpawner spawner3;

    [Header("Cannons")]
    [SerializeField] SlotsWeakPoint cannon1;
    [SerializeField] SlotsWeakPoint cannon2;
    [SerializeField] SlotsWeakPoint cannon3;
    [SerializeField] SlotsWeakPoint cannon4;

    [Header("--- Slot Stats ---")]
    [Range(1, 50)][SerializeField] float _hatchOpenTime;
    [Range(1,100)][SerializeField] float _spinSpeed;
    [Range(0.1f, 1)][SerializeField] float _spinStartDelay;
   
    [Range(1, 10)][SerializeField] float spinTimeMax; //How long it spins before it stops
    [Range(1, 10)][SerializeField] float spinTimeMin; 
    [Range(1, 500)][SerializeField] float spinDelayMax; //How long it takes to spin up again
    [Range(1, 500)][SerializeField] float spinDelayMin; //How long it takes to spin up again
    [Range(0.1f, 10)][SerializeField] float wheelStopDelayMax;
    [Range(0.1f, 10)][SerializeField] float wheelStopDelayMin;
    [Range(1, 10)][SerializeField] int HaywireMod;

    float _currSpinDelay;
    float _currStopDelay;
    bool _isSpinning;
    bool _canStop;
    bool _wheel1Spin;
    bool _wheel2Spin;
    bool _wheel3Spin;
    int cannonCount;

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

    bool waitingForSpawner1;
    bool waitingForSpawner2;
    bool waitingForSpawner3;

    // Start is called before the first frame update
    void Start()
    {
        _isSpinning = _wheel1Spin = _wheel2Spin = _wheel3Spin = false;
        _currSpinDelay = UnityEngine.Random.Range(spinDelayMin, spinDelayMax);
        _currJackpotOdds = jackpotOdds;
        isStunned = true;
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
    public void Begin()
    {
        unStun();
        _currSpinDelay = UnityEngine.Random.Range(0, 1);
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

    public void ForceStopSpawn()
    {
        spawner1.ForceStop();
        spawner2.ForceStop();
        spawner3.ForceStop();
        waitingForSpawner1 = false;
        waitingForSpawner2 = false;
        waitingForSpawner3 = false;

    }

    public IEnumerator DamageWheel()
    {
        Health--;
        if(Health == 2)
        {
            yield return new WaitForSeconds(2f);
            //_wheel1HayWire = true;
            //_wheel1Spin = false;
            unStun();

        }
        if(Health == 1)
        {
            yield return new WaitForSeconds(2f);
            //_wheel3HayWire = true;
            //_wheel3Spin = false;
            unStun();

        }
        if (Health == 0)
        {
            WinnersToken.instance.Spawn();
        }
    }

    public IEnumerator StunWheel()
    {
        cannonCount++;
        if (cannonCount >= 4)
        {
            ForceStopSpawn();
            isStunned = true;
            _wheel1Spin = _wheel2Spin = _wheel3Spin = false;
            _currStopDelay = 0;
            _isSpinning = false;
            _canStop = false;
            OpenHatch();
            _currSpinDelay = UnityEngine.Random.Range(spinDelayMin, spinDelayMax);
            cannonCount = 0;
            yield return new WaitForSeconds(_hatchOpenTime);
            unStun();
            
        }
    }

    void unStun()
    {
        if (!isStunned || Health <= 0)
        {
            return;
        }
        isStunned = false;
        SealHatch();
        if (Health < 2)
        {
            SpawnCannons(true);
        }
        else
        {
            SpawnCannons(false);
        }
        if (Health < 3)
        {
            MoveCannons();
        }
        _currSpinDelay = UnityEngine.Random.Range(1, 3);
    }




    //Slots Logic
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

            if (_wheel1Spin)
            {
                _wheel1Spin = false;
                _leftSlot.transform.rotation = Quaternion.Euler(((360 / 20) * (int)_wheelOneResult) + (90 - (360 / 20)), 0, 0);
                _currStopDelay = UnityEngine.Random.Range(wheelStopDelayMin, wheelStopDelayMax);
            }

            if (!_wheel1Spin && _currStopDelay <= 0 && _wheel2Spin)
            {
                _wheel2Spin = false;
                _middleSlot.transform.rotation = Quaternion.Euler(((360 / 20) * (int)_wheelTwoResult) + (90 - (360 / 20)), 0, 0);
                _currStopDelay = UnityEngine.Random.Range(wheelStopDelayMin, wheelStopDelayMax);
            }
            if (!_wheel2Spin && _currStopDelay <= 0 && _wheel3Spin) { 
                _rightSlot.transform.rotation = Quaternion.Euler(((360 / 20) * (int)_wheelThreeResult) + (90 - (360 / 20)), 0, 0);
                _wheel3Spin = false;
                _isSpinning = false;
                _currSpinDelay = UnityEngine.Random.Range(spinDelayMin, spinDelayMax);
                SpinAction();
                Debug.Log((int)_wheelOneResult + " " + (int)_wheelTwoResult + " " + (int)_wheelThreeResult);
            }
        }
        else if (!waitingForSpawner1 && !waitingForSpawner2 && !waitingForSpawner3)
        {
            _currSpinDelay -= Time.deltaTime;
        }
    }

    void SpinAction()
    {
        UpdateSpawnLocations();
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

    void UpdateSpawnLocations()
    {
        Transform[] SpawnLocations = GetActiveCannons();
        foreach(SpawnConditions sc in normalFaceStats)
        {
            if(sc.GetSpawnStyles() == SpawnStyles.FireAtPlayer || sc.GetSpawnStyles() == SpawnStyles.FireOnAllFronts)
            {
                sc.SetSpawnLocations(SpawnLocations);
            }
        }
        foreach (SpawnConditions sc in jackpotFaceStats)
        {
            if (sc.GetSpawnStyles() == SpawnStyles.FireAtPlayer || sc.GetSpawnStyles() == SpawnStyles.FireOnAllFronts)
            {
                sc.SetSpawnLocations(SpawnLocations);
            }
        }
    }
    IEnumerator Spin()
    {
        _Lever.SetTrigger("Pull");
        _isSpinning = true;
        _canStop = false;
        yield return new WaitForSeconds(.5f);
        _wheel1Spin = true;
        yield return new WaitForSeconds(_spinStartDelay);
        _wheel2Spin = true;
        yield return new WaitForSeconds(_spinStartDelay);
        _wheel3Spin = true;
        if(UnityEngine.Random.Range(1,100) < _currJackpotOdds) 
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
            _middleSlot.transform.Rotate(_spinSpeed, 0, 0 * Time.deltaTime);
        }
        if (_wheel3Spin)
        {
            _rightSlot.transform.Rotate(_spinSpeed, 0, 0 * Time.deltaTime);
        }

    }
    public void OpenHatch()
    {
        if (_hatchOpen)
        {
            return;
        }
        _hatchOpen = true;


        if (Health == 3)
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
    }

    void SealHatch()
    {
        if(Health >= 2)
        {
            _LeftHatch.SetBool("HatchOpen", false);
        }
        if (Health >= 1)
        {
            _RightHatch.SetBool("HatchOpen", false);
        }
        if (Health >= 0)
        {
            _BottomHatch.SetBool("HatchOpen", false);
        }
        _hatchOpen = false;
    }

    //Here comes the Cannon Method Section.
    void SpawnCannons(bool reinforced)
    {
        cannon1.Respawn(reinforced);
        cannon2.Respawn(reinforced);
        cannon3.Respawn(reinforced);
        cannon4.Respawn(reinforced);
    }
    void MoveCannons()
    {
        if (cannon1.GetHealth() > 0)
            cannon1.StartMoving();
        if (cannon2.GetHealth() > 0)
            cannon2.StartMoving();
        if (cannon3.GetHealth() > 0)
            cannon3.StartMoving();
        if (cannon4.GetHealth() > 0)
            cannon4.StartMoving();
    }

    Transform[] GetActiveCannons()
    {
        List<Transform> returnList = new List<Transform>();
        if (cannon1.GetHealth() > 0 && cannon1.GetActive())
            returnList.Add(cannon1.giveBarrel());
        if (cannon2.GetHealth() > 0 && cannon2.GetActive())
            returnList.Add(cannon2.giveBarrel());

        if (cannon3.GetHealth() > 0 && cannon3.GetActive())
            returnList.Add(cannon3.giveBarrel());

        if (cannon4.GetHealth() > 0 && cannon4.GetActive())
            returnList.Add(cannon4.giveBarrel());
        return returnList.ToArray();
    }
}
