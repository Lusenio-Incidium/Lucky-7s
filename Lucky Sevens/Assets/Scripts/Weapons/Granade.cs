using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Granade : MonoBehaviour
{
    [SerializeField] Rigidbody rb;
    [SerializeField] SphereCollider sc;
    [SerializeField] Renderer theRenderer;
    [SerializeField] float timer;
    [SerializeField] float hitBoxTimer;
    [SerializeField] int range;
    [SerializeField] int speed;
    [SerializeField] int damage;
    void Start()
    {
        rb.velocity = transform.forward * speed;
        sc.enabled = false;
        StartCoroutine(Ignite());
    }

    IEnumerator Ignite()
    {
        yield return new WaitForSeconds(timer);
        sc.enabled = true;
        sc.radius = range;
        theRenderer.enabled = false;
        Destroy(gameObject, hitBoxTimer);
    }

    private void OnTriggerEnter(Collider other)
    {
        IDamage damagable = other.GetComponent<IDamage>();
        if(damagable == null)
        {
            return;
        }
        damagable.takeDamage(damage);
    }
}
