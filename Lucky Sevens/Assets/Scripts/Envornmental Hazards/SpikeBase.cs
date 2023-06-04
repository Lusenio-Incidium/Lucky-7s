using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeBase : MonoBehaviour
{
    [Header("Spikes")]
    [SerializeField] SpikeAttack spikes;
    [Header("TriggerMethod")]
    [SerializeField] bool triggerOnStep;

    [Header("Timed Strike Settings")]
    [SerializeField] int strikePeriod;
    bool striking;
    // Update is called once per frame

    void Update()
    {
        if(!striking && !triggerOnStep)
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
        if (triggerOnStep)
        {
            spikes.TriggerAttack();
        }
    }
}
