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
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void Quit()
    {
        Application.Quit();
    }
    public void respawnPlayer()
    {
        GameManager.instance.unPauseState();
        GameManager.instance.playerScript.spawnPlayer();
        GameManager.instance.UpdatePlayerHP();
    }

    public void ReturnToLobby()
    {
        GameManager.instance.unPauseState();
        if(SceneManager.GetActiveScene().name != "TheHub")
            StartCoroutine(loadHub());
    }

    IEnumerator loadHub() 
    {
        Animator anim = GameObject.FindGameObjectWithTag("Transition").GetComponent<Animator>();

        anim.SetTrigger("Transition");

        yield return new WaitForSeconds(3f);

        anim.ResetTrigger("Transition");
        SceneManager.LoadScene("TheHub");
    }
}
