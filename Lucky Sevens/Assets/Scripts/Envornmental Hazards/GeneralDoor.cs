using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneralDoor : MonoBehaviour, IButtonTrigger, ICannonKey, IBattle
{
    private enum Functions {
        None,
        OpenDoor,
        CloseDoor,
        OpenDoorOnce,
        CloseDoorOnce
    }

    [SerializeField] Animator animator;
    [Header("Trigger Functions")]
    [SerializeField] Functions onButtonPress;
    [SerializeField] Functions onButtonRelease;
    [SerializeField] Functions onBattleBegin;
    [SerializeField] Functions onBattleEnd;
    [SerializeField] Functions onCannonDeath;
    [SerializeField] Functions onTriggerEnter;
    [SerializeField] Functions onTriggerExit;

    private Functions FunctionAction(Functions function)
    {
        switch (function) 
        {
            case Functions.None:
                break;
            case Functions.OpenDoor:
                animator.SetBool("Open", true);
                break;
            case Functions.CloseDoor:
                animator.SetBool("Open", false);
                break;
            case Functions.OpenDoorOnce:
                animator.SetBool("Open", true);
                function = Functions.None;
                break;
            case Functions.CloseDoorOnce:
                animator.SetBool("Open", false);
                function = Functions.None;
                break;
            default:
                Debug.LogError("Bing da boop, sumthin went wrong idk what but if you're seeing this you screwed up sumthin major in GeneralDoor, and the enumerator on one of the options got set to something impossible. :D");
                return Functions.None;
        }

        return function;
    }
    public void OnButtonPress()
    {
        onButtonPress = FunctionAction(onButtonPress);
    }
    public void OnButtonRelease()
    {
        onButtonRelease = FunctionAction(onButtonRelease);

    }
    public void OnCannonDeath()
    {
        onCannonDeath = FunctionAction(onCannonDeath);
    }

    public void OnBattleBegin()
    {
        onBattleBegin = FunctionAction(onBattleBegin);
    }

    public void OnBattleEnd()
    {
        onBattleEnd = FunctionAction(onBattleEnd);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            onTriggerEnter = FunctionAction(onTriggerEnter);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            onTriggerExit = FunctionAction(onTriggerExit);
        }
    }
}
