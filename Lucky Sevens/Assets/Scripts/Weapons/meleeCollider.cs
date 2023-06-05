using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class meleeCollider : MonoBehaviour
{
    [SerializeField] int damage;

    // Start is called before the first frame update
    void Start()
    {

    }
    private void OnTriggerEnter(Collider other)
    {
        IDamage damageable = other.GetComponent<IDamage>();
        if (damageable != null && other.CompareTag("Player"))
        {
            damageable.takeDamage(damage);
        }
    }
}
