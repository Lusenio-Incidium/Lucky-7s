using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleMoverEnd : MonoBehaviour, IBattle
{
    [SerializeField] GameObject Barrier;
    public void OnBattleBegin()
    {
       gameObject.SetActive(true);
    }

    public void OnBattleEnd()
    {
        Barrier.SetActive(false);
        ItemMover movables = transform.GetComponent<ItemMover>();
        if(movables != null)
        {
            movables.OnBattleEnd();
        }
    }
}
