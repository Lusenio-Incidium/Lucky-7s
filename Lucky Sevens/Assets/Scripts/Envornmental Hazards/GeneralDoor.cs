using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneralDoor : MonoBehaviour, IButtonTrigger, ICannonKey, IBattle
{
    [SerializeField] Animator animator;

    public void OnButtonPress()
    {
        animator.SetBool("Open", true);
    }
    public void OnButtonRelease()
    {
        animator.SetBool("Open", false);

    }
    public void OnCannonDeath()
    {
        animator.SetBool("Open", true);
    }

    public void OnBattleBegin()
    {
        animator.SetBool("Open", false);

    }

    public void OnBattleEnd()
    {
        animator.SetBool("Open", true);

    }
}
