using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class LevelWarp : MonoBehaviour
{
    [SerializeField] string WarpToScene;
    [SerializeField] int loadTime;

    private Animator anim;

    private void Awake()
    {
        anim = GameObject.FindGameObjectWithTag("Transition").GetComponent<Animator>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
        {
            return;
        }
        StartCoroutine(loadScene());
    }

    IEnumerator loadScene() 
    {
        GameManager.instance.loadingScreen.SetActive(true);
        anim.SetTrigger("Transition");

        yield return new WaitForSeconds(3f);

        anim.ResetTrigger("Transition");
        SceneManager.LoadScene(WarpToScene);
    }
}
