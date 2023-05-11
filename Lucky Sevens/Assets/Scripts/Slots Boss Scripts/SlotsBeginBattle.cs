using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlotsBeginBattle : MonoBehaviour
{
    bool hit = false;
    [SerializeField] GameObject DestroyOnBegin;
    [SerializeField] GameObject ArenaLight;
    [SerializeField] Animator startAnimation;
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player") || hit)
        {
            return;
        }
        hit = true;
        Destroy(DestroyOnBegin);
        ArenaLight.SetActive(true);
        startAnimation.SetTrigger("LockArena");
    }
}
