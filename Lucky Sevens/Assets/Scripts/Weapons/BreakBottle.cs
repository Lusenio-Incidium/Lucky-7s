using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakBottle : MonoBehaviour
{
    [SerializeField] Rigidbody rb;
    [SerializeField] GameObject puddle;
    [SerializeField] bool smashed = false;
    [SerializeField] float velocity;
    // Start is called before the first frame update
    private void Start()
    {
        rb.velocity = (transform.forward * velocity);
    }

    private void OnCollisionEnter(Collision other)
    {
        if (smashed)
        {
            Instantiate(puddle, gameObject.transform.position, other.transform.rotation);
            smashed = true;
            Destroy(gameObject);
        }

    }
}
