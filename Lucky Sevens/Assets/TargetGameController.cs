using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetGameController : MonoBehaviour
{
    [System.Serializable]
    public class TargetGame 
    {
        [SerializeField] int ID;
        [SerializeField] GameObject[] targets;
        [SerializeField] int ammoReward;
    }

    [SerializeField] TargetGame[] games;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
