using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;


public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("----- Player Stuff -----")]
    public GameObject player;
    public PlayerController playerScript;
    public GameObject playerSpawnPos;
    public GunSystem gunSystem;
    public int storeTokens;
    public int playerAmmo;

    [Header("----- UI Stuff -----")]
    public GameObject ReloadText;
    public GameObject activeMenu;
    public GameObject pauseMenu;
    public GameObject loseMenu;
    public GameObject winMenu;
    public GameObject comfirmMenu;
    public GameObject errorMenu;
    public TextMeshProUGUI errorMenuText;
    public TextMeshProUGUI comfirmMenuText;
    public GameObject retical;
    public GameObject ShopMenu;
    public GameObject emptyAmmo;
    public TextMeshProUGUI ammoDisplay;
    public TextMeshProUGUI ammoMagCount;
    public TextMeshProUGUI HPDisplay;
    public GameObject loadingScreen;

    public int enemiesRemaining;
    public bool isPaused;
    public float timeElapsed;
    float timeScaleOrig;
    int AmmoLoaded;
    bool reloading;

    void Awake()
    {
        //Code to check if a new game manager is made, and if it is delete it.
        //Used for keeping the game manager and player UI throughout different scenes
        if (instance != null && instance != this) 
        {
            Destroy(gameObject.transform.parent.gameObject);
        }
        else 
        {
            instance = this;
            player = GameObject.FindGameObjectWithTag("Player");
            playerScript = player.GetComponent<PlayerController>();
            playerSpawnPos = GameObject.FindGameObjectWithTag("Player Spawn Pos");
            timeScaleOrig = Time.timeScale;
            gunSystem = player.GetComponentInChildren<GunSystem>(player);
            AmmoLoaded = gunSystem.GetMagCount();
            UpdateAmmoCount();
            
        }

        DontDestroyOnLoad(this.transform.parent);

    }

    //Refereshes the game manger on a new scene loaded. Just to get all the prefabs re-loaded.
    
    //TODO: Make player not be destroyed between scenes? Just a thought.
    public void refreshGameManager() 
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerScript = player.GetComponent<PlayerController>();
        playerSpawnPos = GameObject.FindGameObjectWithTag("Player Spawn Pos");
        timeScaleOrig = Time.timeScale;
        gunSystem = player.GetComponentInChildren<GunSystem>(player);
        UpdateAmmoCount();
    }

    void Update()
    {
        if(Input.GetButton("Cancel") && activeMenu == null)
        {
            isPaused = !isPaused;
            activeMenu = pauseMenu;
            activeMenu.SetActive(isPaused);
            pauseState();
        }
        timeElapsed += Time.deltaTime;
    }

    public void pauseState()
    {
        Time.timeScale = 0;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
        retical.SetActive(false);
        
    }

    public void unPauseState()
    {
        Time.timeScale = timeScaleOrig;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        isPaused = !isPaused;
        activeMenu.SetActive(false);
        activeMenu = null;
        retical.SetActive(true);
    }

    public void youLose()
    {
        isPaused = !isPaused;
        pauseState();
        activeMenu = loseMenu;
        activeMenu.SetActive(true);
    }

    public void TradeAmmo(int ammount) 
    {
        playerAmmo -= ammount;
        storeTokens += ammount;
    }

    public void ErrorMenu(string errorText) 
    {
        pauseState();
        activeMenu = errorMenu;
        errorMenuText.text = errorText;
        activeMenu.SetActive(true);
        isPaused = !isPaused;
    }

    public void ComfirmMenu(string actionText)
    {
        pauseState();
        activeMenu = comfirmMenu;
        comfirmMenuText.text = actionText;
        activeMenu.SetActive(true);
        isPaused = !isPaused;
    }

    public void Shop() 
    {
        pauseState();
        activeMenu = ShopMenu;
        activeMenu.SetActive(true);
    }
    public void UpdateEnemyCount(int amount)
    {
        int enemiesKilled = 0;
        enemiesRemaining += amount;
        if(amount < 0)
        {
            enemiesKilled += amount * -1;
        }
        WinnersToken.instance.UpdateEnemyCount(enemiesKilled);
    }

    public IEnumerator youWin(float time)
    {
       yield return new WaitForSeconds(time); 
        activeMenu = winMenu;
        activeMenu.SetActive(true);
        pauseState();
        yield return null;
    }
    public void UpdateAmmoCount()
    {
        ammoDisplay.text = gunSystem.GetAmmoCount().ToString();
        ammoMagCount.text = gunSystem.GetMagCount().ToString();
    }
    public void CharReloading()
    {
        StartCoroutine(Reload());
    }

    IEnumerator Reload()
    {
        activeMenu = ReloadText;
        activeMenu.SetActive(true);
        yield return new WaitForSeconds(gunSystem.GetReloadTime());
        activeMenu = ReloadText;
        activeMenu.SetActive(false);
        activeMenu = null;
    }
    public void UpdatePlayerHP()
    {
        HPDisplay.text = playerScript.GetPlayerHP().ToString();
    }

    public void CharZeroAmmo()
    {
        StartCoroutine(ZeroAmmo());
    }
    IEnumerator ZeroAmmo()
    {
        activeMenu = ReloadText;
        activeMenu.SetActive(false);
        activeMenu = emptyAmmo;
        activeMenu.SetActive(true);
        ammoDisplay.color = Color.red;
        yield return new WaitForSeconds(2);
        ammoDisplay.color = Color.white;
        activeMenu = emptyAmmo;
        activeMenu.SetActive(false);
        activeMenu = null;
    }
}
