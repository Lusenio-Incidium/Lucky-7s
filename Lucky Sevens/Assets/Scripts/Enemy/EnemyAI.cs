using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour,IDamage,IStatusEffect,IPhysics,IBattleEnemy
{
    [Header("----- Components -----")]
    [SerializeField] Renderer model;
    [SerializeField] NavMeshAgent agent;
    [SerializeField] Animator anim;
    [SerializeField] Transform headPos;
    [SerializeField] Transform shootPos;
    [SerializeField] public bool Complicated;
    [SerializeField] public bool isMelee;
    [SerializeField] public bool healer;
    [SerializeField] Collider enemyCol;

    [Header("----- Enemy Stats -----")]
    [SerializeField] float HP;
    [SerializeField] float viewAngle;
    [SerializeField] int playerFaceSpeed;
    [SerializeField] int roamDistance;
    [SerializeField] int roamPauseTime;
    [SerializeField] int animTransSpeed;
    [SerializeField] int timeToHeal;
    [SerializeField] int healAmount;
    [SerializeField] GameObject itemDropped;
    [SerializeField] GameObject particleShown;

    [Header("----- EnemyWeapons -----")]
    [SerializeField] float shootSpeed;
    [SerializeField] int range;
    [SerializeField] float attackAngle;
    [SerializeField] GameObject gunProjectile;
    [SerializeField] Collider leftFistCol = null;
    [SerializeField] Collider rightFistCol = null;
    [SerializeField] int bulletSpeed;


    bool isShooting;
    [SerializeField] StatusEffectObj hitEffect;
    private float timePassed = 0;
    private float OrigSpeed;
    Color colorOrig;
    Vector3 playerDir;
    float angleOfPlayer;
    bool playerInRange;
    Vector3 startingPos;
    bool destinationChosen;
    float stoppingDistanceOrig;
    float speed;
    GameObject tempParticle;
    float shootSpeedOrig;
    float HPOrig;
    Vector3 ranPos;
    Vector3 pushBack;

    BattleManager battleManager;
    void Start()
    {
        colorOrig = model.material.color;
        OrigSpeed = agent.speed;
        GameManager.instance.UpdateEnemyCount(1);
        startingPos = transform.position;
        stoppingDistanceOrig = agent.stoppingDistance;
        agent.radius = Random.Range(.5f, .75f);
        shootSpeedOrig = shootSpeed;
        enemyCol = GetComponent<CapsuleCollider>();
        HPOrig = HP;
    }
    void OnEnable()
    {
        if (GameManager.instance != null)
        {
            if (GameManager.instance.hard)
            {
                HP *= 1.5f;
                Complicated = false;
            }
            else if (GameManager.instance.easy)
            {
                HP *= .5f;
                Complicated = true;
            }
            if (HP <= 0)
            {
                HP = HPOrig;
            }
            Start();
            agent.enabled = true;
            enemyCol.enabled = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        //Simplified the Update function, using only one if(agent.isActiveAndEnabled) statment
        if (agent.isActiveAndEnabled) 
        {
            if (Complicated)
                complicated();
            else
                simple();
        }
    }
    //Everything for a complicated enemy (Roaming, view cones, etc.
    void complicated() 
    {
            speed = Mathf.Lerp(speed, agent.velocity.normalized.magnitude, Time.deltaTime * animTransSpeed);
            anim.SetFloat("Speed", speed);

            if (agent.destination == GameManager.instance.player.transform.position)
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
    //Simple enemy AI, Enemies can always see player, always go to player, etc. No roaming.
    void simple() 
    {
        Debug.Log("Simple");
            speed = Mathf.Lerp(speed, agent.velocity.normalized.magnitude, Time.deltaTime * animTransSpeed);
            anim.SetFloat("Speed", speed);
            if (!healer)
            {
                if (agent.isActiveAndEnabled)
                {
                    agent.SetDestination(GameManager.instance.player.transform.position);
                    CanSeePlayer();
                }
            }
            else
            {
                if (playerInRange && agent.isActiveAndEnabled)
                {
                    RunAwayFromPlayer();
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
    void RunAwayFromPlayer()
    {
        if (agent.velocity == Vector3.zero && Vector3.Distance(GameManager.instance.player.transform.position,transform.position) <= 10)
        {
            ranPos = new Vector3(transform.position.x + Random.Range(-10, 10), transform.position.y + Random.Range(-10, 10), transform.position.z + Random.Range(-10, 10));
            while (Vector3.Distance(GameManager.instance.player.transform.position, ranPos) <= 10)
            {
                ranPos = new Vector3(transform.position.x + Random.Range(-10, 10), transform.position.y + Random.Range(-10  , 10), transform.position.z + Random.Range(-10, 10));
            }
            NavMeshHit hit;
            NavMesh.SamplePosition(ranPos, out hit, roamDistance, 1);
            agent.SetDestination(hit.position);
        }
    }
    bool CanSeePlayer()
    {
        playerDir = GameManager.instance.playerScript.GetPlayerCenter().position - headPos.position;
        //Debug.DrawRay(headPos.position,playerDir);
        angleOfPlayer = Vector3.Angle(new Vector3(playerDir.x, playerDir.y, playerDir.z), transform.forward);
        RaycastHit hit;

        if(Physics.Raycast(headPos.position,playerDir,out hit))
        {
            if(hit.collider.CompareTag("Player") && angleOfPlayer <= viewAngle)
            {
                //StopCoroutine(Roam());
                agent.stoppingDistance = stoppingDistanceOrig;
                if (healer)
                {
                    RunAwayFromPlayer();
                    
                }
                else
                {
                    agent.SetDestination(GameManager.instance.player.transform.position);
                    if (agent.remainingDistance <= agent.stoppingDistance)
                    {
                        FacePlayer();
                    }
                    if (!isShooting && angleOfPlayer <= attackAngle && agent.remainingDistance <= range && !healer)
                    {
                        StartCoroutine(Shoot());
                    }
                }
                return true;
            }
        }
        agent.stoppingDistance = stoppingDistanceOrig;
        return false;
    }


    IEnumerator Shoot()
    {
        isShooting = true;
        anim.SetTrigger("Shoot");
        //shootPos.rotation = Quaternion.Euler(playerDir.x,playerDir.y,playerDir.z); 
        if (isMelee)
        {
            anim.SetInteger("Melee", Random.Range(0, 10));
        }

        yield return new WaitForSeconds(shootSpeed);
       // shootPos.rotation = Quaternion.identity;
        isShooting = false;
    }

    public void createBullet()
    {
        shootPos.LookAt(GameManager.instance.playerScript.GetPlayerCenter().position);
        gunProjectile.GetComponent<Bullet>().SetBulletSpeed(bulletSpeed);
        ObjectPoolManager.instance.SpawnObject(gunProjectile, shootPos.transform.position,shootPos.transform.rotation, ObjectPoolManager.PoolType.GameObject);
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
        if (hitEffect.particleEffect != null)
        {
            tempParticle = Instantiate(hitEffect.particleEffect, transform.position, Quaternion.Euler(270, 0, 0));
            tempParticle.transform.parent = transform;
        }
        if (hitEffect.duration != 0)
        {
            if (hitEffect.slowEffect != 0)
            {
                agent.speed /= hitEffect.slowEffect;
                shootSpeed *= hitEffect.slowEffect;
            }
            timePassed = Time.deltaTime;
            while ( Time.deltaTime - timePassed <= effectTime && hitEffect != null)
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
        StopCoroutine(BurnEffect());
        Destroy(tempParticle);
        hitEffect = null;
        timePassed = 0;
        agent.speed = OrigSpeed;
        shootSpeed = shootSpeedOrig;
    }
    public void takeDamage(float dmg, Transform pos = null)
    {
        if(HP == HPOrig && dmg < 0)
        {
            return;
        }
        HP -= dmg;
        if (dmg < 0)
        {
            return;
        }
        //AddPushBack();
        if (HP <= 0)
        {
            anim.SetBool("Dead", true);
            GameManager.instance.UpdateEnemyCount(-1);
            StopAllCoroutines();
            agent.enabled = false;
            fistColOff();
            enemyCol.enabled = false;
            if (itemDropped != null)
            {
                ObjectPoolManager.instance.SpawnObject(itemDropped, transform.position + new Vector3(0, 1, 0), gameObject.transform.rotation);
            }
            if (battleManager != null)
            {
                battleManager.DeclareDeath(1);
            }
            StartCoroutine(WaitSecondsBeforeDespawn());
            GameManager.instance.enemiesKilled++;
        }
        else 
        {
            StartCoroutine(FlashColor());
            anim.SetTrigger("Damage");
            agent.SetDestination(GameManager.instance.player.transform.position);
            playerInRange = true;
        }
            
            
           
    }

    public void instaKill()
    {
        takeDamage(HP);
    }

    public void SetBattleManager(BattleManager manager)
    {
        battleManager = manager;
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
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            agent.stoppingDistance = 0;
            playerInRange = false;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Player")) 
        {
            GameManager.instance.playerScript.TakePush(new Vector3(1, 0, 1));
            GameManager.instance.playerScript.setControl(false);
        }
            
    }
    
    IEnumerator giveBackControl() 
    {
        yield return new WaitForSeconds(0.5f);
        GameManager.instance.playerScript.setControl(true);
    }

    void AddPushBack()
    {
        if (agent.enabled)
        {
            agent.Move((agent.velocity + pushBack) * Time.deltaTime *.5f);
            pushBack = Vector3.Lerp(pushBack, Vector3.zero, Time.deltaTime);
        }
    }
    public float GetEnemyHP()
    {
        return HP;
    }
    public float GetOrigEnemyHP()
    {
        return HPOrig;
    }    
    public float GetHealCoolDown()
    {
        return timeToHeal;
    }  
    public float GetHealAmount()
    {
        return healAmount * -1;
    }    
    public NavMeshAgent GetAgent()
    {
        return agent;
    }
    public void TakePush(Vector3 dir)
    {
        pushBack += dir;
        AddPushBack();
    }
    private IEnumerator WaitSecondsBeforeDespawn()
    {
        float passedTime = 0f;
        while (passedTime < 8)
        {
            passedTime += Time.deltaTime;
            yield return null;
        }
        ObjectPoolManager.instance.ReturnObjToInfo(gameObject);
        Instantiate(particleShown, transform.position,transform.rotation);
    }
    public Vector3 GetEnemyHeadHeight()
    {
       GameObject pop = transform.GetChild(5).transform.GetChild(0).transform.GetChild(2).transform.GetChild(0).transform.GetChild(1).transform.GetChild(0).gameObject;
       return pop.transform.position;
    }
}
