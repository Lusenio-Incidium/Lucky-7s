using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Cannons : MonoBehaviour, IDamage, ICannonKey, IBattle, IButtonTrigger
{
    private enum Functions
    {
        None,
        Activate,
        Deactivate,
        InstaKill
    }
    [SerializeField] GameObject projectile;
    [SerializeField] Animator animator;
    [SerializeField] float fireRate;
    [SerializeField] int fireCone;
    [Header("-----Stats-----")]
    [SerializeField] bool invincible;
    [SerializeField] int health;
    [SerializeField] float speed;
    [SerializeField] int viewCone;
    [SerializeField] bool active;
    [Header("-----Transforms-----")]
    [SerializeField] Transform barrelPosition;
    [Header("- - -Explosion- - -")]
    [SerializeField] GameObject explosion;
    [Header("-----On Death-----")]
    [SerializeField] GameObject[] triggerOnDeath;
    [Header("Trigger Functions")]
    [SerializeField] Functions onButtonPress;
    [SerializeField] Functions onButtonRelease;
    [SerializeField] Functions onBattleBegin;
    [SerializeField] Functions onBattleEnd;
    [SerializeField] Functions onCannonDeath;
    [Header("--- Audio ---")]
    [SerializeField] AudioSource noiser;
    [SerializeField] AudioClip[] boom;
    [Range(0, 1)][SerializeField] float boomVol;
    [SerializeField] AudioClip[] deathNoise;
    [Range(0, 1)][SerializeField] float deathVol;

    bool isShooting;
    bool inRange;
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
        if (inRange)
        {
            PlayerInRange();
        }
        else
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, origRot, Time.deltaTime * speed);
        }
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

        if (angleToPlayer <= fireCone && !isShooting && active)
        {
            StartCoroutine(Shoot());
        }
        if (angleToPlayer <= viewCone && active)
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
        boomVol  = GameManager.instance.playerScript.GetJumpVol();
        isShooting = true;
        noiser.PlayOneShot(boom[Random.Range(0, boom.Length)], boomVol);
        Instantiate(projectile, transform.position, transform.rotation);
        yield return new WaitForSeconds(fireRate);
        isShooting = false;
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
            deathVol = GameManager.instance.playerScript.GetJumpVol();
            animator.enabled = true;
            Instantiate(explosion, barrelPosition.transform.position, transform.rotation);
            noiser.PlayOneShot(deathNoise[Random.Range(0, deathNoise.Length)], deathVol);
            foreach (GameObject targetObject in triggerOnDeath)
            {
                ICannonKey key = targetObject.GetComponent<ICannonKey>();
                if(key != null) {
                    key.OnCannonDeath();
                }
                else
                {
                    Debug.Log("err");
                }
            }
            invincible = true;
            active = false;
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

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            inRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            inRange = false;
        }
    }

    private Functions FunctionActions(Functions functions)
    {
        switch (functions)
        {
            case Functions.None:
                break;
            case Functions.Activate:
                active = true;
                break;
            case Functions.Deactivate: 
                active = false;
                break;
            case Functions.InstaKill:
                instaKill();
                break;
        }

        return functions;
    }

    public void OnButtonPress()
    {
        onButtonPress = FunctionActions(onButtonPress);
    }
    public void OnButtonRelease()
    {
        onButtonRelease = FunctionActions(onButtonRelease);
    }

    public void OnBattleBegin()
    {
        onBattleBegin = FunctionActions(onBattleBegin);
    }

    public void OnBattleEnd()
    {
        onBattleEnd = FunctionActions(onBattleEnd);
    }

    public void OnCannonDeath()
    {
        onCannonDeath = FunctionActions(onCannonDeath);
    }
}
