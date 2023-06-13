using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwingingAxe : MonoBehaviour, IButtonTrigger, IBattle, ICannonKey
{
    private enum Functions
    {
        None,
        StartSwingingOnce,
        StartSwinging,
        StopAtNeturalOnce,
        StopAtNetural,
        StopInstantlyOnce,
        StopInstantly,

    }
    [SerializeField] Animator animator;
    [SerializeField] BoxCollider damageHitBox;
    [SerializeField] AudioSource noiseMaker;
    [Header("--- Audio ---")]
    [SerializeField] AudioClip[] woosh;
    [Range(0,1)][SerializeField] float volume;
    [Header("--- Chop Settings ---")]    
    [SerializeField] float speedMod;
    [SerializeField] bool startSwinging;
    [SerializeField] int damage;
    [SerializeField] int knockback;
    [Header("Trigger Functions")]
    [SerializeField] Functions onButtonPress;
    [SerializeField] Functions onButtonRelease;
    [SerializeField] Functions onBattleBegin;
    [SerializeField] Functions onBattleEnd;
    [SerializeField] Functions onCannonDeath;
    bool active;
    private void Start()
    {
        animator.speed = speedMod;
        if (startSwinging)
        {
            BeginSwinging();
        }
    }
    public void BeginSwinging()
    {
        animator.enabled = true;
        animator.SetBool("Swing", true);
        active = true;
        damageHitBox.enabled = true;
    }
    public void StopSwinging()
    {
        animator.SetBool("Swing", false);
        damageHitBox.enabled = false;
        active = false;
    }
    public void StopSwingingInstant()
    {
        animator.enabled = false;
        damageHitBox.enabled = false;
        active = false;
    }


    private void OnTriggerEnter(Collider other)
    {
        IDamage damageTarg = other.GetComponent<IDamage>();
        if (damageTarg != null)
        {
            damageTarg.takeDamage(damage);
            Debug.LogWarning(gameObject.name + " hit " + other.name);
        }
        IPhysics physics = other.GetComponent<IPhysics>();
        if (physics != null)
        {
            Vector3 dir = other.gameObject.transform.position - transform.position;
            physics.TakePush(dir * knockback);
        }
    }

    private Functions FunctionActions(Functions function)
    {
        switch (function) {
            case Functions.None:
                break;
            case Functions.StartSwinging:
                BeginSwinging();
                break;
            case Functions.StopAtNetural:
                StopSwinging();
                break;
            case Functions.StopInstantly:
                StopSwingingInstant(); 
                break;
            case Functions.StopAtNeturalOnce:
                StopSwinging();
                function = Functions.None;
                break;
            case Functions.StopInstantlyOnce:
                StopSwingingInstant();
                function = Functions.None;
                break;
        }

        return function;
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

    public void PlaySwoosh()
    {
        if (!active)
        {
            return;
        }
        noiseMaker.PlayOneShot(woosh[Random.Range(0, woosh.Length)], volume);
    }
}
