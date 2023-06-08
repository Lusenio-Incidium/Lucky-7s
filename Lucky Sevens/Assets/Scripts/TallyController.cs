using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TallyController : MonoBehaviour
{
    [Header("Tally Screen Objects")]
    [SerializeField] GameObject enemiesTxt;
    [SerializeField] GameObject timeTxt;
    [SerializeField] GameObject chipsUsedTxt;
    [SerializeField] GameObject chipsGatheredTxt;
    [SerializeField] GameObject chipsNetTxt;

    [Header("Tally Screen Texts")]
    [SerializeField] TextMeshProUGUI enemies;
    [SerializeField] TextMeshProUGUI time;
    [SerializeField] TextMeshProUGUI chipsUsed;
    [SerializeField] TextMeshProUGUI chipsGathered;
    [SerializeField] TextMeshProUGUI chipsNet;

    private void OnEnable()
    {
        enemiesTxt.SetActive(false);
        enemies.gameObject.SetActive(false);
        timeTxt.SetActive(false);
        time.gameObject.SetActive(false);
        chipsUsedTxt.SetActive(false);
        chipsUsed.gameObject.SetActive(false);
        chipsGatheredTxt.SetActive(false);
        chipsGathered.gameObject.SetActive(false);
        chipsNetTxt.SetActive(false);
        chipsNet.gameObject.SetActive(false);
    }

    public void EnemyEnable() 
    {
        enemiesTxt.SetActive(true);
        
    }

    public void TimeEnable()
    {
        enemies.gameObject.SetActive(true);
        timeTxt.SetActive(true);
        time.text = GameManager.instance.timeElapsed.ToString("F2");
    }

    public void ChipsUsedEnable()
    {
        time.gameObject.SetActive(true);
        chipsUsedTxt.SetActive(true);
    }

    public void ChipsGatheredEnable()
    {
        chipsUsed.gameObject.SetActive(true);
        chipsGatheredTxt.SetActive(true);
    }

    public void ChipsNetEnable()
    {
        chipsGathered.gameObject.SetActive(true);
        chipsNetTxt.SetActive(true);
    }

    public void ChipsNetTextEnable() 
    {
        chipsNet.gameObject.SetActive(true);
    }
}
