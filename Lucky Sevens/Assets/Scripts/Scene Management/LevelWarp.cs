using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelWarp : MonoBehaviour
{
    [SerializeField] string WarpToScene;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
        {
            return;
        }
        StartCoroutine(GameManager.instance.loadScene(WarpToScene));
        GameManager.instance.onLoad();
    }

}
