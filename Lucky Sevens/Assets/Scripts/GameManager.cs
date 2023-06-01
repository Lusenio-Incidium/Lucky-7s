using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("----- Player Stuff -----")]
    public GameObject player;
    public GameObject playerCam;
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
    public GameObject playerDamageFlash;
    public TextMeshProUGUI shopChipsTotal;
    public TextMeshProUGUI ammoReserveCount;
    public TextMeshProUGUI ammoMagCount;
    public TextMeshProUGUI HPDisplay;
    public TextMeshProUGUI timerDisplay;
    public GameObject loadingScreen;
    public GameObject interactTxt;
    public Image playerHPBar;
    public Image BossBar;
    public GameObject BossBarContainer;
    public TextMeshProUGUI bossName;

    public int enemiesRemaining;
    public bool isPaused;
    public float timeElapsed;
    float timeScaleOrig;
    int AmmoLoaded;
    bool reloading;
    bool timerInc;

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
            refreshGameManager();
        }

        DontDestroyOnLoad(this.transform.parent);

    }

    //Refereshes the game manger on a new scene loaded. Just to get all the prefabs re-loaded.
    public void refreshGameManager() 
    {
        //Initalize GameManager
        player = GameObject.FindGameObjectWithTag("Player");
        playerScript = player.GetComponent<PlayerController>();
        playerCam = GameObject.FindGameObjectWithTag("MainCamera");
        playerSpawnPos = GameObject.FindGameObjectWithTag("Player Spawn Pos");
        timeScaleOrig = Time.timeScale;
        gunSystem = player.GetComponent<GunSystem>();
        enemiesRemaining = 0;
        timeElapsed = 0;
        gunSystem.updateOrig();
        UpdateAmmoCount();

        //shopRefresh
        ShopMenu.GetComponent<ShopController>().updateCrate();

        //Timer Refresh
        if(SceneManager.GetActiveScene().name != "TheHub") 
        {
            timerDisplay.gameObject.transform.parent.gameObject.SetActive(true);
        }
        else 
        {
            timerDisplay.gameObject.transform.parent.gameObject.SetActive(false);
        }

        //player refresh
        playerScript.spawnPlayerOnLoad();
        playerScript.updatePlayerUI();

    }

    void Update()
    {
        //Pause Menu Code
        if(Input.GetButton("Cancel") && activeMenu == null)
        {
            isPaused = !isPaused;
            activeMenu = pauseMenu;
            activeMenu.SetActive(isPaused);
            pauseState();
        }
        
        updateTimer();
    }

    public void pauseState()
    {
        Time.timeScale = 0;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
        retical.SetActive(false);
        
    }

    IEnumerator timerIncrease() 
    {
        timerInc = true;
        timeElapsed += Time.deltaTime;
        yield return new WaitForSeconds(0.001f);
        timerInc = false;
    }

    public IEnumerator loadScene(string sceen)
    {
        //Load a scene with the loadTransition so we have a cool loading screen.
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
        //Put this in an if check to stop some bugs.
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
        isPaused = !isPaused;
        pauseState();
        activeMenu = ShopMenu;

        shopChipsTotal.text = playerAmmo.ToString("F0");

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
        //Added a check so enemies stop bugging out
        if(WinnersToken.instance != null)
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
        ammoReserveCount.text = gunSystem.GetAmmoCount().ToString();
        ammoMagCount.text = gunSystem.GetMagCount().ToString();
    }

    public void updateTimer()
    {
        if (!timerInc)
            StartCoroutine(timerIncrease());
        timerDisplay.text = timeElapsed.ToString("F2");
    }
    public void CharReloading()
    {
        if(gunSystem.hasGun == true)
           StartCoroutine(Reload());
    }

    IEnumerator Reload()
    {
        if (!isPaused)
        {
            activeMenu = ReloadText;
            activeMenu.SetActive(true);
            yield return new WaitForSeconds(gunSystem.GetReloadTime());
            activeMenu = ReloadText;
            activeMenu.SetActive(false);
            activeMenu = null;
        }
        
    }
    public void CharZeroReserve()
    {
        if (gunSystem.hasGun == true)
            StartCoroutine(ZeroReserve());
    }
    IEnumerator ZeroReserve()
    {
        if(!isPaused)
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
        
    }

    public void CharEmtpyMag()
    {
        if (gunSystem.hasGun == true)
            StartCoroutine(EmptyMag());
    }
    IEnumerator EmptyMag()
    {
        if(!isPaused)
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
}
