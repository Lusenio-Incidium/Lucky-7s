using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoToNewArea : MonoBehaviour
{
    public GameObject newResPoint;
    public GameObject oldResPoint;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            oldResPoint.SetActive(false);
            newResPoint.SetActive(true);
            other.transform.position = newResPoint.transform.position;
            other.transform.rotation = newResPoint.transform.rotation;
        }
    }
}
