using System.Collections;
using UnityEngine;

public class ThrowableStatusBottle : MonoBehaviour
{
    [SerializeField] Color potionColor;
    [SerializeField] StatusEffectObj statusEffect;
    [SerializeField] Rigidbody rb;
    [SerializeField] SphereCollider sc;
    [SerializeField] Renderer theRenderer;
    [SerializeField] float lingerTime;
    [SerializeField] int speed;
    [SerializeField] int radius;
    bool smashed;
    void Start()
    {
        rb.velocity = transform.forward * speed;
        sc.enabled = false;
        theRenderer.material.color = potionColor;
        sc.radius = radius;
    }
    private void OnCollisionEnter(Collision collision)
    {
        StartCoroutine(smash());
    }
    private void OnTriggerEnter(Collider other)
    {
        if (smashed)
        {
            IStatusEffect effectable = other.GetComponent<IStatusEffect>();
            if (effectable == null)
            {
                return;
            }
            effectable.ApplyStatusEffect(statusEffect);
        }
    }

    IEnumerator smash()
    {
        smashed = true;
        sc.enabled = true;
        theRenderer.enabled = false;
        rb.constraints = RigidbodyConstraints.FreezePositionX;
        rb.constraints = RigidbodyConstraints.FreezePositionZ;
        rb.constraints = RigidbodyConstraints.FreezeRotation;
        yield return new WaitForSeconds(lingerTime);
        Destroy(gameObject);
    }
}
