using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeAttack : MonoBehaviour
{
    [SerializeField] BoxCollider bc;
    [SerializeField] float chargeTime;
    [SerializeField] float attackTime;
    [SerializeField] Animator animator;
    [SerializeField] bool destroyAfterAttack;
    [SerializeField] bool attackOnSpawn;
    bool attacking;
    void Start()
    {
        if (attackOnSpawn)
        {
            StartCoroutine(attack());
        }
    }
    public void TriggerAttack()
    {
        if (attacking)
        {
            return;
        }
        StartCoroutine(attack());
    }
    IEnumerator attack()
    {
        attacking = true;
        animator.SetTrigger("Reveal");
        yield return new WaitForSeconds(chargeTime);
        animator.SetTrigger("Strike");
        bc.enabled = true;
        yield return new WaitForSeconds(attackTime);
        animator.SetTrigger("Hide");
        yield return new WaitForSeconds(.3f);
        attacking = false;
        if (destroyAfterAttack)
        {
            Destroy(gameObject);
        }

    }

}
