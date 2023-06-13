using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class movecard : MonoBehaviour
{

   public GameObject cardCurrPos;
   public GameObject cardNewPos;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(MoveCardPos());
        }
    }

    private IEnumerator MoveCardPos()
    {
        float elapseTime = 0f;
        float duration = 0.1f;
        Vector3 initialPosition = cardCurrPos.transform.localPosition;
        Quaternion initialRotation = cardNewPos.transform.localRotation;
        
        while(elapseTime < duration)
        {
           cardCurrPos.transform.localPosition = Vector3.Lerp(cardCurrPos.transform.position, cardNewPos.transform.position, elapseTime / duration);
            cardCurrPos.transform.localRotation = Quaternion.Slerp(cardCurrPos.transform.rotation, cardNewPos.transform.rotation, elapseTime / duration);
            elapseTime += Time.deltaTime;
            yield return null;
        }

    }
}