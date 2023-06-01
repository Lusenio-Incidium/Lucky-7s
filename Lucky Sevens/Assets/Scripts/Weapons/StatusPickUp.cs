using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusPickUp : MonoBehaviour,ICollectable
{
    [SerializeField] StatusEffectObj statusToGive;
    [SerializeField] GameObject particleEffect;

    private GameObject effectPart;
    public void onCollect()
    {
        if (GameManager.instance.gunSystem.weapons.Count > 0)
        {
            GameManager.instance.gunSystem.AddStatus(statusToGive);
            Destroy(gameObject);
            Destroy(effectPart);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //particleEffect.SetActive(false);
            onCollect();
        }
    }
    void Start()
    {
        effectPart = Instantiate(particleEffect, transform.position, transform.rotation);
    }
}
