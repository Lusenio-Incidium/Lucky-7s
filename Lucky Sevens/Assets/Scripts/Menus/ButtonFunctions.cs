using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonFunctions : MonoBehaviour
{
    public void resume()
    {
        GameManager.instance.unPauseState();
        GameManager.instance.playSoundAudSource.PlayOneShot(GameManager.instance.buttonSoundAud, GameManager.instance.playerScript.GetJumpVol());

    }
    public void restart()
    {
        GameManager.instance.didRestart = true;
        
        //GameManager.instance.playerScript.SetisDead(false);
        GameManager.instance.ReEnableDisplay();
        GameManager.instance.playSoundAudSource.PlayOneShot(GameManager.instance.buttonSoundAud, GameManager.instance.playerScript.GetJumpVol());
        GameManager.instance.unPauseState();
        StartCoroutine(GameManager.instance.loadScene(SceneManager.GetActiveScene().name));
        GameManager.instance.gunSystem.restartGun();
        GameManager.instance.playerAmmo = GameManager.instance.playerAmmoOrign;
        GameManager.instance.playerScript.playerHeal((int)GameManager.instance.playerScript.GetMaxHP());
        
    }

    public void Quit()
    {
        MainMenuManager.instance.playSoundAudSource.PlayOneShot(MainMenuManager.instance.buttonPressAud, MainMenuManager.instance.SFXVolume / 10f);
        Application.Quit();
    }
    public void respawnPlayer()
    {
        //GameManager.instance.playerScript.SetisDead(false);
        GameManager.instance.ReEnableDisplay();
        GameManager.instance.playSoundAudSource.PlayOneShot(GameManager.instance.buttonSoundAud, GameManager.instance.playerScript.GetJumpVol());
        GameManager.instance.unPauseState();
        GameManager.instance.playerScript.spawnPlayer();
       
    }

    public void EndGame() 
    {
        GameManager.instance.startGameOver();
    }

    public void ReturnToLobby()
    {
        GameManager.instance.playerAnim.SetBool("dead", false);
        GameManager.instance.ReEnableDisplay();
        GameManager.instance.playSoundAudSource.PlayOneShot(GameManager.instance.buttonSoundAud, GameManager.instance.playerScript.GetJumpVol());
        GameManager.instance.unPauseState();
        StartCoroutine(GameManager.instance.loadScene("TheHub"));
        GameManager.instance.onLoad();
    }

    public void ReturnToMainMenu()
    {
        GameManager.instance.playSoundAudSource.PlayOneShot(GameManager.instance.buttonSoundAud, GameManager.instance.playerScript.GetJumpVol());
        GameManager.instance.BackToMainMenuConfirm();
    }

    public void ConfirmBackToMenu()
    {
        GameManager.instance.playSoundAudSource.PlayOneShot(GameManager.instance.buttonSoundAud, GameManager.instance.playerScript.GetJumpVol());
        GameManager.instance.unPauseState();
        if (SceneManager.GetActiveScene().name != "MainMenu")
        {   
           StartCoroutine(GameManager.instance.loadScene("MainMenu"));
        }
    }

    public void DenyBackToMenu()
    {
        GameManager.instance.playSoundAudSource.PlayOneShot(GameManager.instance.buttonSoundAud, GameManager.instance.playerScript.GetJumpVol());
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
            MainMenuManager.instance.playSoundAudSource.PlayOneShot(MainMenuManager.instance.buttonPressAud, MainMenuManager.instance.SFXVolume / 100f);
            PlayerPrefs.SetFloat("MusicVolume",MainMenuManager.instance.musicVolume);
            PlayerPrefs.SetFloat("SFXVolume", MainMenuManager.instance.SFXVolume);
            PlayerPrefs.SetFloat("Sensitivity", MainMenuManager.instance.sensitivity);
            PlayerPrefs.Save();
        }
        else
        {
            GameManager.instance.playSoundAudSource.PlayOneShot(GameManager.instance.buttonSoundAud, GameManager.instance.playerScript.GetJumpVol());
            GameManager.instance.activeMenu.SetActive(false);
            GameManager.instance.PauseMenu();

            PlayerPrefs.SetFloat("MusicVolume", GameManager.instance.musicVol);
            PlayerPrefs.SetFloat("SFXVolume", GameManager.instance.sfxVol);
            PlayerPrefs.SetFloat("Sensitivity", GameManager.instance.sensitivity);
            PlayerPrefs.Save();
        }
    }
    public void NewGame()
    {
        if (MainMenuManager.instance.mainEasy || MainMenuManager.instance.mainMedium || MainMenuManager.instance.mainHard)
        {
            MainMenuManager.instance.activeMenu.SetActive(false);
            MainMenuManager.instance.fadeOut();
            if (SceneManager.GetActiveScene().name != "Tutorial")
                StartCoroutine(MainMenuManager.instance.loadScene("Tutorial"));
            MainMenuManager.instance.playSoundAudSource.PlayOneShot(MainMenuManager.instance.buttonPressAud, MainMenuManager.instance.SFXVolume / 10f);
        }
    }


    public void Options()
    {
        if (SceneManager.GetActiveScene().name == "MainMenu")
        {
            MainMenuManager.instance.OptionsMenu();
            MainMenuManager.instance.playSoundAudSource.PlayOneShot(MainMenuManager.instance.buttonPressAud, MainMenuManager.instance.SFXVolume / 10f);
        }
        else
        {
            GameManager.instance.InGameOptions();
            GameManager.instance.playSoundAudSource.PlayOneShot(GameManager.instance.buttonSoundAud, GameManager.instance.playerScript.GetJumpVol());
        }
    }

    public void Credits()
    {
        MainMenuManager.instance.CreditsMenu();
        MainMenuManager.instance.playSoundAudSource.PlayOneShot(MainMenuManager.instance.buttonPressAud, MainMenuManager.instance.SFXVolume / 10f);
    }
    public void Easy()
    {
        GameManager.instance.easy = true;
        GameManager.instance.medium = false;
        GameManager.instance.hard = false;
        GameManager.instance.playSoundAudSource.PlayOneShot(GameManager.instance.buttonSoundAud, GameManager.instance.playerScript.GetJumpVol());
    }
    public void Medium()
    {
        GameManager.instance.easy = false;
        GameManager.instance.medium = true;
        GameManager.instance.hard = false;
        GameManager.instance.playSoundAudSource.PlayOneShot(GameManager.instance.buttonSoundAud, GameManager.instance.playerScript.GetJumpVol());
    }
    public void Hard()
    {
        GameManager.instance.easy = false;
        GameManager.instance.medium = false;
        GameManager.instance.hard = true;
        GameManager.instance.playSoundAudSource.PlayOneShot(GameManager.instance.buttonSoundAud, GameManager.instance.playerScript.GetJumpVol());
    }
    public void DifficultyButton()
    {
        GameManager.instance.DifficultyMenu();
        GameManager.instance.playSoundAudSource.PlayOneShot(GameManager.instance.buttonSoundAud, GameManager.instance.playerScript.GetJumpVol());
    }
    public void ReturnToLoseScreen()
    {
        GameManager.instance.ReturnToLoseScreen();
        GameManager.instance.playSoundAudSource.PlayOneShot(GameManager.instance.buttonSoundAud, GameManager.instance.playerScript.GetJumpVol());
    }
    public void MainEasy()
    {
        MainMenuManager.instance.mainEasy = true;
        MainMenuManager.instance.mainMedium = false;
        MainMenuManager.instance.mainHard = false;
        MainMenuManager.instance.playSoundAudSource.PlayOneShot(MainMenuManager.instance.buttonPressAud, MainMenuManager.instance.SFXVolume / 10f);
    }
    public void MainMedium()
    {
        MainMenuManager.instance.mainEasy = false;
        MainMenuManager.instance.mainMedium = true;
        MainMenuManager.instance.mainHard = false;
        MainMenuManager.instance.playSoundAudSource.PlayOneShot(MainMenuManager.instance.buttonPressAud, MainMenuManager.instance.SFXVolume / 10f);
    }
    public void MainHard()
    {
        MainMenuManager.instance.mainEasy = false;
        MainMenuManager.instance.mainMedium = false;
        MainMenuManager.instance.mainHard = true;
        MainMenuManager.instance.playSoundAudSource.PlayOneShot(MainMenuManager.instance.buttonPressAud, MainMenuManager.instance.SFXVolume / 10f);
    }
    public void DisplayDifficultyMenu()
    {
        MainMenuManager.instance.DifficultyMenu();
        MainMenuManager.instance.playSoundAudSource.PlayOneShot(MainMenuManager.instance.buttonPressAud, MainMenuManager.instance.SFXVolume / 10f);
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
                MainMenuManager.instance.playSoundAudSource.PlayOneShot(MainMenuManager.instance.buttonPressAud, MainMenuManager.instance.SFXVolume / 10f);
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
                GameManager.instance.playSoundAudSource.PlayOneShot(GameManager.instance.buttonSoundAud, GameManager.instance.playerScript.GetJumpVol());
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
                MainMenuManager.instance.playSoundAudSource.PlayOneShot(MainMenuManager.instance.buttonPressAud, MainMenuManager.instance.SFXVolume / 10f);
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
                GameManager.instance.playSoundAudSource.PlayOneShot(GameManager.instance.buttonSoundAud, GameManager.instance.playerScript.GetJumpVol());
            }
        }
    }

    public void IncreaseSFXVolume()
    {
        if (SceneManager.GetActiveScene().name == "MainMenu")
        {
            if (MainMenuManager.instance.SFXVolume != 100)
            {
                MainMenuManager.instance.SFXVolume += 1.0f;
                MainMenuManager.instance.SFXvolumetext.text = MainMenuManager.instance.SFXVolume.ToString();
                MainMenuManager.instance.SFXVolumeBar.fillAmount = MainMenuManager.instance.SFXVolume / 100f;
                MainMenuManager.instance.playSoundAudSource.PlayOneShot(MainMenuManager.instance.buttonPressAud, MainMenuManager.instance.SFXVolume / 100f);
            }
        }
        else
        {
            if (GameManager.instance.sfxVol != 100)
            {
                GameManager.instance.sfxVol += 1.0f;
                GameManager.instance.SFXText.text = GameManager.instance.sfxVol.ToString();
                GameManager.instance.SFXBar.fillAmount = GameManager.instance.sfxVol / 100f;
                GameManager.instance.playerScript.UpdateSFX(GameManager.instance.sfxVol);
                GameManager.instance.playSoundAudSource.PlayOneShot(GameManager.instance.buttonSoundAud, GameManager.instance.playerScript.GetJumpVol());
            }
        }
    }

    public void DecreaseSFXVolume()
    {
        if (SceneManager.GetActiveScene().name == "MainMenu")
        {
            if (MainMenuManager.instance.SFXVolume != 0)
            {
                MainMenuManager.instance.SFXVolume -= 1.0f;
                MainMenuManager.instance.SFXvolumetext.text = MainMenuManager.instance.SFXVolume.ToString();
                MainMenuManager.instance.SFXVolumeBar.fillAmount = MainMenuManager.instance.SFXVolume / 100f;
                MainMenuManager.instance.playSoundAudSource.PlayOneShot(MainMenuManager.instance.buttonPressAud, MainMenuManager.instance.SFXVolume / 100f);
            }
        }
        else
        {
            if (GameManager.instance.sfxVol != 0)
            {
                GameManager.instance.sfxVol -= 1.0f;
                GameManager.instance.SFXText.text = GameManager.instance.sfxVol.ToString();
                GameManager.instance.SFXBar.fillAmount = GameManager.instance.sfxVol / 100f;
                GameManager.instance.playerScript.UpdateSFX(GameManager.instance.sfxVol);
                GameManager.instance.playSoundAudSource.PlayOneShot(GameManager.instance.buttonSoundAud, GameManager.instance.playerScript.GetJumpVol());
            }
        }
    }

    public void IncreaseMusicVolume()
    {
        if (SceneManager.GetActiveScene().name == "MainMenu")
        {
            if (MainMenuManager.instance.musicVolume != 100)
            {
                MainMenuManager.instance.musicVolume += 1.0f;
                MainMenuManager.instance.musicvolumetext.text = MainMenuManager.instance.musicVolume.ToString();
                MainMenuManager.instance.musicVolumeBar.fillAmount = MainMenuManager.instance.musicVolume / 100f;
                MainMenuManager.instance.playSoundAudSource.PlayOneShot(MainMenuManager.instance.buttonPressAud, MainMenuManager.instance.SFXVolume / 100f);
            }
        }
        else
        {
            if (GameManager.instance.musicVol != 100)
            {
                GameManager.instance.musicVol += 1.0f;
                GameManager.instance.musicText.text = GameManager.instance.musicVol.ToString();
                GameManager.instance.musicBar.fillAmount = GameManager.instance.musicVol / 100f;
                GameManager.instance.playerScript.UpdateMusic(GameManager.instance.musicVol);
                GameManager.instance.playSoundAudSource.PlayOneShot(GameManager.instance.buttonSoundAud, GameManager.instance.playerScript.GetJumpVol());
            }
        }
    }

    public void DecreaseMusicVolume()
    {
        if (SceneManager.GetActiveScene().name == "MainMenu")
        {
            if (MainMenuManager.instance.musicVolume != 0)
            {
                MainMenuManager.instance.musicVolume -= 1.0f;
                MainMenuManager.instance.musicvolumetext.text = MainMenuManager.instance.musicVolume.ToString();
                MainMenuManager.instance.musicVolumeBar.fillAmount = MainMenuManager.instance.musicVolume / 100f;
                MainMenuManager.instance.playSoundAudSource.PlayOneShot(MainMenuManager.instance.buttonPressAud, MainMenuManager.instance.SFXVolume / 100f);
            }
        }
        else
        {
            if (GameManager.instance.musicVol != 0)
            {
                GameManager.instance.musicVol -= 1.0f;
                GameManager.instance.musicText.text = GameManager.instance.musicVol.ToString();
                GameManager.instance.musicBar.fillAmount = GameManager.instance.musicVol / 100f;
                GameManager.instance.playerScript.UpdateMusic(GameManager.instance.musicVol);
                GameManager.instance.playSoundAudSource.PlayOneShot(GameManager.instance.buttonSoundAud, GameManager.instance.playerScript.GetJumpVol());
            }
        }
        
    }

    public void PlaySound()
    {
        if (SceneManager.GetActiveScene().name == "MainMenu")
        {
            MainMenuManager.instance.playSoundAudSource.PlayOneShot(MainMenuManager.instance.playSoundAud, MainMenuManager.instance.SFXVolume / 100f);
        }
        else
        {
            GameManager.instance.playSoundAudSource.PlayOneShot(GameManager.instance.playSoundAud, GameManager.instance.playerScript.GetJumpVol());
        }
    }
}
