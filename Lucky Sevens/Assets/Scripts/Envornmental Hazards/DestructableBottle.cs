using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructableBottle : MonoBehaviour, IButtonTrigger, ICannonKey, IBattle, IDialouge, IDamage, IBottle
{
    private enum Functions 
    {
        None,
        Destroy
    }
    [SerializeField] GameObject debrisSpawn;
    [SerializeField] BoxCollider collisionBox;
    [Header("Sends OnBottleBlowup To")]
    [SerializeField] GameObject[] recievers;
    [Header("Shootable Functions")]
    [SerializeField] bool isShootable;
    //[SerializeField] int heatlh;

    [Header("Trigger Functions")]
    [SerializeField] Functions onButtonPress;
    [SerializeField] Functions onButtonRelease;
    [SerializeField] Functions onBattleBegin;
    [SerializeField] Functions onBattleEnd;
    [SerializeField] Functions onCannonDeath;
    [SerializeField] Functions onDialougeContinue;
    [SerializeField] Functions onBottleBlowup;
    [SerializeField] Functions onTriggerEnter;
    [SerializeField] Functions onTriggerExit;

    private Functions FunctionActions(Functions function)
    {
        switch (function) 
        {
            case Functions.None:
                return Functions.None;
            case Functions.Destroy:
                Instantiate(debrisSpawn, transform.position, debrisSpawn.transform.rotation);
                Debug.Log("Spawning");
                gameObject.GetComponent<Renderer>().enabled = false;
                collisionBox.enabled = false;
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

    public void OnDialougeContinue()
    {
        onDialougeContinue = FunctionActions(onDialougeContinue);
    }

    public void OnBottleBlowup()
    {
        onBottleBlowup = FunctionActions(onBottleBlowup);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
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

    public void instaKill()
    {
        Debug.Log("Tf you do to instakill this thing.");
    }

    public void takeDamage(float damage, Transform position = null)
    {
        if (isShootable)
        {
            Debug.Log("Feature not implemented. Plz check again later thx :3");
        }
    }
}
