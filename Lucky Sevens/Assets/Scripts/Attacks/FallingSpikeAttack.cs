using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingSpikeAttack : MonoBehaviour
{
    [SerializeField] int damage;
    private void OnCollisionEnter(Collision collision)
    {
        IDamage damageTarg = collision.gameObject.GetComponent<IDamage>();
        if(damageTarg != null)
        {
            damageTarg.takeDamage(damage);
        }
        Destroy(gameObject);
    }
}
