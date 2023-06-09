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
    bool finalToken;
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

    public void SetFinalToken(bool ft)
    {
        finalToken = ft;
    }
    private void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("Player") && !collected)
        {
            collected = true;
            GameManager.instance.playerScript.Invincible(true);
            SpeedUp();
            GameManager.instance.SetLatestLevel(level);
            if(GameManager.instance.GetLastestLevel() == 3)
            {
                GameManager.instance.completed = true;
            }
            StartCoroutine(GameManager.instance.WaitForFall());
            GameManager.instance.playerAmmoOrign = GameManager.instance.playerAmmo + 100;
            GameManager.instance.playerAmmo += 100;
            GameManager.instance.SetWin();
        }
    }
}
