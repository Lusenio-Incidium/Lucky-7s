using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions.Must;

public class SlotsWeakPoint : MonoBehaviour, IDamage
{
    [Header("--- Weak Point Data ---")]
    [SerializeField] int health;
    [SerializeField] int trackSpeed;
    [SerializeField] Transform barrelPosition;
    [SerializeField] Transform NeturalTarget;
    [SerializeField] Animator animator;
    [SerializeField] SlotsCannonBase cannonBase;
    [SerializeField] GameObject explosion;
    bool active;
    int currHealth;
    // Start is called before the first frame update
    void Start()
    {
        active = false;
        currHealth = health;
        Activate();
    }
    private void Update()
    {
        if (active)
        {
            TurnToPlayer();
        }
        else
        {
            Quaternion rot = Quaternion.LookRotation(NeturalTarget.position - transform.position);
            transform.rotation = Quaternion.Lerp(transform.rotation, rot, Time.deltaTime * trackSpeed);
        }
    }
    void TurnToPlayer()
    {
        Quaternion rot = Quaternion.LookRotation(new Vector3(GameManager.instance.player.transform.position.x, GameManager.instance.player.transform.position.y, GameManager.instance.player.transform.position.z) - transform.position);
        transform.rotation = Quaternion.Lerp(transform.rotation, rot, Time.deltaTime * trackSpeed);
    }

    public void Activate() 
    {
        if(active == true)
        {
            return;
        }
        cannonBase.BringOutCannon();
    }
    public void takeDamage(int count)
    {
        currHealth -= count;
        if(currHealth <= 0) 
        {
            active = false;
            StartCoroutine(BlowUp());
        }
    }

    public void Hide()
    {
        active = false;
        cannonBase.HideCannon();
    }

    IEnumerator BlowUp()
    {
        animator.enabled = true;
        animator.SetTrigger("BlowUpCannon");
        yield return new WaitForSeconds(2);
        animator.SetTrigger("Retreat");
        cannonBase.HideCannon();
    }
    // Update is called once per frame
    public void Respawn(bool reinforced)
    {
        if(reinforced)
        currHealth = health;
        else
        {
            currHealth = health * 2;
        }
        animator.enabled = false;
        active = false;
    }

    
    public void IsOut()
    {
        active = true;
    }
    private void OnTriggerEnter(Collider other)
    {
        //Insert Bomb Script here.
        PickUpItem bomb = other.GetComponent<PickUpItem>();
        if (bomb != null && bomb.PickedUpByPlayer())
        {
            Instantiate(explosion, other.transform.position, transform.rotation.normalized);
            takeDamage(health);
            Destroy(other.gameObject);
        }
    }
}
