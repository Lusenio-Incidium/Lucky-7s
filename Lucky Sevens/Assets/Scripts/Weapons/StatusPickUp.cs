using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusPickUp : MonoBehaviour,ICollectable
{
    [SerializeField] StatusEffectObj statusToGive;
    public void onCollect()
    {
        GameManager.instance.gunSystem.AddStatus(statusToGive);
        Destroy(gameObject);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            onCollect();
        }
    }
}
