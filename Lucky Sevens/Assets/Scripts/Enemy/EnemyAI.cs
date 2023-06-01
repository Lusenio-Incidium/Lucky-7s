using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour,IDamage,IStatusEffect
{
    [Header("----- Components -----")]
    [SerializeField] Renderer model;
    [SerializeField] NavMeshAgent agent;
    [SerializeField] Animator anim;
    [SerializeField] Transform headPos;
    [SerializeField] Transform shootPos;
    [SerializeField] public bool Complicated;
    [SerializeField] public bool isMelee;

    [Header("----- Enemy Stats -----")]
    [SerializeField] float HP;
    [SerializeField] float viewAngle;
    [SerializeField] int playerFaceSpeed;
    [SerializeField] int roamDistance;
    [SerializeField] int roamPauseTime;
    [SerializeField] int animTransSpeed;

    [Header("----- EnemyWeapons -----")]
    [SerializeField] float shootSpeed;
    [SerializeField] int range;
    [SerializeField] float attackAngle;
    [SerializeField] GameObject gunProjectile;
    [SerializeField] Collider leftFistCol;
    [SerializeField] Collider rightFistCol;


    bool isShooting;
    [SerializeField] StatusEffectObj hitEffect;
    private float timePassed = 0;
    private float OrigSpeed;
    Color colorOrig;
    Vector3 playerDir;
    float angleOfPlayer;
    bool playerInRange;
    bool destination;
    Vector3 startingPos;
    bool destinationChosen;
    float stoppingDistanceOrig;
    float speed;
    void Start()
    {
        colorOrig = model.material.color;
        OrigSpeed = agent.speed;
        GameManager.instance.UpdateEnemyCount(1);
        startingPos = transform.position;
        stoppingDistanceOrig = agent.stoppingDistance;
    }

    // Update is called once per frame
    void Update()
    {
        if (Complicated)
        {
            if (agent.isActiveAndEnabled)
            {
                speed = Mathf.Lerp(speed, agent.velocity.normalized.magnitude, Time.deltaTime * animTransSpeed);
                anim.SetFloat("Speed", speed);

                if (destination)
                    FacePlayer();

                if (playerInRange && !CanSeePlayer())
                {
                    StartCoroutine(Roam());
                }
                else if (agent.destination != GameManager.instance.player.transform.position)
                {
                    StartCoroutine(Roam());
                }
            }
        }
        else
        {
            if (agent.isActiveAndEnabled)
            {
                speed = Mathf.Lerp(speed, agent.velocity.normalized.magnitude, Time.deltaTime * animTransSpeed);
                anim.SetFloat("Speed", speed);
                agent.SetDestination(GameManager.instance.player.transform.position);
                CanSeePlayer();
            }
        }
    }
    IEnumerator Roam()
    {
        if(!destinationChosen && agent.remainingDistance <0.05f)
        {
            destinationChosen = true;
            agent.stoppingDistance = 0;
            yield return new WaitForSeconds(roamPauseTime);
            destinationChosen = false;

            Vector3 ranPos = Random.insideUnitSphere * roamDistance;
            ranPos += startingPos;

            NavMeshHit hit;
            NavMesh.SamplePosition(ranPos, out hit, roamDistance,1);

            agent.SetDestination(hit.position);
        }
    }
    bool CanSeePlayer()
    {
        playerDir = GameManager.instance.player.transform.position - headPos.position;
        angleOfPlayer = Vector3.Angle(new Vector3(playerDir.x, 0, playerDir.z), transform.forward);

        RaycastHit hit;

        if(Physics.Raycast(headPos.position,playerDir,out hit))
        {
            if(hit.collider.CompareTag("Player") && angleOfPlayer <= viewAngle)
            {
                agent.SetDestination(GameManager.instance.player.transform.position);
                agent.stoppingDistance = stoppingDistanceOrig;
                if(agent.remainingDistance <= agent.stoppingDistance)
                {
                    if (Complicated)
                    {
                        destination = true;
                        FacePlayer();
                    }
                }
                if(!isShooting && angleOfPlayer <= attackAngle && agent.remainingDistance <= range)
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

        anim.SetTrigger("Shoot");

        if (isMelee)
        {
            anim.SetInteger("Melee", Random.Range(0, 10));
        }

        yield return new WaitForSeconds(shootSpeed);

        isShooting = false;
    }

    public void createBullet()
    {
        Instantiate(gunProjectile, shootPos.position, transform.rotation);
    }

    public void fistColOn()
    {
        leftFistCol.enabled = true;
        rightFistCol.enabled = true;
    }

    public void fistColOff()
    {
        leftFistCol.enabled = false;
        rightFistCol.enabled = false;
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
        int effectTime = hitEffect.duration;
        if (hitEffect.duration != 0)
        {
            if (hitEffect.slowEffect != 0)
            {
                agent.speed /= hitEffect.slowEffect;
            }
            timePassed = Time.time;
            while ( Time.time - timePassed <= effectTime && hitEffect != null)
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
            if (hitEffect != null)
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

    public void takeDamage(float dmg)
    {
        HP -= dmg;
        if(HP <= 0)
        {
            anim.SetBool("Dead", true);
            GameManager.instance.UpdateEnemyCount(-1);
            agent.enabled = false;
            fistColOff();
            Destroy(gameObject, 8);
        }
        StartCoroutine(FlashColor());
        destination = true;
        anim.SetTrigger("Damage");
    }

    IEnumerator FlashColor()
    {
        model.material.color = Color.red;
        yield return new WaitForSeconds(.1f);
        model.material.color = colorOrig;
    }
    public void FacePlayer()
    {
        playerDir = GameManager.instance.player.transform.position - headPos.position;
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
