using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class MusicPlus : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public enum ButtonType 
    {
        increaseMusic,
        decreaseMusic,
        increaseSFX,
        decreaseSFX,
        increaseSens,
        decreaseSens
    }

    [SerializeField] ButtonType button;



    bool isDown;
    bool isMusicIncreasing;
    bool isMusicDecreasing;
    bool isSFXIncreasing;
    bool isSFXDecreasing;
    bool isSensIncreasing;
    bool isSensDecreasing;
    void Update()
    {
        if (isDown) 
        {
            if (!isMusicIncreasing && button == ButtonType.increaseMusic)
                StartCoroutine(IncreaseMusicVolume());
            else if (!isMusicDecreasing && button == ButtonType.decreaseMusic)
                StartCoroutine(DecreaseMusicVolume());
            else if (!isSFXIncreasing && button == ButtonType.increaseSFX)
                StartCoroutine(IncreaseSFXVolume());
            else if (!isSFXDecreasing && button == ButtonType.decreaseSFX)
                StartCoroutine(DecreaseSFXVolume());
            else if (!isSensIncreasing && button == ButtonType.increaseSens)
                StartCoroutine(IncreaseSensitivity());
            else if (!isSensDecreasing && button == ButtonType.decreaseSens)
                StartCoroutine(DecreaseSensitivity());
        }
    }

    void IPointerDownHandler.OnPointerDown(PointerEventData eventData) 
    {
        isDown = true;
    }

    void IPointerUpHandler.OnPointerUp(PointerEventData eventData) 
    {
        isDown = false;
    }


    IEnumerator IncreaseMusicVolume()
    {
        isMusicIncreasing = true;
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
        yield return new WaitForSecondsRealtime(0.1f);
        isMusicIncreasing = false;
    }

    IEnumerator DecreaseMusicVolume()
    {
        isMusicDecreasing = true;
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
        yield return new WaitForSecondsRealtime(0.1f);
        isMusicDecreasing = false;
    }

    IEnumerator IncreaseSFXVolume()
    {
        isSFXIncreasing = true;
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
        yield return new WaitForSecondsRealtime(0.1f);
        isSFXIncreasing = false;
    }

    IEnumerator DecreaseSFXVolume()
    {
        isSFXDecreasing = true;
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
        yield return new WaitForSecondsRealtime(0.1f);
        isSFXDecreasing = false;
    }

    IEnumerator IncreaseSensitivity()
    {
        isSensIncreasing = true;
        if (SceneManager.GetActiveScene().name == "MainMenu")
        {
            if (MainMenuManager.instance.sensitivity < 100)
            {
                MainMenuManager.instance.sensitivity += 1;
                MainMenuManager.instance.sensitivitytext.text = MainMenuManager.instance.sensitivity.ToString();
                MainMenuManager.instance.sensitivityBar.fillAmount = MainMenuManager.instance.sensitivity / 100;
                MainMenuManager.instance.playSoundAudSource.PlayOneShot(MainMenuManager.instance.buttonPressAud, MainMenuManager.instance.SFXVolume / 10f);
            }
        }
        else
        {
            if (GameManager.instance.sensitivity < 100)
            {
                GameManager.instance.sensitivity += 1;
                GameManager.instance.sensitivityText.text = GameManager.instance.sensitivity.ToString();
                GameManager.instance.sensitivityBar.fillAmount = GameManager.instance.sensitivity / 100;
                GameManager.instance.playerCam.GetComponent<CameraController>().UpdateSensitivity(GameManager.instance.sensitivity);
                GameManager.instance.playSoundAudSource.PlayOneShot(GameManager.instance.buttonSoundAud, GameManager.instance.playerScript.GetJumpVol());
            }
        }
        yield return new WaitForSecondsRealtime(0.1f);
        isSensIncreasing = false;
    }

    IEnumerator DecreaseSensitivity()
    {
        isSensDecreasing = true;
        if (SceneManager.GetActiveScene().name == "MainMenu")
        {
            if (MainMenuManager.instance.sensitivity > 1)
            {
                MainMenuManager.instance.sensitivity -= 1;
                MainMenuManager.instance.sensitivitytext.text = MainMenuManager.instance.sensitivity.ToString();
                MainMenuManager.instance.sensitivityBar.fillAmount = MainMenuManager.instance.sensitivity / 100;
                MainMenuManager.instance.playSoundAudSource.PlayOneShot(MainMenuManager.instance.buttonPressAud, MainMenuManager.instance.SFXVolume / 10f);
            }
        }
        else
        {
            if (GameManager.instance.sensitivity > 1)
            {
                GameManager.instance.sensitivity -= 1;
                GameManager.instance.sensitivityText.text = GameManager.instance.sensitivity.ToString();
                GameManager.instance.sensitivityBar.fillAmount = GameManager.instance.sensitivity / 100;
                GameManager.instance.playerCam.GetComponent<CameraController>().UpdateSensitivity(GameManager.instance.sensitivity);
                GameManager.instance.playSoundAudSource.PlayOneShot(GameManager.instance.buttonSoundAud, GameManager.instance.playerScript.GetJumpVol());
            }
        }
        yield return new WaitForSecondsRealtime(0.1f);
        isSensDecreasing = false;
    }
}
