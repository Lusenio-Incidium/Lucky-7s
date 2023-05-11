using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.EditorTools;

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

    public int enemiesRemaining;
    public bool isPaused;
    float timeScaleOrig;
    int magSize;
    bool reloading;

    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
        player = GameObject.FindGameObjectWithTag("Player");
        playerScript = player.GetComponent<PlayerController>();
        playerSpawnPos = GameObject.FindGameObjectWithTag("Player Spawn Pos");
        timeScaleOrig = Time.timeScale;
        gunSystem = player.GetComponentInChildren<GunSystem>(player);
        UpdateAmmoCount();
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
    public void updateGameGoal(int amount)
    {
        enemiesRemaining += amount;

        if (enemiesRemaining <= 0)
        {
            StartCoroutine(youWin());
        }
    }

    IEnumerator youWin()
    {
        yield return new WaitForSeconds(1);
        activeMenu = winMenu;
        activeMenu.SetActive(true);
        pauseState();
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
