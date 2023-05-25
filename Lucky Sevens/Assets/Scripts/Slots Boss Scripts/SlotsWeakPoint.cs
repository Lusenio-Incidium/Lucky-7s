using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions.Must;

public class SlotsWeakPoint : MonoBehaviour, IDamage
{
    [Header("--- Weak Point Data ---")]
    [SerializeField] int health;
    [SerializeField] int trackSpeed;
    [SerializeField] int VerticalClampMax;
    [SerializeField] int VerticalClampMin;
    [SerializeField] int HorizontalClampMax;
    [SerializeField] int HorizontalClampMin;
    [SerializeField] Transform barrelPosition;
    [SerializeField] Transform NeturalTarget;
    [SerializeField] Animator cannonAnimator;
    [SerializeField] Animator cannonBaseAnimator;
    [SerializeField] GameObject explosion;
    [SerializeField] BoxCollider boxCollider;
    bool active;
    int currHealth;
    // Start is called before the first frame update
    void Start()
    {
        active = false;
        currHealth = health;
        Hide();
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

    public IEnumerator Activate() 
    {
        cannonBaseAnimator.SetBool("Out", true);
        yield return new WaitForSeconds(1);
        active = true;
        boxCollider.enabled = true;    }
    public void Hide()
    {
        active = false;
        boxCollider.enabled = false;
        cannonBaseAnimator.SetBool("Out", false);
    }
    public void StartMoving()
    {
        cannonBaseAnimator.SetBool("Moving", true);
    }

    public void takeDamage(int count)
    {
        currHealth -= count;
        if(currHealth <= 0) 
        {
            boxCollider.enabled = false;
            active = false;
            StartCoroutine(BlowUp());
            StartCoroutine(SlotsController.instance.StunWheel());
            
        }
    }

    IEnumerator BlowUp()
    {
        boxCollider.enabled = false;
        cannonAnimator.enabled = true;
        cannonAnimator.SetTrigger("BlowUpCannon");
        Instantiate(explosion, barrelPosition.transform.position, transform.rotation);
        yield return new WaitForSeconds(2);
        cannonBaseAnimator.SetBool("Out", false);
        cannonAnimator.SetTrigger("Retreat");

    }

    public void Respawn(bool reinforced)
    {
        if(!reinforced)
        currHealth = health;
        else
        {
            currHealth = health * 2;
        }
        cannonAnimator.enabled = false;
        StartCoroutine(Activate());
    }
    public int GetHealth()
    {
        return currHealth;
    }

    public bool GetActive()
    {
        return active;
    }

    public Transform giveBarrel()
    {
        return barrelPosition;
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
