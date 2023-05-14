using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class speedPickup : MonoBehaviour, ICollectable
{
    [SerializeField] [Range(1.0f, 10.0f)] float boostAmount;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            onCollect();
        }
    }

    public void onCollect()
    {
        GameManager.instance.playerScript.speedChange(boostAmount);
        Destroy(gameObject);
    }
}
