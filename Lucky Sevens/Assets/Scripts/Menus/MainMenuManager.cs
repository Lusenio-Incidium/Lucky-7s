using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class MainMenuManager : MonoBehaviour
{
    public static MainMenuManager instance;

    public GameObject activeMenu;
    public GameObject optionsMenu;
    public GameObject creditsMenu;
    public GameObject loadingScreen;
    public GameObject mainMenu;
    public GameObject difficultyMenu;
    public GameObject hardButton;
    public Image sensitivityBar;
    public Image SFXVolumeBar;
    public Image musicVolumeBar;
    public TextMeshProUGUI sensitivitytext;
    public TextMeshProUGUI SFXvolumetext;
    public TextMeshProUGUI musicvolumetext;
    float timeScaleOrig;
    public bool mainEasy;
    public bool mainMedium;
    public bool mainHard;
    public bool isCompleted;
    public float sensitivity;
    public float SFXVolume;
    public float musicVolume;
    // Start is called before the first frame update
    void Start()
    {
        Destroy(GameObject.FindGameObjectWithTag("Player"));
        if(GameManager.instance != null)
        {
            musicVolume = GameManager.instance.playerScript.GetMusicAud().volume * 10f;
            SFXVolume = GameManager.instance.playerScript.GetJumpVol() * 10f;
            sensitivity = GameManager.instance.playerCam.GetComponent<CameraController>().GetSensitivity();
            isCompleted = GameManager.instance.completed;
            Destroy(GameManager.instance.transform.parent.gameObject);
        }
        instance = this;
        activeMenu = mainMenu;
        DontDestroyOnLoad(this.transform.parent);
        if (!isCompleted)
        {
            MainLocked();
        }
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
        
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public IEnumerator loadScene(string sceen)
    {
        //Load a scene with the loadTransition so we have a cool loading screen.
        Animator anim = loadingScreen.GetComponent<Animator>();
        activeMenu = loadingScreen;
        MainMenuManager.instance.loadingScreen.SetActive(true);
        anim.SetTrigger("Transition");

        yield return new WaitForSeconds(3f);
        anim.ResetTrigger("Transition");
        SceneManager.LoadScene(sceen);
        loadingScreen.GetComponent<autoShutoff>().enabled = true;
    }

    public void CreditsMenu()
    {
        activeMenu.SetActive(false);
        activeMenu = null;
        activeMenu = creditsMenu;
        activeMenu.SetActive(true);
    }

    public void OptionsMenu()
    {
        sensitivitytext.text = sensitivity.ToString();
        sensitivityBar.fillAmount = sensitivity / 10f;
        SFXvolumetext.text = SFXVolume.ToString();
        SFXVolumeBar.fillAmount = SFXVolume / 10f;
        musicvolumetext.text = musicVolume.ToString();
        musicVolumeBar.fillAmount = musicVolume / 10f;
        activeMenu.SetActive(false);
        activeMenu = null;
        activeMenu = optionsMenu;
        activeMenu.SetActive(true);
    }
    public void DifficultyMenu()
    {
        activeMenu.SetActive(false);
        activeMenu = null;
        activeMenu = difficultyMenu;
        activeMenu.SetActive(true);
    }
    public void MainLocked()
    {
        hardButton.GetComponent<Button>().enabled = false;
        hardButton.GetComponent<Image>().color = Color.black;
        TextMeshProUGUI buttonText = hardButton.GetComponentInChildren<TextMeshProUGUI>();
        buttonText.color = Color.red;
    }

    
}
