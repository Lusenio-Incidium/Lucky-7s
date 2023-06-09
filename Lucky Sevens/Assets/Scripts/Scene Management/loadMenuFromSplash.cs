using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class loadMenuFromSplash : MonoBehaviour
{
    CanvasGroup alpha;
    void Start()
    {
        alpha = GetComponent<CanvasGroup>();
        StartCoroutine(fadeIn());
    }

    IEnumerator fadeIn() 
    {
        alpha.alpha -= 0.01f;
        
        if(alpha.alpha > 0) 
        {
            yield return new WaitForSeconds(0.01f);
            StartCoroutine(fadeIn());
        }
        else 
        {
            yield return new WaitForSeconds(2f);
            StartCoroutine(fadeOut());
        }
    }

    IEnumerator fadeOut()
    {
        alpha.alpha += 0.01f;
        
        if (alpha.alpha < 1)
        {
            yield return new WaitForSeconds(0.01f);
            StartCoroutine(fadeOut());
        }
        else
        {
            yield return new WaitForSeconds(1.5f);
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }
}
