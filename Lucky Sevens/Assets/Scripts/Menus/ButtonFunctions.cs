using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonFunctions : MonoBehaviour
{
    public void resume()
    {
        GameManager.instance.unPauseState();
    }
    public void restart()
    {
        GameManager.instance.unPauseState();
        StartCoroutine(GameManager.instance.loadScene(SceneManager.GetActiveScene().name));
        GameManager.instance.gunSystem.restartGun();
    }

    public void Quit()
    {
        Application.Quit();
    }
    public void respawnPlayer()
    {
        GameManager.instance.unPauseState();
        GameManager.instance.playerScript.spawnPlayer();
    }

    public void ReturnToLobby()
    {
        GameManager.instance.unPauseState();
        if(SceneManager.GetActiveScene().name != "TheHub")
            StartCoroutine(GameManager.instance.loadScene("TheHub"));
    }

    public void ReturnToMainMenu()
    {
        GameManager.instance.BackToMainMenuConfirm();
    }

    public void ConfirmBackToMenu()
    {
       GameManager.instance.MainMenu();
    }

    public void DenyBackToMenu()
    {
        GameManager.instance.activeMenu.SetActive(false);
        GameManager.instance.activeMenu = null;
        GameManager.instance.PauseMenu();
    }

    public void ExitBackToMenu()
    {
        if(GameManager.instance.prevMenu == GameManager.instance.pauseMenu)
        {
            GameManager.instance.activeMenu.SetActive(false);
            GameManager.instance.activeMenu = null;
            GameManager.instance.PauseMenu();
        }
        else if(GameManager.instance.prevMenu == GameManager.instance.mainMenu)
        {
            GameManager.instance.MainMenu();
        }
    }
    public void NewGame()
    {
        GameManager.instance.unPauseState();
        if (SceneManager.GetActiveScene().name != "TheHub")
            StartCoroutine(GameManager.instance.loadScene("TheHub"));
    }

    public void Continue()
    {

    }

    public void Options()
    {
        GameManager.instance.OptionsMenu();
    }

    public void Credits()
    {
        GameManager.instance.CreditsMenu();
    }

}
