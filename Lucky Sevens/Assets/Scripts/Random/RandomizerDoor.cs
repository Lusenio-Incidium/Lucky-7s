using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomizerDoor : MonoBehaviour, IRandomizeAction
{
    [SerializeField] int position;
    [SerializeField] int ID;
    [SerializeField] Animator animator;
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip openDoor;
    [Range(0, 1)][SerializeField] float Vol;

    public void OnSelect()
    {
        Vol = GameManager.instance.playerScript.GetJumpVol();
        audioSource.PlayOneShot(openDoor, Vol);
        animator.SetTrigger("SetOff");
    }
    public int GetPosition()
    {
        return position;
    }

    public int GetID()
    {
        return ID;
    }
}
