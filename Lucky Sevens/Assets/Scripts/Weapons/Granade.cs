using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Granade : MonoBehaviour
{
    [SerializeField] GameObject explosion;
    [SerializeField] Rigidbody rb;
    [SerializeField] bool explodeOnImpact;
    [SerializeField] int fuseTimer;
    [SerializeField] int velocity;

    IEnumerator Start()
    {
        rb.velocity = (transform.forward * velocity);
        yield return new WaitForSeconds(fuseTimer);
        Instantiate(explosion, transform.position, explosion.transform.rotation);
        Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (explodeOnImpact)
        {
            Instantiate(explosion, transform.position, explosion.transform.rotation);
            Destroy(gameObject);
        }
    }
}
