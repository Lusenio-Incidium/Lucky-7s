using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelStart : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] AudioClip levelMusic;
    void Awake()
    {
        if(GameManager.instance != null) 
        {
            GameManager.instance.refreshGameManager();
        }
    }

    private void Start()
    {
        GameManager.instance.playerScript.SetMusic(levelMusic);
        GameManager.instance.playerAmmo = GameManager.instance.playerAmmoOrign;
    }
}
