using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public static MainMenuManager instance;

    public GameObject activeMenu;
    public GameObject optionsMenu;
    public GameObject creditsMenu;
    public GameObject loadingScreen;
    public GameObject mainMenu;
    float timeScaleOrig;
    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        activeMenu = mainMenu;
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
        activeMenu.SetActive(false);
        activeMenu = null;
        activeMenu = optionsMenu;
        activeMenu.SetActive(true);
    }
}
