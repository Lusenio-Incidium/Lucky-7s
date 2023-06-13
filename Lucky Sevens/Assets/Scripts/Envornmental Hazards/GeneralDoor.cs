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
    [SerializeField] AudioSource doorAudio;
    [SerializeField] AudioClip doorOpen;
    [SerializeField] AudioClip doorClose;
    [Range(0,1)][SerializeField] float doorVol;
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
        doorVol = GameManager.instance.playerScript.GetJumpVol();   
        switch (function) 
        {
            case Functions.None:
                break;
            case Functions.OpenDoor:
                doorAudio.PlayOneShot(doorOpen, doorVol);
                animator.SetBool("Open", true);
                break;
            case Functions.CloseDoor:
                animator.SetBool("Open", false);
                break;
            case Functions.OpenDoorOnce:
                doorAudio.PlayOneShot(doorOpen, doorVol);
                animator.SetBool("Open", true);
                function = Functions.None;
                break;
            case Functions.CloseDoorOnce:
                animator.SetBool("Open", false);
                function = Functions.None;
                break;
  
        }

        return function;
    }

    public void openDoor() 
    {
        animator.SetBool("Open", true);
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

    public  void PlayCloseDoor()
    {
        doorAudio.PlayOneShot(doorClose, doorVol);
    }
}
