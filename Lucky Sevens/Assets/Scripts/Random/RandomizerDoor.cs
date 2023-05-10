using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomizerDoor : MonoBehaviour, IRandomizeAction
{
    [SerializeField] int position;
    [SerializeField] Animator animator;

    public void OnSelect()
    {
        animator.SetTrigger("SetOff");
    }
    public int GetPosition()
    {
        return position;
    }
}
