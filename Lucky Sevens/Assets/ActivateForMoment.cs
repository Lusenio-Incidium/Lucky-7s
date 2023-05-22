using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateForMoment : MonoBehaviour
{
    [SerializeField] GameObject crate;
    private void Awake()
    {
        crate.SetActive(true);
    }
}
