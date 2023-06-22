using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    [SerializeField] GameObject targetManager;

    TargetGameController controller;
    [SerializeField] int gameNumber;

    private void Start()
    {
        controller = targetManager.GetComponent<TargetGameController>();
    }
    private void OnDestroy()
    {
        if(gameObject != null && controller != null) 
        {
            controller.onTargetDeath(gameObject, gameNumber);
        }
        
    }
}
