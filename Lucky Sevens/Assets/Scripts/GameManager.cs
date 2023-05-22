using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
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
    public GameObject emptyReserve;
    public GameObject emptyMag;
    public GameObject ammoDisplay;
    public TextMeshProUGUI ammoReserveCount;
    public TextMeshProUGUI ammoMagCount;
    public TextMeshProUGUI HPDisplay;
    public TextMeshProUGUI timerDisplay;
    public GameObject loadingScreen;
    public GameObject interactTxt;
    public Image playerHPBar;

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
        if (instance != null) 
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
            gunSystem = player.GetComponent<GunSystem>();
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
        gunSystem = player.GetComponent<GunSystem>();
        enemiesRemaining = 0;
        timeElapsed = 0;
        UpdateAmmoCount();
        //shopRefresh
        ShopMenu.GetComponent<ShopController>().updateCrate();
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
        updateTimer();
    }

    public void pauseState()
    {
        Time.timeScale = 0;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
        retical.SetActive(false);
        
    }


    public IEnumerator loadScene(string sceen)
    {
        isPaused = true;
        Animator anim = loadingScreen.GetComponent<Animator>();
        activeMenu = loadingScreen;
        GameManager.instance.loadingScreen.SetActive(true);
        anim.SetTrigger("Transition");

        yield return new WaitForSeconds(3f);
        anim.ResetTrigger("Transition");
        SceneManager.LoadScene(sceen);
        loadingScreen.GetComponent<autoShutoff>().enabled = true;
    }


    public void unPauseState()
    {
        if(activeMenu != null) 
        {
            Time.timeScale = timeScaleOrig;
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            isPaused = !isPaused;
            activeMenu.SetActive(false);
            activeMenu = null;
            retical.SetActive(true);
        }
        
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
        Debug.Log("Update Ammo Called");

        ammoReserveCount.text = gunSystem.GetAmmoCount().ToString();
        ammoMagCount.text = gunSystem.GetMagCount().ToString();

        Debug.Log("Mag Count = " + gunSystem.GetMagCount().ToString());
        Debug.Log("Reserve Count = " + gunSystem.GetAmmoCount().ToString());
    }

    public void updateTimer()
    {
        timerDisplay.text = timeElapsed.ToString("#.##");
    }
    public void CharReloading()
    {
        if(gunSystem.hasGun == true)
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
    public void CharZeroReserve()
    {
        if (gunSystem.hasGun == true)
            StartCoroutine(ZeroReserve());
    }
    IEnumerator ZeroReserve()
    {
        activeMenu = ReloadText;
        activeMenu.SetActive(false);
        activeMenu = emptyReserve;
        activeMenu.SetActive(true);
        ammoReserveCount.color = Color.red;
        yield return new WaitForSeconds(2);
        ammoReserveCount.color = Color.white;
        activeMenu = emptyReserve;
        activeMenu.SetActive(false);
        activeMenu = null;
    }

    public void CharEmtpyMag()
    {
        if (gunSystem.hasGun == true)
            StartCoroutine(EmptyMag());
    }
    IEnumerator EmptyMag()
    {
        activeMenu = emptyMag;
        activeMenu.SetActive(true);
        ammoMagCount.color = Color.red;
        yield return new WaitForSeconds(1);
        ammoMagCount.color = Color.white;
        activeMenu = emptyMag;
        activeMenu.SetActive(false);
        activeMenu = null;
    }
}
