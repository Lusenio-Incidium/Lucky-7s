using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CrushingCeiling : MonoBehaviour, IButtonTrigger
{
    [SerializeField] BoxCollider bc;
    [SerializeField] float attackTime;
    [SerializeField] float attackSpeedMod;
    [SerializeField] float pauseTime;
    [SerializeField] Animator animator;
    [SerializeField] bool crushOnTrigger;
    bool attacking;
    bool playerInside;
    private void Start()
    {
        animator.speed = attackSpeedMod;
    }
    void Update()
    {

        if (!crushOnTrigger)
        {
            TriggerAttack();
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
        animator.SetTrigger("Strike");
        bc.enabled = true;
        yield return new WaitForSeconds(attackTime);
        animator.SetTrigger("Hide");
        yield return new WaitForSeconds(pauseTime);
        bc.enabled = false;
        attacking = false;
    }

    public void OnButtonPress()
    {
        TriggerAttack();
    }

    public void OnButtonRelease() { } //Here to please the Interface


    private void OnTriggerEnter(Collider other)
    {
        playerInside = true;
    }
    private void OnTriggerExit(Collider other)
    {
        playerInside = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player") && GameManager.instance.player.GetComponent<PlayerController>())
        {
            GameManager.instance.player.GetComponent<IDamage>().instaKill();
        }
    }
}

