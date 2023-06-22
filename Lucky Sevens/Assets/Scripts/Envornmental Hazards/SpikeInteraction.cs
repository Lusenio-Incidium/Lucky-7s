using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeInteraction : MonoBehaviour
{
    [SerializeField] int damage = 0;
    [SerializeField] int knockback = 0;
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Bingo Enemy"))
        {
            IDamage damageTarg = other.GetComponent<IDamage>();
            if (damageTarg != null)
            {
                damageTarg.takeDamage(damage);
            }
            IPhysics physics = other.GetComponent<IPhysics>();
            if (physics != null)
            {
                physics.TakePush(Vector3.up * knockback);
            }
        }
    }
}
