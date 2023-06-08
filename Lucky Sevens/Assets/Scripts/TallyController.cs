using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TallyController : MonoBehaviour
{
    [SerializeField] GameObject enemiesTxt;
    [SerializeField] GameObject timeTxt;
    [SerializeField] GameObject chipsUsedTxt;
    [SerializeField] GameObject chipsGatheredTxt;
    [SerializeField] GameObject chipsNetTxt;

    public void EnemyEnable() 
    {
        enemiesTxt.SetActive(true);
    }

    public void TimeEnable()
    {
        timeTxt.SetActive(true);
    }

    public void ChipsUsedEnable()
    {
        chipsUsedTxt.SetActive(true);
    }

    public void ChipsGatheredEnable()
    {
        chipsGatheredTxt.SetActive(true);
    }

    public void ChipsNetEnable()
    {
        chipsNetTxt.SetActive(true);
    }
}
