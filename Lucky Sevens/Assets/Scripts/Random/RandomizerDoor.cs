using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomizerDoor : MonoBehaviour, IRandomizeAction
{
    [SerializeField] int position;
    [SerializeField] int ID;
    [SerializeField] Animator animator;

    public void OnSelect()
    {
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
