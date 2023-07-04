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
    [SerializeField] GameObject retry;
    [SerializeField] GameObject returnHub;
    [SerializeField] GameObject endGame;

    [Header("Tally Screen Texts")]
    public TextMeshProUGUI enemies;
    public TextMeshProUGUI time;
    public TextMeshProUGUI chipsUsed;
    public TextMeshProUGUI chipsGathered;
    public TextMeshProUGUI chipsNet;

    bool isBoss;
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

        if (GameObject.FindGameObjectWithTag("Boss"))
            isBoss = true;
    }

    public void EnemyEnable() 
    {
        enemiesTxt.SetActive(true);
        enemies.text = GameManager.instance.enemiesKilled.ToString();
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
        chipsUsed.text = GameManager.instance.ammoUsedTotal.ToString();
    }

    public void ChipsGatheredEnable()
    {
        chipsUsed.gameObject.SetActive(true);
        chipsGatheredTxt.SetActive(true);
        chipsGathered.text = GameManager.instance.ammoGatheredTotal.ToString();
    }

    public void ChipsNetEnable()
    {
        chipsGathered.gameObject.SetActive(true);
        chipsNetTxt.SetActive(true);
        chipsNet.text = (GameManager.instance.ammoGatheredTotal - GameManager.instance.ammoUsedTotal).ToString();
    }

    public void ChipsNetTextEnable() 
    {
        chipsNet.gameObject.SetActive(true);
        retry.SetActive(true);
        if(isBoss)
            endGame.SetActive(true);
        else
            returnHub.SetActive(true);
    }
}
