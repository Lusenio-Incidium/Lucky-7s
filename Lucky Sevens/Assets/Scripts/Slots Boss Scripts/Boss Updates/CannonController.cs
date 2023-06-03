using System.Collections;
using UnityEngine;

public class CannonController : MonoBehaviour, IDamage
{
    [Header("- - -Components- - -")]
    [SerializeField] Animator cannonAnimator;
    [SerializeField] Animator cannonBaseAnimator;
    [SerializeField] BoxCollider boxCollider;
    [Header("- - -Stats- - -")]
    [SerializeField] float health;
    [SerializeField] float speed;
    [Header("- - -Transforms- - -")]
    [SerializeField] Transform barrelPosition;
    [Header("- - -Explosion- - -")]
    [SerializeField] GameObject explosion;

    bool isActive;
    Color colorOrig;
    float currHealth;

    private void Start()
    {
        currHealth = health;
        colorOrig = colorOrig = gameObject.GetComponent<MeshRenderer>().material.color;
        Hide();
    }

    private void Update()
    {
        if (isActive)
        {
            TurnToPlayer();
        }
        else
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.identity, Time.deltaTime * speed);
        }
    }

    void TurnToPlayer()
    {
        Quaternion rot = Quaternion.LookRotation(new Vector3(GameManager.instance.player.transform.position.x, GameManager.instance.player.transform.position.y, GameManager.instance.player.transform.position.z) - transform.position);
        transform.rotation = Quaternion.Lerp(transform.rotation, rot, Time.deltaTime * speed);
    }

    public void Hide()
    {
        isActive = false;
        boxCollider.enabled = false;
        cannonBaseAnimator.SetBool("Out", false);
    }

    public void Respawn(bool reinforced)
    {
        if (reinforced)
            currHealth = health * 2;
        else
            currHealth = health;
        
        cannonAnimator.enabled = false;
        StartCoroutine(Activate());
    }

    public void takeDamage(float count)
    {
        currHealth -= count;
        if (currHealth <= 0)
        {
            boxCollider.enabled = false;
            isActive = false;
            StartCoroutine(BlowUp());

        }
        else
        {
            StartCoroutine(FlashColor());
        }
    }

    public void StartMoving()
    {
        cannonBaseAnimator.SetBool("Moving", true);
    }

    public float GetHealth()
    {
        return currHealth;
    }

    public bool isCannonActive() 
    {
        return isActive;
    }

    IEnumerator FlashColor()
    {
        gameObject.GetComponent<MeshRenderer>().material.color = Color.red;
        yield return new WaitForSeconds(.1f);
        gameObject.GetComponent<MeshRenderer>().material.color = colorOrig;
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
    public IEnumerator Activate()
    {
        cannonBaseAnimator.SetBool("Out", true);
        yield return new WaitForSeconds(1);
        isActive = true;
        boxCollider.enabled = true;
    }
}
