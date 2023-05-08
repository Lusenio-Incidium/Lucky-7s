using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("----- Player Stuff -----")]
    public GameObject player;
    public PlayerController playerScript;
    public GameObject playerSpawnPos;
    public int storeTokens;
    public int playerAmmo;

    [Header("----- UI Stuff -----")]
    public GameObject activeMenu;
    public GameObject pauseMenu;
    //public GameObject loseMenu;
    public GameObject comfirmMenu;
    public GameObject errorMenu;
    public GameObject errorMenuText;
    public GameObject comfirmMenuText;

    public bool isPaused;
    float timeScaleOrig;

    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
        player = GameObject.FindGameObjectWithTag("Player");
        playerScript = player.GetComponent<PlayerController>();
        //playerSpawnPos = GameObject.FindGameObjectWithTag("Player Spawn Pos");
        timeScaleOrig = Time.timeScale;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetButton("Cancel") && activeMenu == null)
        {
            isPaused = !isPaused;
            activeMenu = pauseMenu;
            activeMenu.SetActive(isPaused);
            pauseState();
        }
    }

    public void pauseState()
    {
        Time.timeScale = 0;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
    }

    public void unPauseState()
    {
        Time.timeScale = timeScaleOrig;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        isPaused = !isPaused;
        activeMenu.SetActive(false);
        activeMenu = null;
    }

    //public void youLose()
    //{
    //    pauseState();
    //    activeMenu = loseMenu;
    //    activeMenu.SetActive(true);
    //}

    public void TradeAmmo(int ammount) 
    {
        playerAmmo -= ammount;
        storeTokens += ammount;
    }

    public void ErrorMenu(string errorText) 
    {
        pauseState();
        activeMenu = errorMenu;
        errorMenuText.GetComponent<TextMesh>().text = errorText;
        activeMenu.SetActive(true);
        isPaused = !isPaused;
    }

    public void ComfirmMenu(string actionText)
    {
        pauseState();
        activeMenu = comfirmMenu;
        errorMenuText.GetComponent<TextMesh>().text = actionText;
        activeMenu.SetActive(true);
        isPaused = !isPaused;
    }
}
