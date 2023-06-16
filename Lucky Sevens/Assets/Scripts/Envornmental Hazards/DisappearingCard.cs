using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisappearingCard : MonoBehaviour, IButtonTrigger, IBattle, ICannonKey
{
    private enum Functions 
    {
        None,
        Spawn,
        Despawn
    }

    [SerializeField] GameObject cardObject;
    [SerializeField] GameObject desintigrationParticle;
    [SerializeField] GameObject disappearParicle;
    [SerializeField] Color distructionColor;
    [SerializeField] float disappearSpeed;
    [SerializeField] bool respawn;
    [SerializeField] bool spawnImmediately;
    [SerializeField] float respawnTime;

    [Header("Trigger Functions")]
    [SerializeField] Functions onTriggerEnter;
    [SerializeField] Functions onTriggerExit;
    [SerializeField] Functions onButtonPress;
    [SerializeField] Functions onButtonRelease;
    [SerializeField] Functions onBattleBegin;
    [SerializeField] Functions onBattleEnd;
    [SerializeField] Functions onCannonDeath;
    float currDisappearTime;
    float currRespawnTime;
    GameObject spawnedCard;
    GameObject spawnedDesintParticle;
    Vector3 OrigCardScale;
    Color OrigCardColor;
    bool triggered;
    private void Start()
    {
        if (spawnImmediately)
        {
            ForceRespawn();
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (triggered)
        {
            currDisappearTime += Time.deltaTime;
            if (spawnedDesintParticle == null)
            {
                spawnedDesintParticle = Instantiate(desintigrationParticle, transform.position, transform.rotation, spawnedCard.transform);
                
            }
            spawnedCard.GetComponent<Renderer>().material.color = Color.Lerp(OrigCardColor, distructionColor, currDisappearTime / disappearSpeed);
            spawnedCard.transform.localScale = Vector3.Lerp(OrigCardScale, Vector3.zero, currDisappearTime / disappearSpeed);


            if (currDisappearTime >= disappearSpeed)
            {
                triggered = false;
                Destroy(spawnedDesintParticle);
                Destroy(spawnedCard);
                Instantiate(disappearParicle, transform.position, transform.rotation);
            }
        }
        else if (spawnedCard == null && respawn)
        {
            currRespawnTime += Time.deltaTime;
            if (currRespawnTime >= respawnTime)
            {
                spawnedCard = Instantiate(cardObject, transform.position, transform.rotation, transform);
                currDisappearTime = 0;
                currRespawnTime = 0;
                OrigCardColor = spawnedCard.GetComponent<Renderer>().material.color;
                OrigCardScale = spawnedCard.transform.localScale;
            }

            
        }
    }

    public void ForceRespawn()
    {
        if(spawnedCard != null)
        {
            return;
        }
        currRespawnTime = 0;
        spawnedCard = Instantiate(cardObject, transform.position, transform.rotation, transform);
        spawnedCard.transform.SetParent(gameObject.transform);
        currDisappearTime = 0;
        OrigCardColor = spawnedCard.GetComponent<Renderer>().material.color;
        OrigCardScale = spawnedCard.transform.localScale;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if(onTriggerEnter == Functions.Spawn)
            {
                Debug.LogError("Do NOT use onTriggerEnter to spawn the card unless the hitbox is not attached straight to the card for fairness!");
            }
            onTriggerEnter = FunctionActions(onTriggerEnter);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            onTriggerExit = FunctionActions(onTriggerExit);
        }
    }
    private Functions FunctionActions(Functions function)
    {
        switch (function) 
        {
            case Functions.None: return Functions.None;
            case Functions.Spawn: ForceRespawn(); break;
            case Functions.Despawn: if (spawnedCard != null) { triggered = true; } break;
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
}
