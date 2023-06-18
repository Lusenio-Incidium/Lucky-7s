using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TextHandler : MonoBehaviour, IButtonTrigger, ICannonKey, IBattle, IDialouge
{
    private enum Functions 
    {
        None,
        FadeIn,
        FadeInOnce,
        FadeOut,
        FadeOutOnce,
    }
    [SerializeField] TextMeshPro text;
    [SerializeField] bool disappearAfterXTime;
    [SerializeField] float disappearTimer;
    [Header("OnDialougeContinue")]
    [SerializeField] GameObject[] triggerOnDisable;
    [Header("Trigger Functions")]
    [SerializeField] Functions onButtonPress;
    [SerializeField] Functions onButtonRelease;
    [SerializeField] Functions onBattleBegin;
    [SerializeField] Functions onBattleEnd;
    [SerializeField] Functions onCannonDeath;
    [SerializeField] Functions onDialougeContinue;
    [SerializeField] Functions onTriggerEnter;
    [SerializeField] Functions onTriggerExit;

    float currDisapearTimer;

    private void Update()
    {
        if(text.enabled && disappearAfterXTime)
        {
            currDisapearTimer += Time.deltaTime;
            if(currDisapearTimer >= disappearTimer)
            {
                disableText();
                currDisapearTimer = 0;
            }
        }
    }

    private void disableText()
    {
        if (!text.enabled)
        {
            return;
        }
        text.enabled = false;
        foreach(GameObject obj in triggerOnDisable)
        {
            IDialouge dialoge = obj.GetComponent<IDialouge>();
            if(dialoge != null)
            {
                dialoge.OnDialougeContinue();
            }
            else
            {
                Debug.LogWarning("Obj missing IDialouge");
            }
        }
    }
    private Functions FunctionActions(Functions function)
    {
        switch (function) 
        {
            case Functions.None:
                return Functions.None;
            case Functions.FadeIn:
                text.enabled = true;
                break;
            case Functions.FadeInOnce:
                text.enabled = true;
                function = Functions.None;
                break;
            case Functions.FadeOut:
                disableText();
                break;
            case Functions.FadeOutOnce:
                disableText();
                function = Functions.None;
                break;
        }
        return function;
    }

    public void OnButtonPress()
    {
        onButtonPress = FunctionActions(onButtonPress);
    }
    public void OnButtonRelease()
    {
        onButtonRelease = FunctionActions(onButtonRelease);
    }

    public void OnBattleBegin()
    {
        onBattleBegin = FunctionActions(onBattleBegin);
    }

    public void OnBattleEnd()
    {
        onBattleEnd = FunctionActions(onBattleEnd);
    }

    public void OnCannonDeath()
    {
        onCannonDeath = FunctionActions(onCannonDeath);
    }

    public void OnDialougeContinue()
    {
        onDialougeContinue = FunctionActions(onDialougeContinue);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            onTriggerEnter = FunctionActions(onTriggerEnter);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            onTriggerExit = FunctionActions(onTriggerExit);
        }
    }
}
