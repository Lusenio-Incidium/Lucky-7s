using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomizerDoor : MonoBehaviour, IRandomizeAction
{
    [SerializeField] int position;

    public void OnSelect()
    {
        gameObject.GetComponent<Animator>().enabled = true;
    }
    public int GetPosition()
    {
        return position;
    }
}
