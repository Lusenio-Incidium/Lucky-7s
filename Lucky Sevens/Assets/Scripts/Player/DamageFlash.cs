using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageFlash : MonoBehaviour
{
    [SerializeField] CanvasGroup leftFlash;
    [SerializeField] CanvasGroup rightFlash;
    [SerializeField] CanvasGroup topFlash;
    [SerializeField] CanvasGroup bottomFlash;
    [SerializeField][Range(0.01f, 0.9f)] float modifier; 

    public void leftHit(float dmg)
    {
        leftFlash.alpha = dmg * modifier;
        StartCoroutine(removeFlash());
    }

    public void rightHit(float dmg)
    {
        rightFlash.alpha = dmg * modifier;
        StartCoroutine(removeFlash());
    }

    public void topHit(float dmg)
    {
        topFlash.alpha = dmg * modifier;
        StartCoroutine(removeFlash());
    }

    public void bottomHit(float dmg)
    {
        bottomFlash.alpha = dmg * modifier;
        StartCoroutine(removeFlash());
    }

    IEnumerator removeFlash() 
    {
        yield return new WaitForSeconds(1.5f);
        leftFlash.alpha = 0;
        rightFlash.alpha = 0;
        topFlash.alpha = 0;
        bottomFlash.alpha = 0;
    }

}
