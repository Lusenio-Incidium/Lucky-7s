using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlotsWeakPoint : MonoBehaviour, IDamage
{
    [Header("--- Weak Point Data ---")]
    [SerializeField] int health;
    [SerializeField] int trackSpeed;
    [SerializeField] Transform barrelPosition;
    bool active;
    // Start is called before the first frame update
    void Start()
    {
        active = false;

    }
    private void Update()
    {
        if (active)
        {
            TurnToPlayer();
        }
    }
    void TurnToPlayer()
    {
        Quaternion rot = Quaternion.LookRotation(new Vector3(GameManager.instance.player.transform.position.x, GameManager.instance.player.transform.position.y, GameManager.instance.player.transform.position.z) - transform.position);
        transform.rotation = Quaternion.Lerp(transform.rotation, rot, Time.deltaTime * trackSpeed);
        Debug.DrawRay(transform.position, GameManager.instance.player.transform.position);
    }
    public void Activate() 
    {
        if(active == true)
        {
            return;
        }
        SlotsController.instance.UpdateWeakPoints(1);
        active = true;
    }

    public void takeDamage(int count)
    {
        health -= count;
        if(health <= 0) 
        {
            active = false;
            SlotsController.instance.UpdateWeakPoints(-1);
        }
    }
    // Update is called once per frame
}
