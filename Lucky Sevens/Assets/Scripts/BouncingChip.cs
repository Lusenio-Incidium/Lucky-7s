using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class BouncingChip : MonoBehaviour
{
    [SerializeField] Rigidbody rb;
    [SerializeField] int damage;
    [SerializeField] int knockBack;
    [SerializeField] float speed;
    [SerializeField] int destroyTimer;

    bool touchdown;
    Vector3 prevVelo;
    Vector3 reflectDirect;
    private void Start()
    {

        Destroy(gameObject, destroyTimer);
        transform.Rotate(10, 0, 0 * Time.deltaTime);
    }

    
    private void LateUpdate()
    {
        prevVelo = rb.velocity;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!touchdown && collision.gameObject.CompareTag("Untagged"))
        {
            rb.useGravity = false;
            rb.freezeRotation = true;
            rb.velocity = new Vector3(Random.Range(0, speed), 0, Random.Range(0, speed));
            transform.rotation = Quaternion.Euler(0, 0, 0);
            touchdown = true;
            return;
        }
        IDamage damageTarg = collision.gameObject.GetComponent<IDamage>();
        if (damageTarg != null)
        {
            damageTarg.takeDamage(damage);
            IPhysics physics = collision.gameObject.GetComponent<IPhysics>();
            if (physics != null)
            {
                Vector3 dir = collision.gameObject.transform.position - transform.position;
                physics.TakePush(dir * knockBack);
            }
        }
            reflectDirect = Vector3.Reflect(prevVelo.normalized, collision.contacts[0].normal);
            rb.velocity = reflectDirect * speed;
    }
}
