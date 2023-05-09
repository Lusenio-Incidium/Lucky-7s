using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour,IDamage,IStatusEffect
{
    [Header("----- Components -----")]
    [SerializeField] Renderer model;
    [SerializeField] NavMeshAgent agent;

    [Header("----- Enemy Stats -----")]
    [SerializeField] int HP;
    [SerializeField] int enemySpeed;

    [Header("----- EnemyWeapons -----")]
    [SerializeField] int damage;
    [SerializeField] float shootSpeed;
    [SerializeField] int range;
    [SerializeField] GameObject gunProjectile;

    bool isShooting = true;
    [SerializeField] StatusEffectObj hitEffect;
    private float timePassed = 0;
    private float OrigSpeed;
    Color colorOrig;
    // Start is called before the first frame update
    void Start()
    {
        colorOrig = model.material.color;
        OrigSpeed = agent.speed;
    }

    // Update is called once per frame
    void Update()
    {
        agent.SetDestination(GameManager.instance.player.transform.position);
        if (!isShooting)
            StartCoroutine(Shoot());
    }

    IEnumerator Shoot()
    {
        isShooting = true;

        Instantiate(gunProjectile, transform.position, transform.rotation);

        yield return new WaitForSeconds(shootSpeed);

        isShooting = false;
    }


    public void ApplyStatusEffect(StatusEffectObj data)
    {
        hitEffect = data;
        StartCoroutine(BurnEffect());

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

        if(HP <= 0)
        {
            Destroy(gameObject);
        }
    }
    IEnumerator FlashColor()
    {
        model.material.color = Color.red;
        yield return new WaitForSeconds(.1f);
        model.material.color = colorOrig;
    }
}
