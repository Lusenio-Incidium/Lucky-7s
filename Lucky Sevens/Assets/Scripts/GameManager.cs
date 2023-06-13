using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System.ComponentModel;
/*public enum LevelSelect
{
    Tutorial = 0,
    Training,
    Level1,
    Level2,
    Level3,
    Level4,
    SlotsBoss
}*/

public class GameManager : MonoBehaviour
{


    public static GameManager instance;

    [Header("----- Player Stuff -----")]
    public GameObject player;
    public GameObject playerCam;
    public Transform playerCamSecondPos;
    public PlayerController playerScript;
    public Animator playerAnim;
    public GameObject playerSpawnPos;
    public GunSystem gunSystem;
    public int storeTokens;
    public int playerAmmo;
    public AudioClip victory;

    [Header("----- UI Stuff -----")]
    public GameObject ReloadText;
    public GameObject activeMenu;
    public GameObject pauseMenu;
    public GameObject loseMenu;
    public GameObject winMenu;
    public GameObject optionsMenu;
    public GameObject comfirmMenu;
    public GameObject toMainMenuConfirmMenu;
    public GameObject errorMenu;
    public GameObject difficultyMenu;
    public GameObject inGameOptionsMenu;
    public GameObject completionText;
    public GameObject healthMenu;
    public GameObject lowHealthFlashMenu;
    public GameObject PlayerModelForDeath;
    public TextMeshProUGUI sensitivityText;
    public TextMeshProUGUI SFXText;
    public TextMeshProUGUI musicText;
    public TextMeshProUGUI errorMenuText;
    public TextMeshProUGUI comfirmMenuText;
    public GameObject activeRetical;
    public GameObject defaultRetical;
    public GameObject arPistolRetical;
    public GameObject shotgunRetical;
    public GameObject ShopMenu;
    public GameObject emptyReserve;
    public GameObject emptyMag;
    public GameObject ammoDisplay;
    public GameObject playerDamageFlash;
    public DamageFlash dm;
    public TextMeshProUGUI shopChipsTotal;
    public TextMeshProUGUI ammoReserveCount;
    public TextMeshProUGUI ammoMagCount;
    public TextMeshProUGUI HPDisplay;
    public TextMeshProUGUI timerDisplay;
    public GameObject loadingScreen;
    public GameObject interactTxt;
    public Image playerHPBar;
    public Image backPlayerHPBar;
    public Image BossBar;
    public Image sensitivityBar;
    public Image SFXBar;
    public Image musicBar;
    public Image damagePanel;
    public Image damageBlood;
    public GameObject BossBarContainer;
    public TextMeshProUGUI bossName;
    public bool easy;
    public bool medium;
    public bool hard;
    public GameObject hardMode;
    public AudioSource playSoundAudSource;
    public AudioClip buttonSoundAud;
    public AudioClip playSoundAud;
    public GameObject ending;

    public int enemiesRemaining;
    public bool coinCollected;
    public bool hasSpeedUpgrade;
    public bool hasAR;
    public bool hasShotgun;
    public bool isPaused;
    public float timeElapsed;
    public float sensitivity;
    public float sfxVol;
    public float musicVol;
    float timeScaleOrig;
    int AmmoLoaded;
    bool reloading;
    bool timerInc;
    public bool completed;
    public int ammoUsedTotal;
    public int ammoGatheredTotal;
    public int enemiesKilled;
    Vector3 origCamPos;
    GameObject playerStore;

    //Level Manager Variables
    List<int> completedLevels;
    int recentCompletedLevel = 0;

    void Awake()
    {
        completedLevels = new List<int>();
        //Debug.Log(completedLevels.Count);
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
        if (!completed)
        {
            Locked();
        }
        activeRetical = defaultRetical;
        if (MainMenuManager.instance != null)
        {
            medium = MainMenuManager.instance.mainMedium;
            easy = MainMenuManager.instance.mainEasy;
            hard = MainMenuManager.instance.mainHard;
            completed = MainMenuManager.instance.isCompleted;
            Destroy(MainMenuManager.instance.transform.parent.gameObject);
        }

    }

    //Refereshes the game manger on a new scene loaded. Just to get all the prefabs re-loaded.
    public void refreshGameManager() 
    {
        //Initalize GameManager
        player = GameObject.FindGameObjectWithTag("Player");
        playerScript = player.GetComponent<PlayerController>();
        playerCam = GameObject.FindGameObjectWithTag("MainCamera");
        playerSpawnPos = GameObject.FindGameObjectWithTag("Player Spawn Pos");
        dm = playerDamageFlash.GetComponent<DamageFlash>();
        timeScaleOrig = Time.timeScale;
        playerAnim = player.GetComponent<Animator>();
        gunSystem = player.GetComponent<GunSystem>();
        enemiesRemaining = 0;
        timeElapsed = 0;
        ammoGatheredTotal = 0;
        ammoUsedTotal = 0;
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

    public void UpdateCompleteLevels() //Searches through the CompletedLevels List to see if the level already was completed before adding it. Called through HubManager.
    {
        if(recentCompletedLevel <= 0) //For a Null Token set to 0
        {
            return;
        }
        foreach (int num in completedLevels)
        {
            if (num == recentCompletedLevel)
            {
                return;
            }
        }
        completedLevels.Add(recentCompletedLevel);
    }
    
    void Update()
    {
        //Pause Menu Code
        if(Input.GetButton("Cancel") && activeMenu == null)
        {
            PauseMenu();
        }
        if (coinCollected)
        {
            if (Input.GetKey(KeyCode.Space))
            {
                StartCoroutine(youWin(2));
                coinCollected = false;
                //playerAnim.SetBool("gotACoin", false);
            }
        }
        /* if (trauma > 0)
         {
             StartCoroutine(TraumaDown());
         }*/
        updateTimer();
    }

    public void pauseState()
    {
        Time.timeScale = 0;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
        activeRetical.SetActive(false);
        
    }

    public void InGameOptions()
    {
        isPaused = !isPaused;
        pauseState();
        sensitivity = playerCam.GetComponent<CameraController>().GetSensitivity();
        sensitivityText.text = sensitivity.ToString();
        sensitivityBar.fillAmount = sensitivity / 10f;
        sfxVol = playerScript.GetJumpVol() * 10f;
        SFXText.text = sfxVol.ToString();
        SFXBar.fillAmount = sfxVol / 10f;
        musicVol = playerScript.GetMusicAud().volume * 10f;
        musicText.text = musicVol.ToString();
        musicBar.fillAmount = musicVol / 10f;
        activeMenu.SetActive(false);
        activeMenu = null;
        activeMenu = inGameOptionsMenu;
        activeMenu.SetActive(true);
    }


    public void PauseMenu()
    {
        isPaused = !isPaused;
        activeMenu = pauseMenu;
        activeMenu.SetActive(isPaused);
        pauseState();
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
        playerScript.Invincible(true);
        playerScript.fadeOut();
        yield return new WaitForSeconds(3f);
        anim.ResetTrigger("Transition");
        SceneManager.LoadScene(sceen);
        loadingScreen.GetComponent<autoShutoff>().enabled = true;
        playerScript.Invincible(false);

        
    }

    public void startGameOver() 
    {
        playerScript.Invincible(true);
        isPaused = false;
        unPauseState();
        ending.SetActive(true);
        StartCoroutine(endWait());
    }

    IEnumerator endWait() 
    {
        yield return new WaitForSeconds(5f);
        SceneManager.LoadScene("MainMenu");
    }


    public void unPauseState()
    {
        //Put this in an if check to stop some bugs.
        if(activeMenu != null) 
        {
            Time.timeScale = timeScaleOrig;
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            isPaused = false;
            activeMenu.SetActive(false);
            activeMenu = null;
            activeRetical.SetActive(true);
            
        }
        
    }
    public void OptionsMenu()
    {
        isPaused = true;
        pauseState();
        activeMenu.SetActive(false);
        activeMenu = null;
        activeMenu = optionsMenu;
        activeMenu.SetActive(true);
    }


    public void youLose()
    {
        isPaused = true;
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
        isPaused = true;
    }

    public void ComfirmMenu(string actionText)
    {
        pauseState();
        activeMenu = comfirmMenu;
        comfirmMenuText.text = actionText;
        activeMenu.SetActive(true);
        isPaused = true;
    }
    public void BackToMainMenuConfirm()
    {
        isPaused = true;
        pauseState();
        activeMenu.SetActive(false);
        activeMenu = toMainMenuConfirmMenu;
        activeMenu.SetActive(true);
    }

    public void Shop() 
    {
        isPaused = true;
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
        ReEnableDisplay();
        isPaused = true;
        pauseState();
        playerAnim.SetBool("gotACoin", false);
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
    public void DifficultyMenu()
    {
        activeMenu.SetActive(false);
        activeMenu = null;
        activeMenu = difficultyMenu;
        activeMenu.SetActive(true);
    }
    public void ReturnToLoseScreen()
    {
        activeMenu.SetActive(false);
        activeMenu = null;
        activeMenu = loseMenu;
        activeMenu.SetActive(true);
    }
    public void Locked()
    {
        hardMode.GetComponent<Button>().enabled = false;
        hardMode.GetComponent<Image>().color = Color.gray;
    }    

    public void SetLatestLevel(int levelID)
    {
        recentCompletedLevel = levelID;
    }
     public int GetLastestLevel()
    {
        return recentCompletedLevel; 
    }
    public List<int> GetCompletedLevels()
    {

        return completedLevels;
    }
    public void WinSequence()
    {
        SpawnPlayerForWinAnim();
        Transform secondCamera = player.transform.GetChild(0);
        origCamPos = playerCam.transform.localPosition;
        lowHealthFlashMenu.SetActive(false);
        HudDisabledDisplay(secondCamera);
        //playerAnim.SetBool("gotACoin", true);
        activeMenu = completionText;
        activeMenu.SetActive(true);
        coinCollected = true;
        playerScript.SetMusic(victory);
    }
    public IEnumerator DeathSequence()
    {
        SpawnPlayerForDeathAnim();
        Transform deathCamera = player.transform.GetChild(3);
        activeMenu = completionText;
        origCamPos = playerCam.transform.localPosition;
        HudDisabledDisplay(deathCamera);
        //playerAnim.SetBool("dead", true);
        yield return new WaitForSeconds(3);
        youLose();
    }

    public void HudDisabledDisplay(Transform Camera)
    {
        playerCam.GetComponent<CameraController>().enabled = false;
        playerScript.enabled = false;
        playerCam.transform.localPosition = Camera.localPosition;
        playerCam.transform.LookAt(player.transform);
        playerCam.transform.GetChild(1).gameObject.SetActive(false);
        gunSystem.enabled = false;
        timerDisplay.enabled = false;
        activeRetical.gameObject.SetActive(false);
        healthMenu.SetActive(false);
        ammoDisplay.SetActive(false);
    }
    public void ReEnableDisplay()
    {
        playerCam.GetComponent<CameraController>().enabled = true;
        playerScript.enabled = true;
        completionText.SetActive(false);
        if (origCamPos != Vector3.zero)
        {
            playerCam.transform.localPosition = origCamPos;
        }
        playerCam.transform.GetChild(1).gameObject.SetActive(true);
        gunSystem.enabled = true;
        timerDisplay.enabled = true;
        activeRetical.gameObject.SetActive(true);
        lowHealthFlashMenu.SetActive(true);
        healthMenu.SetActive(true);
        ammoDisplay.SetActive(true);
        if(playerStore != null)
        {
            ObjectPoolManager.instance.ReturnObjToInfo(playerStore);
        }
    }
    public void SpawnPlayerForDeathAnim()
    {
       playerStore = ObjectPoolManager.instance.SpawnObject(PlayerModelForDeath,player.transform.position,player.transform.localRotation);
       playerStore.GetComponent<Animator>().SetBool("dead", true);
    }
    public void SpawnPlayerForWinAnim()
    {
       playerStore = ObjectPoolManager.instance.SpawnObject(PlayerModelForDeath,player.transform.position,player.transform.localRotation);
       playerStore.GetComponent<Animator>().SetBool("gotACoin", true);
    }
}
