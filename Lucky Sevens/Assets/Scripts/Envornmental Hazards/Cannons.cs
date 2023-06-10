using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Cannons : MonoBehaviour, IDamage
{
    [SerializeField] GameObject projectile;
    [SerializeField] float fireRate;
    [SerializeField] int fireCone;
    [Header("-----Stats-----")]
    [SerializeField] bool invincible;
    [SerializeField] int health;
    [SerializeField] float speed;
    [SerializeField] int viewCone;
    [Header("-----Transforms-----")]
    [SerializeField] Transform barrelPosition;
    [Header("- - -Explosion- - -")]
    [SerializeField] GameObject explosion;
    [Header("-----On Death-----")]
    [SerializeField] GameObject[] triggerOnDeath;



    bool isShooting;
    Color colorOrig;
    float currHealth;
    Vector3 origPos;
    Quaternion origRot;
    void Start()
    {
        currHealth = health;
        colorOrig = colorOrig = gameObject.GetComponent<MeshRenderer>().material.color;
        origPos = transform.forward;
        origRot = transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        PlayerInRange();

    }
    void TurnToPlayer()
    {
        Quaternion rot = Quaternion.LookRotation(new Vector3(GameManager.instance.player.transform.position.x, GameManager.instance.player.transform.position.y, GameManager.instance.player.transform.position.z) - transform.position);
        transform.rotation = Quaternion.Lerp(transform.rotation, rot, Time.deltaTime * speed);
    }

    void PlayerInRange()
    {
        Vector3 playerDir = GameManager.instance.player.transform.position - transform.position;
        float angleToPlayer = Vector3.Angle(new Vector3(playerDir.x, 0, playerDir.z), origPos);

        if (angleToPlayer <= fireCone && !isShooting)
        {
            StartCoroutine(Shoot());
        }
        if (angleToPlayer <= viewCone)
        {
            TurnToPlayer();
        }
        else
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, origRot, Time.deltaTime * speed);
        }
        return ;
    }

    IEnumerator Shoot()
    {
        isShooting = true;
        Instantiate(projectile, transform.position, transform.rotation);
        yield return new WaitForSeconds(fireRate);
        isShooting = false;
        Debug.Log("Shoot");
    }

    public void takeDamage(float damage, Transform pos = null)
    {
        if (invincible)
        {
            return;
        }
        currHealth -= damage;
        if (currHealth <= 0)
        {

            Instantiate(explosion, barrelPosition.transform.position, transform.rotation);
            foreach (GameObject targetObject in triggerOnDeath)
            {
                ICannonKey key = targetObject.GetComponent<ICannonKey>();
                if(key != null) {
                    key.OnCannonDeath();
                }
                else
                {
                    Debug.LogWarning("Item in triggerOnDeath in a cannon doesn't have ICannonKey."); //make this more descriptive in the future. 
                }
            }
        }
        else
        {
            StartCoroutine(FlashColor());
        }
    }

    IEnumerator FlashColor()
    {
        gameObject.GetComponent<MeshRenderer>().material.color = Color.red;
        yield return new WaitForSeconds(.1f);
        gameObject.GetComponent<MeshRenderer>().material.color = colorOrig;
    }

    public void instaKill()
    {
        takeDamage(currHealth);
    }
}
