using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetManager : MonoBehaviour
{
    public static TargetManager instance;
    int numOfTargets;
    [SerializeField] GameObject spawnable;
    private void Awake()
    {
        instance = this;
    }

    public void updateTargets(int update) 
    {
        numOfTargets += update;

        if(numOfTargets <= 0) 
        {
            spawnable.SetActive(true);
        }
    }
}
