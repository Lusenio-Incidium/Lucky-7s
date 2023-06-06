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
    public TextMeshProUGUI sensitivitytext;
    float timeScaleOrig;
    public bool mainEasy;
    public bool mainMedium;
    public bool mainHard;
    public bool isCompleted;
    public float sensitivity;
    // Start is called before the first frame update
    void Start()
    {
        Destroy(GameObject.FindGameObjectWithTag("Player"));
        if(GameManager.instance != null)
        {
            sensitivity = GameManager.instance.playerCam.GetComponent<CameraController>().GetSensitivity();
            Destroy(GameManager.instance.transform.parent.gameObject);
        }
        instance = this;
        activeMenu = mainMenu;
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
        TextMeshProUGUI buttonText = hardButton.GetComponentInChildren<TextMeshProUGUI>();
        buttonText.color = Color.red;
    }

    
}
