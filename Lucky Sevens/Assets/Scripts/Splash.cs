using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Splash : MonoBehaviour
{
    [SerializeField] StatusEffectObj status;
    [SerializeField] Collider sphere;
    [SerializeField] float timer;

    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, timer);
    }
    // Update is called once per frame
    private void OnTriggerEnter(Collider other)
    {
        IStatusEffect statusable = other.GetComponent<IStatusEffect>();
        if (statusable != null)
        {
            statusable.ApplyStatusEffect(status);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        IStatusEffect statusable = other.GetComponent<IStatusEffect>();
        if (statusable != null)
        {
            statusable.ApplyStatusEffect(status);
            sphere.enabled = true;
        }
    }
}
