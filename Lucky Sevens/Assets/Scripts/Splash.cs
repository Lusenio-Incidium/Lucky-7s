using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Splash: MonoBehaviour
{
    [SerializeField] StatusEffectObj status;
    [SerializeField] Collider sphere;
    [SerializeField] float timer;
    [SerializeField] bool isObject;

    // Start is called before the first frame update
    void Start()
    {
        if (!isObject)
        {
            Destroy(gameObject, timer);
        }
    }
    // Update is called once per frame
    private void OnTriggerEnter(Collider other)
    {
        if (Vector3.Distance(other.transform.position, transform.position) < gameObject.transform.localScale.x)
        {
            IStatusEffect statusable = other.GetComponent<IStatusEffect>();
            if (statusable != null)
            {
                statusable.ApplyStatusEffect(status);
            }
        }
    }
/*    private void OnTriggerExit(Collider other)
    {
        IStatusEffect statusable = other.GetComponent<IStatusEffect>();
        if (statusable != null)
        {
            statusable.ApplyStatusEffect(status);
            sphere.enabled = true;
        }
    }*/
}
