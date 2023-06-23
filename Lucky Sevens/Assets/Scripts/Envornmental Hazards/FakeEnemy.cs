using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FakeEnemy : MonoBehaviour, IDamage, IBattleEnemy
{
    BattleManager battleManager;
    [SerializeField] AudioSource poofSource;
    [SerializeField] AudioClip[] poofSounds;
    [Range(0,1)][SerializeField] float poofVol;
    bool dead;

    private void Start()
    {
        poofVol = GameManager.instance.playerScript.GetJumpVol();
        poofSource.PlayOneShot(poofSounds[Random.Range(0, poofSounds.Length - 1)], poofVol);
    }

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
