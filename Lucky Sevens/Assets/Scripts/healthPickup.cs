using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class healthPickup : MonoBehaviour, ICollectable
{
    [SerializeField] Material mat;
    [SerializeField] [Range(10, 100)] int healAmount;
    

    // Start is called before the first frame update
    void Start()
    {
        if (healAmount >= 100)
        {
            mat.color = Color.red;
        }
        else if (healAmount >= 50)
        {
            mat.color = Color.green;
        }
        else
        {
            mat.color = Color.blue;
        }
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
        GameManager.instance.playerScript.playerHeal(healAmount);
        Destroy(gameObject);
    }
}
