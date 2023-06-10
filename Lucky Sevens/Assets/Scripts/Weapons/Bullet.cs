using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] int mDamage;
    [SerializeField] int mSpeed;
    [SerializeField] int mTimer;

    [SerializeField] Rigidbody rb;

    private Coroutine _timerCoroutine;
    // Start is called before the first frame update
    void OnEnable()
    {
        _timerCoroutine = StartCoroutine(TimerCoroutine());
        rb.velocity = transform.forward * mSpeed;
    }
    private void OnTriggerEnter(Collider other)
    {
        IDamage damageable = other.GetComponent<IDamage>();
        if(damageable != null)
        {
            damageable.takeDamage(mDamage,transform);
        }
        ObjectPoolManager.instance.ReturnObjToInfo(gameObject);
    }
    public void SetBulletSpeed(int speed)
    {
        mSpeed = speed;
    }
    private IEnumerator TimerCoroutine()
    {
        float elapsedTime = 0f;
        while (elapsedTime < mTimer)
        {
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        ObjectPoolManager.instance.ReturnObjToInfo(gameObject);
    }
}
