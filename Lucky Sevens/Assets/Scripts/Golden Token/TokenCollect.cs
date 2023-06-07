using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TokenCollect : MonoBehaviour
{
    [SerializeField] float spinSpeed;
    [SerializeField] int level;
    int spinMod;
    float youWinDelay;
    bool collected = false;
    private void Update()
    {
        gameObject.transform.Rotate(spinSpeed * Time.deltaTime, 0, 0);
    }

    public void SetSpinSpeed(float speed)
    {
        spinSpeed = speed;
    }

    public void SetLevel(int num)
    {
        level = num;
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

        if (other.CompareTag("Player") && !collected)
        {
            collected = true;
            SpeedUp();
            GameManager.instance.SetLatestLevel(level);
            StartCoroutine(GameManager.instance.youWin(youWinDelay));
        }
    }
}
