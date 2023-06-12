using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeBase : MonoBehaviour, IButtonTrigger, IBattle, ICannonKey
{
    private enum Functions
    {
        None,
        StartTimedStrike,
        StartTriggerStrike,
        Extrude,
        Disarm
    }

    [Header("Spikes")]
    [SerializeField] SpikeAttack spikes;
    [Header("Trigger Method")]
    [SerializeField] bool triggerOnStep;
    [SerializeField] bool active;
    [SerializeField] bool onlyPlayerCanTrigger;
    [Header("Timed Strike Settings")]
    [SerializeField] int strikePeriod;

    [Header("Trigger Functions")]
    [SerializeField] Functions onButtonPress;
    [SerializeField] Functions onButtonRelease;
    [SerializeField] Functions onBattleBegin;
    [SerializeField] Functions onBattleEnd;
    [SerializeField] Functions onCannonDeath;
    bool striking;
    // Update is called once per frame
    private void Start()
    {
        if (active)
        {
            spikes.RestoreActive();
        }
    }
    void Update()
    {
        if(!striking && !triggerOnStep && active)
        {
            StartCoroutine(timedStrike());

        }
    }

    IEnumerator timedStrike()
    {
        striking = true;
        spikes.TriggerAttack();
        yield return new WaitForSeconds(strikePeriod);
        striking = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (triggerOnStep && active && ((onlyPlayerCanTrigger && other.CompareTag("Player")) || !onlyPlayerCanTrigger))
        {
            spikes.TriggerAttack();
        }
    }

    private Functions FunctionActions(Functions function)
    {
        switch (function) 
        {
            case Functions.None:
                break;
            case Functions.StartTimedStrike:
                spikes.RestoreActive();
                active = true;
                triggerOnStep = false;
                break;
            case Functions.StartTriggerStrike:
                spikes.RestoreActive();
                active = true;
                triggerOnStep = true;
                break;
            case Functions.Extrude:
                spikes.Extrude();
                active = false;
                break;
            case Functions.Disarm:
                spikes.Hide();
                active = false;
                break;
        }
        return function;
    }

    public void OnBattleBegin()
    {
        onBattleBegin = FunctionActions(onBattleBegin);
    }

    public void OnBattleEnd()
    {
        onBattleEnd = FunctionActions(onBattleEnd);
    }

    public void OnButtonPress()
    {
        onButtonPress = FunctionActions(onButtonPress);
    }

    public void OnButtonRelease()
    {
        onButtonRelease = FunctionActions(onButtonRelease);
    }

    public void OnCannonDeath()
    {
        onCannonDeath = FunctionActions(onCannonDeath);
    }
}
