using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeAttack : MonoBehaviour
{
    [SerializeField] BoxCollider bc;
    [SerializeField] AudioSource noiseyBoi;
    [Header("--- Noises for Noisey Boi ---")]
    [SerializeField] AudioClip prime;
    [Range(0,1)][SerializeField] float primeVol;
    [SerializeField] AudioClip strike;
    [Range(0, 1)][SerializeField] float strikeVol;
    [Header("--- Attack Settings ---")]
    [SerializeField] float chargeTime;
    [SerializeField] float attackTime;
    [SerializeField] bool startExtruded;
    [SerializeField] bool active;
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
        if (active && startExtruded)
        {
            Extrude();
        }
    }
    public void TriggerAttack()
    {
        if (attacking )
        {
            return;
        }
        StartCoroutine(attack());
    }
    IEnumerator attack()
    {
        attacking = true;
        animator.ResetTrigger("Hide");
        if (active)
        {
            animator.SetTrigger("Reveal");
            noiseyBoi.PlayOneShot(prime, primeVol);
            yield return new WaitForSeconds(chargeTime);
        }
        if (active)
        {
            animator.SetTrigger("Strike");
            bc.enabled = true;
            noiseyBoi.PlayOneShot(strike, strikeVol);
            yield return new WaitForSeconds(attackTime);
        }
        if (active)
        {
            bc.enabled = false;
            animator.SetTrigger("Hide");

            yield return new WaitForSeconds(.3f);
        }
        attacking = false;
        if (destroyAfterAttack)
        {
            Destroy(gameObject);
        }
    }
    public void Hide()
    {
        bc.enabled = false;
        animator.ResetTrigger("Strike");
        animator.ResetTrigger("Reveal");
        animator.SetTrigger("Hide");
        active = false;
    }

    public void Extrude()
    {
        animator.ResetTrigger("Hide");
        animator.SetTrigger("Reveal");
        animator.SetTrigger("Strike");
        bc.enabled = true;
        active = false;
    }
    public void RestoreActive()
    {
        animator.SetTrigger("Hide");
        animator.ResetTrigger("Reveal");
        animator.ResetTrigger("Strike");
        active = true;
    }
}
