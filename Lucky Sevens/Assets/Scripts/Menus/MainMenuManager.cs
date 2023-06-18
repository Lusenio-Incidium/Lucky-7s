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
    public GameObject loginMenu;
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
    public AudioSource playSoundAudSource;
    public AudioSource music;
    public AudioClip buttonPressAud;
    public AudioClip playSoundAud;
    float timeScaleOrig;
    public bool mainEasy;
    public bool mainMedium;
    public bool mainHard;
    public bool isCompleted;
    public float sensitivity;
    public float SFXVolume;
    public float musicVolume;

    //Account stuff
    public GameObject loginButton;
    public GameObject accountMenu;
    public Button logIn;
    public Button logOut;
    public Button easy;
    public TMP_InputField username;
    public TMP_InputField password;
    public TextMeshProUGUI usernameText;
    public Image avatar;
    System.Action<bool> signedInCallback;

    // Start is called before the first frame update
    void Start()
    {
        Destroy(GameObject.FindGameObjectWithTag("Player"));
        if(GameManager.instance != null)
        {
            musicVolume = GameManager.instance.playerScript.GetMusicVol() * 10f;
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
        //StartCoroutine(musicFadeIn());

        if(GameJolt.API.GameJoltAPI.Instance.CurrentUser != null) 
        {
            loginButton.SetActive(false);
            accountMenu.SetActive(true);
        }
        music.volume = 0;
        
    }

    // Update is called once per frame
    void Update()
    {
        music.volume = musicVolume / 10f;

        if (GameJolt.API.GameJoltAPI.Instance.CurrentUser != null && avatar != GameJolt.API.GameJoltAPI.Instance.CurrentUser.Avatar) 
        {
            GameJolt.API.GameJoltAPI.Instance.CurrentUser.DownloadAvatar();
            avatar.sprite = GameJolt.API.GameJoltAPI.Instance.CurrentUser.Avatar;
            usernameText.text = GameJolt.API.GameJoltAPI.Instance.CurrentUser.Name;
        }
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

    public void fadeOut() 
    {
        StartCoroutine(musicFade());
    }

    IEnumerator musicFade() 
    {
        while(music.volume > 0) 
        {
            music.volume -= 0.05f;
            yield return new WaitForSeconds(0.1f);
        }
    }

    //IEnumerator musicFadeIn()
    //{
    //    while (music.volume < musicVolume / 10f)
    //    {
    //        music.volume += 0.05f;
    //        yield return new WaitForSeconds(0.1f);
    //    }
    //}

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

    public void LoginMenu() 
    {
        activeMenu.SetActive(false);
        activeMenu = loginMenu;
        activeMenu.SetActive(true);
    }

    public void LogOut() 
    {
        GameJolt.API.GameJoltAPI.Instance.CurrentUser.SignOut();
        loginButton.SetActive(true);
        accountMenu.SetActive(false);
    }

    public void LogIn() 
    {
        if (username.text.Trim() == string.Empty || password.text.Trim() == string.Empty)
        {
            
        }
        else
        {


            var user = new GameJolt.API.Objects.User(username.text.Trim(), password.text.Trim());
            user.SignIn(signInSuccess =>
            {
                if (signInSuccess)
                {
                    Dismiss(true);
                }
            });

            
            
            loginButton.SetActive(false);
            accountMenu.SetActive(true);
            activeMenu.SetActive(false);
            activeMenu = mainMenu;
            activeMenu.SetActive(true);
        }
    }

    public void Dismiss(bool success)
    {
        if (signedInCallback != null)
        {
            signedInCallback(success);
            signedInCallback = null;
        }
        loginButton.SetActive(false);
        accountMenu.SetActive(true);
    }

    public void DifficultyMenu()
    {
        easy.Select();
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
