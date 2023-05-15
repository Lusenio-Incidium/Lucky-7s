using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour,IDamage,IStatusEffect
{
    [Header("----- Components -----")]
    [SerializeField] Renderer model;
    [SerializeField] NavMeshAgent agent;
    [SerializeField] Transform headPos;
    [SerializeField] Transform shootPos;

    [Header("----- Enemy Stats -----")]
    [SerializeField] int HP;
    [SerializeField] float viewAngle;
    [SerializeField] int playerFaceSpeed;

    [Header("----- EnemyWeapons -----")]
    [SerializeField] float shootSpeed;
    [SerializeField] int range;
    [SerializeField] float attackAngle;
    [SerializeField] GameObject gunProjectile;

    bool isShooting;
    [SerializeField] StatusEffectObj hitEffect;
    private float timePassed = 0;
    private float OrigSpeed;
    Color colorOrig;
    Vector3 playerDir;
    float angleOfPlayer;
    bool playerInRange;
    bool destination;
    // Start is called before the first frame update
    void Start()
    {
        colorOrig = model.material.color;
        OrigSpeed = agent.speed;
        GameManager.instance.UpdateEnemyCount(1);
    }

    // Update is called once per frame
    void Update()
    {
        if (destination)
            FacePlayer();

        if(playerInRange && CanSeePlayer())
        {

        }
    }

    bool CanSeePlayer()
    {
        playerDir = GameManager.instance.player.transform.position - headPos.position;
        angleOfPlayer = Vector3.Angle(new Vector3(playerDir.x, 0, playerDir.z), transform.forward);

        Debug.DrawRay(headPos.position, playerDir);
        Debug.Log(angleOfPlayer);

        RaycastHit hit;

        if(Physics.Raycast(headPos.position,playerDir,out hit))
        {
            if(hit.collider.CompareTag("Player") && angleOfPlayer <= viewAngle)
            {
                agent.SetDestination(GameManager.instance.player.transform.position);
                if(agent.remainingDistance <= agent.stoppingDistance)
                {
                    destination = true;
                    FacePlayer();
                }
                if(!isShooting && angleOfPlayer <= attackAngle)
                {
                    StartCoroutine(Shoot());
                }
                return true;
            }
        }
        return false;
    }


    IEnumerator Shoot()
    {
        isShooting = true;

        Instantiate(gunProjectile, shootPos.position, transform.rotation);

        yield return new WaitForSeconds(shootSpeed);

        isShooting = false;
    }


    public void ApplyStatusEffect(StatusEffectObj data)
    {
        if (data != null && hitEffect == null)
        {
            hitEffect = data;
            StartCoroutine(BurnEffect());
        }
    }

    public IEnumerator BurnEffect()
    {
        if (hitEffect.duration != 0)
        {
            if (hitEffect.slowEffect != 0)
            {
                agent.speed /= hitEffect.slowEffect;
            }
            timePassed = Time.time;
            while (Time.time - timePassed <= hitEffect.duration)
            {
                if (hitEffect.damage != 0)
                {
                    yield return new WaitForSeconds(hitEffect.damagespeed);
                    takeDamage(hitEffect.damage);
                }
                else
                {
                    yield return new WaitForSeconds(hitEffect.duration);
                    RemoveEffect();
                }
            }
            if (Time.time - timePassed >= hitEffect.duration)
            {
                RemoveEffect();
            }
        }
    }

    public void RemoveEffect()
    {
        hitEffect = null;
        timePassed = 0;
        agent.speed = OrigSpeed;
    }

    public void takeDamage(int dmg)
    {
        HP -= dmg;
        StartCoroutine(FlashColor());
        destination = true;

        if(HP <= 0)
        {
            Destroy(gameObject);
            GameManager.instance.UpdateEnemyCount(-1);
        }
    }
    IEnumerator FlashColor()
    {
        model.material.color = Color.red;
        yield return new WaitForSeconds(.1f);
        model.material.color = colorOrig;
    }
    public void FacePlayer()
    {
        Quaternion rot = Quaternion.LookRotation(new Vector3(playerDir.x, 0, playerDir.z));
        transform.rotation = Quaternion.Lerp(transform.rotation, rot, Time.deltaTime * playerFaceSpeed);
    }
    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            playerInRange = true;
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            destination = false;
        }
    }
}
