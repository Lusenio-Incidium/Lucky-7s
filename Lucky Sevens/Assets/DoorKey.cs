using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorKey : MonoBehaviour
{
    [SerializeField] GeneralDoor door;
    void Start()
    {
        this.gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        door.openDoor();
        Destroy(gameObject);
    }
}
