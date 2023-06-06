using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FakeEnemy : MonoBehaviour, IDamage, IBattleEnemy
{
    BattleManager battleManager;

    public void SetBattleManager(BattleManager manager)
    {
        battleManager = manager;
    }

    public void takeDamage(float num)
    {
        if(battleManager != null)
        {
            battleManager.DeclareDeath(1);
        }
        Destroy(gameObject);
    }

    public void instaKill()
    {

    }
}
