using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelStart : MonoBehaviour
{
    // Start is called before the first frame update
    void Awake()
    {
        if(GameManager.instance != null) 
        {
            GameManager.instance.refreshGameManager();
        }
    }

    private void Start()
    {
        GameManager.instance.playerScript.updatePlayerUI();
    }
}
