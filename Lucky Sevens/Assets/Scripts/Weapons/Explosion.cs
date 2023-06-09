using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    [SerializeField] int damage = 0;
    [SerializeField] int pushAmount = 0;
    [SerializeField] float duration = 0;
    [SerializeField] float shakeAmount = 0;
    [SerializeField] ParticleSystem explosionPrefab;

    // Start is called before the first frame update
    void Start()
    {
        Instantiate(explosionPrefab, transform.position, transform.rotation);
        Destroy(gameObject, 0.15f);
    }

    // Update is called once per frame
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Bingo Enemy"))
        {
            if (Vector3.Distance(other.transform.position, transform.position) < gameObject.transform.localScale.x)
            {
                IPhysics physics = other.GetComponent<IPhysics>();
                if (physics != null)
                {
                    Vector3 dir = other.transform.position - transform.position;
                    physics.TakePush(dir * pushAmount);
                }
                IDamage damageable = other.GetComponent<IDamage>();
                if (damageable != null)
                {
                    damageable.takeDamage(damage);
                }
            }
            CameraShake playerCam = GameManager.instance.playerCam.GetComponent<CameraShake>();
            playerCam.SetStrengthAmount(shakeAmount);
            playerCam.SetDuration(duration);
            playerCam.start = true;
        }
    }

}
