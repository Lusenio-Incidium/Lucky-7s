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
        GameManager.instance.unPauseState();
        Debug.Log("Loading Main Menu");
        if (SceneManager.GetActiveScene().name != "MainMenu")
        {   
           StartCoroutine(GameManager.instance.loadScene("MainMenu"));
        }
    }

    public void DenyBackToMenu()
    {
        GameManager.instance.activeMenu.SetActive(false);
        GameManager.instance.activeMenu = null;
        GameManager.instance.PauseMenu();
    }

    public void ExitBackToMenu()
    {
        MainMenuManager.instance.activeMenu.SetActive(false);
        MainMenuManager.instance.activeMenu = MainMenuManager.instance.mainMenu;
        MainMenuManager.instance.activeMenu.SetActive(true);
    }
    public void NewGame()
    {
        if (MainMenuManager.instance.mainEasy || MainMenuManager.instance.mainMedium || MainMenuManager.instance.mainHard)
        {
            MainMenuManager.instance.activeMenu.SetActive(false);
            if (SceneManager.GetActiveScene().name != "Tutorial")
                StartCoroutine(MainMenuManager.instance.loadScene("Tutorial"));
        }
    }

    public void Continue()
    {

    }

    public void Options()
    {
        MainMenuManager.instance.OptionsMenu();
    }

    public void Credits()
    {
        MainMenuManager.instance.CreditsMenu();
    }
    public void Easy()
    {
        GameManager.instance.easy = true;
        GameManager.instance.medium = false;
        GameManager.instance.hard = false;
    }
    public void Medium()
    {
        GameManager.instance.easy = false;
        GameManager.instance.medium = true;
        GameManager.instance.hard = false;
    }
    public void Hard()
    {
        GameManager.instance.easy = false;
        GameManager.instance.medium = false;
        GameManager.instance.hard = true;
    }
    public void DifficultyButton()
    {
        GameManager.instance.DifficultyMenu();
    }
    public void ReturnToLoseScreen()
    {
        GameManager.instance.ReturnToLoseScreen();
    }
    public void MainEasy()
    {
        MainMenuManager.instance.mainEasy = true;
        MainMenuManager.instance.mainMedium = false;
        MainMenuManager.instance.mainHard = false;
    }
    public void MainMedium()
    {
        MainMenuManager.instance.mainEasy = false;
        MainMenuManager.instance.mainMedium = true;
        MainMenuManager.instance.mainHard = false;
    }
    public void MainHard()
    {
        MainMenuManager.instance.mainEasy = false;
        MainMenuManager.instance.mainMedium = false;
        MainMenuManager.instance.mainHard = true;
    }
    public void DisplayDifficultyMenu()
    {
        MainMenuManager.instance.DifficultyMenu();
    }

    public void IncreaseSensitivity()
    {
        if (MainMenuManager.instance.sensitivity != 10)
        {
            MainMenuManager.instance.sensitivity += 0.5f;
            MainMenuManager.instance.sensitivitytext.text = MainMenuManager.instance.sensitivity.ToString();
            MainMenuManager.instance.sensitivityBar.fillAmount = MainMenuManager.instance.sensitivity / 10f;
        }
    }

    public void DecreaseSensitivity()
    {
        if (MainMenuManager.instance.sensitivity != 1)
        {
            MainMenuManager.instance.sensitivity -= 0.5f;
            MainMenuManager.instance.sensitivitytext.text = MainMenuManager.instance.sensitivity.ToString();
            MainMenuManager.instance.sensitivityBar.fillAmount = MainMenuManager.instance.sensitivity / 10f;
        }
    }

    public void IncreaseSFXVolume()
    {
        if (MainMenuManager.instance.SFXVolume != 10)
        {
            MainMenuManager.instance.SFXVolume += 0.5f;
            MainMenuManager.instance.SFXvolumetext.text = MainMenuManager.instance.SFXVolume.ToString();
            MainMenuManager.instance.SFXVolumeBar.fillAmount = MainMenuManager.instance.SFXVolume / 10f;
        }
    }

    public void DecreaseSFXVolume()
    {
        if (MainMenuManager.instance.SFXVolume != 0)
        {
       
            MainMenuManager.instance.SFXVolume -= 0.5f;
            MainMenuManager.instance.SFXvolumetext.text = MainMenuManager.instance.SFXVolume.ToString();
            MainMenuManager.instance.SFXVolumeBar.fillAmount = MainMenuManager.instance.SFXVolume / 10f;
        }
    }

    public void IncreaseMusicVolume()
    {
        if (MainMenuManager.instance.musicVolume != 10)
        {
            MainMenuManager.instance.musicVolume += 0.5f;
            MainMenuManager.instance.musicvolumetext.text = MainMenuManager.instance.musicVolume.ToString();
            MainMenuManager.instance.musicVolumeBar.fillAmount = MainMenuManager.instance.musicVolume / 10f;
        }
    }

    public void DecreaseMusicVolume()
    {
        if (MainMenuManager.instance.musicVolume != 0)
        {
            MainMenuManager.instance.musicVolume -= 0.5f;
            MainMenuManager.instance.musicvolumetext.text = MainMenuManager.instance.musicVolume.ToString();
            MainMenuManager.instance.musicVolumeBar.fillAmount = MainMenuManager.instance.musicVolume / 10f;
        }
    }

}
