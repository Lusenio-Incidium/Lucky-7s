using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneralButtons : MonoBehaviour, IBattle, IButtonTrigger, ICannonKey
{
    private enum Functions
    {
        None = 0,
        EnableOnce,
        DisableOnce,
        Enable,
        Disable
    }

    [SerializeField] GameObject[] affectedItems;
    [SerializeField] PhysicalButtonPress buttonAnimation;
    [SerializeField] bool popsUp;
    [SerializeField] int popUpTimer;

    [Header("Trigger Functions")]
    [SerializeField] Functions onButtonPress;
    [SerializeField] Functions onButtonRelease;
    [SerializeField] Functions onBattleBegin;
    [SerializeField] Functions onBattleEnd;
    [SerializeField] Functions onCannonDeath;
    bool pressed;
    // Update is called once per frame
    

    private void OnTriggerEnter(Collider other)
    {
        if (pressed)
        {
            return;
        }
        if (other.CompareTag("Player"))
        {
            foreach(GameObject affectedObject in affectedItems)
            {
                IButtonTrigger trigger = affectedObject.GetComponent<IButtonTrigger>();
                if (trigger != null)
                {
                    trigger.OnButtonPress();
                }
                else
                {
                    Debug.LogWarning("GeneralButton on " + gameObject.name + " has item in Affected Items which does not have IButtonTrigger: " + affectedObject.name);
                }
            }
            if (popsUp)
            {
                StartCoroutine(timer());
            }
            pressed = true;
        }
    }
    private IEnumerator timer()
    {
        yield return new WaitForSeconds(popUpTimer);
        Arm();
    }
    private void Arm()
    {
        foreach (GameObject affectedObject in affectedItems)
        {
            IButtonTrigger trigger = affectedObject.GetComponent<IButtonTrigger>();
            if (trigger != null)
            {
                trigger.OnButtonRelease();
            }
            else
            {
                Debug.LogWarning("GeneralButton on " + gameObject.name + " has item in Affected Items which does not have IButtonTrigger: " + affectedObject.name);
            }

        }
        pressed = false;
        if (buttonAnimation != null)
        {
            buttonAnimation.Reset();
        }
    }

    private void Disarm()
    {
        pressed = true;
        if(buttonAnimation != null)
        {
            buttonAnimation.Disarm();
        }
    }
    public void OnButtonPress()
    {
        switch (onButtonPress)
        {
            case Functions.None:
                return;
            case Functions.EnableOnce:
                onButtonPress = Functions.None;
                Arm();
                return;
            case Functions.Enable:
                Arm();
                return;
            case Functions.DisableOnce:
                onButtonPress = Functions.None;
                Disarm();
                return;
            case Functions.Disable:
                Disarm();
                return;
        }
    }

    public void OnButtonRelease()
    {
        switch (onButtonRelease)
        {
            case Functions.None:
                return;
            case Functions.EnableOnce:
                onButtonRelease = Functions.None;
                Arm();
                return;
            case Functions.Enable:
                Arm();
                return;
            case Functions.DisableOnce:
                onButtonRelease = Functions.None;
                Disarm();
                return;
            case Functions.Disable:
                Disarm();
                return;
        }
    }

    public void OnBattleBegin()
    {
        switch (onBattleBegin)
        {
            case Functions.None:
                return;
            case Functions.EnableOnce:
                onBattleBegin = Functions.None;
                Arm();
                return;
            case Functions.Enable:
                Arm();
                return;
            case Functions.DisableOnce:
                onBattleBegin = Functions.None;
                Disarm();
                return;
            case Functions.Disable:
                Disarm();
                return;
        }
    }

    public void OnBattleEnd()
    {
        switch (onBattleEnd)
        {
            case Functions.None:
                return;
            case Functions.EnableOnce:
                onBattleEnd = Functions.None;
                Arm();
                return;
            case Functions.Enable:
                Arm();
                return;
            case Functions.DisableOnce:
                onBattleEnd = Functions.None;
                Disarm();
                return;
            case Functions.Disable:
                Disarm();
                return;
        }
    }

    public void OnCannonDeath()
    {
        switch (onCannonDeath)
        {
            case Functions.None:
                return;
            case Functions.EnableOnce:
                onCannonDeath = Functions.None;
                Arm();
                return;
            case Functions.Enable:
                Arm();
                return;
            case Functions.DisableOnce:
                onCannonDeath = Functions.None;
                Disarm();
                return;
            case Functions.Disable:
                Disarm();
                return;
        }

    }
}
