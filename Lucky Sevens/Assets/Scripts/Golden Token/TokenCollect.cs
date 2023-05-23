using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

public class TokenCollect : MonoBehaviour
{
    [SerializeField] float spinSpeed;
    int spinMod;
    float youWinDelay;
    private void Update()
    {
        gameObject.transform.Rotate(spinSpeed * Time.deltaTime, 0, 0);
    }

    public void SetSpinSpeed(float speed)
    {
        spinSpeed = speed;
    }
    public void SetMod(int mod)
    {
        spinMod = mod;
    }
    public void SetYouWinDelay(float delay)
    {
        youWinDelay = delay;
    }
    public void SpeedUp()
    {
        spinSpeed *= spinMod;
    }
    public void SpeedDown()
    {
        spinSpeed /= spinMod;
    }
    private void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("Player"))
        {
            SpeedUp();
            StartCoroutine(GameManager.instance.youWin(youWinDelay));

        }
    }
}
