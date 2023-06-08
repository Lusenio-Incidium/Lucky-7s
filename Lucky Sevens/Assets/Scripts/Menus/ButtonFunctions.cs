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
        if (SceneManager.GetActiveScene().name == "MainMenu")
        {
            MainMenuManager.instance.activeMenu.SetActive(false);
            MainMenuManager.instance.activeMenu = MainMenuManager.instance.mainMenu;
            MainMenuManager.instance.activeMenu.SetActive(true);
        }
        else
        {
            GameManager.instance.activeMenu.SetActive(false);
            GameManager.instance.PauseMenu();
        }
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
        if (SceneManager.GetActiveScene().name == "MainMenu")
        {
            MainMenuManager.instance.OptionsMenu();
        }
        else
        {
            GameManager.instance.InGameOptions();
        }
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
        if (SceneManager.GetActiveScene().name == "MainMenu")
        {
            if (MainMenuManager.instance.sensitivity != 10)
            {
                MainMenuManager.instance.sensitivity += 0.5f;
                MainMenuManager.instance.sensitivitytext.text = MainMenuManager.instance.sensitivity.ToString();
                MainMenuManager.instance.sensitivityBar.fillAmount = MainMenuManager.instance.sensitivity / 10f;
            }
        }
        else
        {
            if (GameManager.instance.sensitivity != 10)
            {
                GameManager.instance.sensitivity += 0.5f;
                GameManager.instance.sensitivityText.text = GameManager.instance.sensitivity.ToString();
                GameManager.instance.sensitivityBar.fillAmount = GameManager.instance.sensitivity / 10f;
                GameManager.instance.playerCam.GetComponent<CameraController>().UpdateSensitivity(GameManager.instance.sensitivity);
            }
        }
    }

    public void DecreaseSensitivity()
    {
        if (SceneManager.GetActiveScene().name == "MainMenu")
        {
            if (MainMenuManager.instance.sensitivity != 1)
            {
                MainMenuManager.instance.sensitivity -= 0.5f;
                MainMenuManager.instance.sensitivitytext.text = MainMenuManager.instance.sensitivity.ToString();
                MainMenuManager.instance.sensitivityBar.fillAmount = MainMenuManager.instance.sensitivity / 10f;
            }
        }
        else
        {
            if (GameManager.instance.sensitivity != 1)
            {
                GameManager.instance.sensitivity -= 0.5f;
                GameManager.instance.sensitivityText.text = GameManager.instance.sensitivity.ToString();
                GameManager.instance.sensitivityBar.fillAmount = GameManager.instance.sensitivity / 10f;
                GameManager.instance.playerCam.GetComponent<CameraController>().UpdateSensitivity(GameManager.instance.sensitivity);
            }
        }
    }

    public void IncreaseSFXVolume()
    {
        if (SceneManager.GetActiveScene().name == "MainMenu")
        {
            if (MainMenuManager.instance.SFXVolume != 10)
            {
                MainMenuManager.instance.SFXVolume += 0.5f;
                MainMenuManager.instance.SFXvolumetext.text = MainMenuManager.instance.SFXVolume.ToString();
                MainMenuManager.instance.SFXVolumeBar.fillAmount = MainMenuManager.instance.SFXVolume / 10f;
            }
        }
        else
        {
            if (GameManager.instance.sfxVol != 10)
            {
                GameManager.instance.sfxVol += 0.5f;
                GameManager.instance.SFXText.text = GameManager.instance.sfxVol.ToString();
                GameManager.instance.SFXBar.fillAmount = GameManager.instance.sfxVol / 10f;
                GameManager.instance.playerScript.UpdateSFX(GameManager.instance.sfxVol);
            }
        }
    }

    public void DecreaseSFXVolume()
    {
        if (SceneManager.GetActiveScene().name == "MainMenu")
        {
            if (MainMenuManager.instance.SFXVolume != 0)
            {

                MainMenuManager.instance.SFXVolume -= 0.5f;
                MainMenuManager.instance.SFXvolumetext.text = MainMenuManager.instance.SFXVolume.ToString();
                MainMenuManager.instance.SFXVolumeBar.fillAmount = MainMenuManager.instance.SFXVolume / 10f;
            }
        }
        else
        {
            if (GameManager.instance.sfxVol != 0)
            {
                GameManager.instance.sfxVol -= 0.5f;
                GameManager.instance.SFXText.text = GameManager.instance.sfxVol.ToString();
                GameManager.instance.SFXBar.fillAmount = GameManager.instance.sfxVol / 10f;
                GameManager.instance.playerScript.UpdateSFX(GameManager.instance.sfxVol);
            }
        }
    }

    public void IncreaseMusicVolume()
    {
        if (SceneManager.GetActiveScene().name == "MainMenu")
        {
            if (MainMenuManager.instance.musicVolume != 10)
            {
                MainMenuManager.instance.musicVolume += 0.5f;
                MainMenuManager.instance.musicvolumetext.text = MainMenuManager.instance.musicVolume.ToString();
                MainMenuManager.instance.musicVolumeBar.fillAmount = MainMenuManager.instance.musicVolume / 10f;
            }
        }
        else
        {
            if (GameManager.instance.musicVol != 10)
            {
                GameManager.instance.musicVol += 0.5f;
                GameManager.instance.musicText.text = GameManager.instance.musicVol.ToString();
                GameManager.instance.musicBar.fillAmount = GameManager.instance.musicVol / 10f;
                GameManager.instance.playerScript.UpdateMusic(GameManager.instance.musicVol);
            }
        }
    }

    public void DecreaseMusicVolume()
    {
        if (SceneManager.GetActiveScene().name == "MainMenu")
        {
            if (MainMenuManager.instance.musicVolume != 0)
            {
                MainMenuManager.instance.musicVolume -= 0.5f;
                MainMenuManager.instance.musicvolumetext.text = MainMenuManager.instance.musicVolume.ToString();
                MainMenuManager.instance.musicVolumeBar.fillAmount = MainMenuManager.instance.musicVolume / 10f;
            }
        }
        else
        {
            if (GameManager.instance.musicVol != 0)
            {
                GameManager.instance.musicVol -= 0.5f;
                GameManager.instance.musicText.text = GameManager.instance.musicVol.ToString();
                GameManager.instance.musicBar.fillAmount = GameManager.instance.musicVol / 10f;
                GameManager.instance.playerScript.UpdateMusic(GameManager.instance.musicVol);
            }
        }
    }

}
