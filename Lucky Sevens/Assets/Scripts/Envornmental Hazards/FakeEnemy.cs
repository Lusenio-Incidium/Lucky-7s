using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FakeEnemy : MonoBehaviour, IDamage, IBattleEnemy
{
    BattleManager battleManager;
    bool dead;

    public void SetBattleManager(BattleManager manager)
    {
        battleManager = manager;
    }

    public void takeDamage(float num, Transform pos = null)
    {
        if (dead)
        {
            return;
        }
        if(battleManager != null)
        {
            battleManager.DeclareDeath(1);
        }
        Destroy(gameObject);
        dead = true;
    }

    public void instaKill()
    {

    }
}
