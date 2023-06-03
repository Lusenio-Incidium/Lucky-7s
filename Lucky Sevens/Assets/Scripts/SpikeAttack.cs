using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeAttack : MonoBehaviour
{
    [SerializeField] BoxCollider bc;
    [SerializeField] float chargeTime;
    [SerializeField] float attackTime;
    [SerializeField] int damage;
    [SerializeField] Animator animator;
    IEnumerator Start()
    {
        yield return new WaitForSeconds(chargeTime);
        animator.SetTrigger("Strike");
        bc.enabled = true;
        yield return new WaitForSeconds(attackTime);
        animator.SetTrigger("Hide");
        yield return new WaitForSeconds(.3f);
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        IDamage damageTarg = other.GetComponent<IDamage>();
        if(damageTarg != null)
        {
            damageTarg.takeDamage(damage);
        }
        IPhysics physics = other.GetComponent<IPhysics>();
        if (physics != null)
        {
            physics.TakePush(Vector3.up * 50);
        }
    }
}
