using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwingingAxe : MonoBehaviour, IButtonTrigger
{
    [SerializeField] Animator animator;
    [SerializeField] BoxCollider damageHitBox;
    [SerializeField] float speedMod;
    [SerializeField] bool startSwinging;
    [SerializeField] int damage;
    [SerializeField] int knockback;
    [SerializeField] bool swingOnButton;
    private void Start()
    {
        animator.speed = speedMod;
        if (startSwinging)
        {
            BeginSwinging();
        }
    }
    public void BeginSwinging()
    {
        animator.SetBool("Swing", true);
        damageHitBox.enabled = true;
    }
    public void StopSwinging()
    {
        animator.SetBool("Swing", false);
        damageHitBox.enabled = false;
    }


    private void OnTriggerEnter(Collider other)
    {
        IDamage damageTarg = other.GetComponent<IDamage>();
        if (damageTarg != null)
        {
            damageTarg.takeDamage(damage);
        }
        IPhysics physics = other.GetComponent<IPhysics>();
        if (physics != null)
        {
            Vector3 dir = other.gameObject.transform.position - transform.position;
            physics.TakePush(dir * knockback);
        }
    }

    public void OnButtonPress()
    {
        if (swingOnButton)
        {
            BeginSwinging();
        }
        else
        StopSwinging();
    }

    public void OnButtonRelease()
    {
        if (!swingOnButton)
        {
            BeginSwinging();
        }
        else
            StopSwinging();
    }
}
