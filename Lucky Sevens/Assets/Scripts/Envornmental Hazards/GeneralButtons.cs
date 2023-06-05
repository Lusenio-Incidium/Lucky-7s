using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneralButtons : MonoBehaviour
{
    [SerializeField] GameObject[] affectedItems;
    [SerializeField] PhysicalButtonPress buttonAnimation;
    [SerializeField] bool popsUp;
    [SerializeField] int popUpTimer;
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
    }
