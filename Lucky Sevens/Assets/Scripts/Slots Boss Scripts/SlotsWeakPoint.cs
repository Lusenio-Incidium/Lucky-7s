using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlotsWeakPoint : MonoBehaviour, IDamage
{
    [Header("--- Weak Point Data ---")]
    [SerializeField] int health;
    // Start is called before the first frame update
    void Start()
    {
        SlotsController.instance.UpdateWeakPoints(1);
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            takeDamage(1000);
        }
    }

    public void takeDamage(int count)
    {
        health -= count;
        if(health <= 0) 
        {
            SlotsController.instance.UpdateWeakPoints(-1);
            Destroy(gameObject);
        }
    }
    // Update is called once per frame
}
