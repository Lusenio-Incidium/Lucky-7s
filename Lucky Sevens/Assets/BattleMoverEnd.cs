using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleMoverEnd : MonoBehaviour, IBattle
{
    [SerializeField] GameObject Barrier;
    [SerializeField] GameObject[] replaceBottles;
    [SerializeField] GameObject explodeBottle;
    public void OnBattleBegin()
    {
       gameObject.SetActive(true);
    }

    public void OnBattleEnd()
    {
        Barrier.SetActive(false);
        foreach(GameObject bottles in replaceBottles) 
        {
            Instantiate(explodeBottle,bottles.transform.position,bottles.transform.rotation);
            bottles.SetActive(false);
        }
        /*ItemMover movables = transform.GetComponent<ItemMover>();
        if(movables != null)
        {
            movables.OnBattleEnd();
        }*/
    }
}
