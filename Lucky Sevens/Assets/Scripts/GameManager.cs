using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System.ComponentModel;

public class GameManager : MonoBehaviour
{
    [System.Serializable]
    public class Menu
    {
        public GameObject menu;
        public string preMenu;
        public string name;
    }

    public static GameManager instance;

    [Header("----- Player Stuff -----")]
    public GameObject player;
    public GameObject playerCam;
    public Transform playerCamSecondPos;
    public PlayerController playerScript;
    public Animator playerAnim;
    public GameObject playerSpawnPos;
    public int storeTokens;
    public int playerMag;
    public int playerMagOrign;
    public int playerAmmo;
    public int playerAmmoOrign;
    public AudioClip victory;

    [Header("----- UI Stuff -----")]
    public Menu[] menus;
    public GameObject ReloadText;
    public GameObject activeMenu;
    public Button returnToLobby;
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
    public string[] tips;
    public TextMeshProUGUI tipsText;
    public Image[] inventorySlots = new Image[20];

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
    public bool completed;
    public int ammoUsedTotal;
    public int ammoGatheredTotal;
    public int enemiesKilled;
    public bool didRestart;
    public bool pressedSpace;
    Vector3 origCamPos;
    GameObject playerStore;
    Dictionary<string, Menu> menuDictionary = new Dictionary<string, Menu>();
    Menu actMenu;

    //Level Manager Variables
    List<int> completedLevels;
    int recentCompletedLevel = 0;

    void Awake()
    {
        completed = false;
        completedLevels = new List<int>();

        //Code to check if a new game manager is made, and if it is delete it.
        //Used for keeping the game manager and player UI throughout different scenes
        if (instance != null) 
        {
            Destroy(gameObject.transform.parent.gameObject);
        }
        else 
        {
            instance = this;
            for (int i = 0; i < menus.Length; i++)
            {
                menuDictionary.TryAdd(menus[i].name, menus[i]);
            }
            menus = null;
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
        timeScaleOrig = Time.timeScale;
        playerAnim = player.GetComponent<Animator>();
        enemiesRemaining = 0;
        timeElapsed = 0;
        ammoGatheredTotal = 0;
        ammoUsedTotal = 0;
        //GameManager.instance.playerScript.HPOrig = GameManager.instance.playerScript.HP
        UpdateAmmoCount();

        //shopRefresh
        ShopMenu.GetComponent<ShopController>().updateCrate();

        //Timer Refresh
        if(SceneManager.GetActiveScene().name != "TheHub") 
        {
            timerDisplay.gameObject.transform.parent.gameObject.SetActive(true);
            if (SceneManager.GetActiveScene().name == "Tutorial")
            {
                returnToLobby.interactable = false;
            }
            else
            {
                returnToLobby.interactable = true;
            }
        }
        else 
        {
            timerDisplay.gameObject.transform.parent.gameObject.SetActive(false);
            returnToLobby.interactable = false;
        }

        //player refresh
        playerScript.spawnPlayerOnLoad();
        playerScript.updatePlayerUI();

    }

    public void onLoad() 
    {
        playerMagOrign = playerMag;
        playerAmmoOrign = playerAmmo;
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
        if(Input.GetButtonDown("Cancel") && activeMenu == null)
        {
            ChangeMenu("PauseMenu");
        }
        else if(Input.GetButtonDown("Cancel")) 
        {
            PreviousMenu();
        }
        if (coinCollected)
        {
            if (Input.GetKey(KeyCode.Space))
            {
                if (!pressedSpace)
                {
                    pressedSpace = true;
                    StartCoroutine(youWin(0));
                    coinCollected = false;
                }
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
        activeMenu.SetActive(true);

    }


    public void ChangeMenu(string menuToLoad)
    {
        if (menuDictionary.TryGetValue(menuToLoad, out actMenu))
        {
            if (activeMenu)
                activeMenu.SetActive(false);
            activeMenu = actMenu.menu;

            if (!isPaused)
            {
                pauseState();
            }
            else
            {
                activeMenu.SetActive(true);
            }
        }
    }

    public void PreviousMenu()
    {
        if (actMenu.preMenu == "NOESCAPE")
            return;
        if (actMenu.preMenu != " ")
        {
            ChangeMenu(actMenu.preMenu);
        }
        else
        {
            unPauseState();
        }
    }


    public void InGameOptions()
    {
        isPaused = true;
        pauseState();
        sensitivity = playerCam.GetComponent<CameraController>().GetSensitivity();
        sensitivityText.text = sensitivity.ToString();
        sensitivityBar.fillAmount = sensitivity / 10f;
        sfxVol = playerScript.GetJumpVol() * 10f;
        SFXText.text = sfxVol.ToString();
        SFXBar.fillAmount = sfxVol / 10f;
        musicVol = playerScript.GetMusicVol() * 10f;
        musicText.text = musicVol.ToString();
        musicBar.fillAmount = musicVol / 10f;
        ChangeMenu("OptionsMenu");
    }


    public IEnumerator loadScene(string sceen)
    {
        //Load a scene with the loadTransition so we have a cool loading screen.
        isPaused = true;
        Animator anim = loadingScreen.GetComponent<Animator>();
        activeMenu = loadingScreen;
        tipsText.text = tips[Random.Range(0,tips.Length)];
        GameManager.instance.loadingScreen.SetActive(true);
        anim.SetTrigger("Transition");
        playerScript.Invincible(true);
        playerScript.fadeOut();
        yield return new WaitForSeconds(3f);
        anim.ResetTrigger("Transition");
        SceneManager.LoadScene(sceen);
        loadingScreen.GetComponent<autoShutoff>().enabled = true;
        playerScript.Invincible(false);
        BossBarContainer.SetActive(false);
        
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
            actMenu = null;
            activeRetical.SetActive(true);
            
        }
        
    }



    public void ErrorMenu(string errorText) 
    {
        errorMenuText.text = errorText;
        ChangeMenu("Error");
    }

    public void ComfirmMenu(string actionText)
    {
        comfirmMenuText.text = actionText;
        ChangeMenu("Comfirm");
    }

    public void Shop() 
    {
        shopChipsTotal.text = playerAmmo.ToString("F0");
        ChangeMenu("Shop");
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
        ChangeMenu("Win");
        ReEnableDisplay();
        pressedSpace = false;
        playerAnim.SetBool("gotACoin", false);
    }
    public void UpdateAmmoCount()
    {
       if (playerScript.hasGun) 
       {
           if (playerScript.equipedGuns[playerScript.selectedGun] == null || !playerScript.equipedGuns[playerScript.selectedGun].destroyOnEmpty)
           {
               ammoReserveCount.text = playerAmmo.ToString();
               ammoMagCount.text = playerMag.ToString();
           }
       //    else
       //    {
       //        ammoReserveCount.text = gunSystem.GetAmmo().ToString();
       //        ammoMagCount.text = gunSystem.GetMagCount().ToString();
       //    }
       }
    }

    public void updateTimer()
    {
        timeElapsed += Time.deltaTime;
        timerDisplay.text = timeElapsed.ToString("F2");
    }
    public void CharReloading()
    {
        //if(gunSystem.hasGun == true)
           StartCoroutine(Reload());
    }

    IEnumerator Reload()
    {
        if (!isPaused)
        {
            activeMenu = ReloadText;
            activeMenu.SetActive(true);
           // yield return new WaitForSeconds(gunSystem.GetReloadTime());
           yield return new WaitForSeconds(0.1f);
            activeMenu = ReloadText;
            activeMenu.SetActive(false);
            activeMenu = null;
        }
        
    }
    public void CharZeroReserve()
    {
        //if (gunSystem.hasGun == true)
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

    public void SetWin()
    {
        completed = true;
    }
    public void WinSequence()
    {
        SpawnPlayerForWinAnim();
        Transform secondCamera = player.transform.GetChild(1);
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
        Transform deathCamera = player.transform.GetChild(4);
        origCamPos = playerCam.transform.localPosition;
        HudDisabledDisplay(deathCamera);
        //playerAnim.SetBool("dead", true);
        yield return new WaitForSeconds(3);
        ChangeMenu("Lose");
    }

    public void HudDisabledDisplay(Transform Camera)
    {
        playerCam.GetComponent<CameraController>().enabled = false;
        playerScript.enabled = false;
        playerCam.transform.localPosition = Camera.localPosition;
        playerCam.transform.LookAt(player.transform);
        playerCam.transform.GetChild(1).gameObject.SetActive(false);
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
        timerDisplay.enabled = true;
        activeRetical.gameObject.SetActive(true);
        lowHealthFlashMenu.SetActive(true);
        healthMenu.SetActive(true);
        ammoDisplay.SetActive(true);
        if(playerStore != null)
        {
            playerScript.GetComponent<Animator>().SetBool("dead", false);
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
    public IEnumerator WaitForFall()
    {
        while (!playerScript.GetCharacterController().isGrounded)
        {
            yield return null;
        }
        WinSequence();
    }
}
