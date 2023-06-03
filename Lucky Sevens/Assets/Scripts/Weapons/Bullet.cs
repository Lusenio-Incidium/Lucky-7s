using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] int mDamage;
    [SerializeField] int mSpeed;
    [SerializeField] int mTimer;

    [SerializeField] Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, mTimer);
        rb.velocity = transform.forward * mSpeed;
    }
    private void OnTriggerEnter(Collider other)
    {
        IDamage damageable = other.GetComponent<IDamage>();
        if(damageable != null)
        {
            damageable.takeDamage(mDamage);
        }
        Destroy(gameObject);
    }
    public void SetBulletSpeed(int speed)
    {
        mSpeed = speed;
    }
}
