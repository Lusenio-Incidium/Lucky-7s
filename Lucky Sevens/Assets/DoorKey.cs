using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorKey : MonoBehaviour, IButtonTrigger
{
    [SerializeField] GeneralDoor door;
    void Start()
    {
        this.gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) 
        {
            door.openDoor();
            Destroy(gameObject);
        }
        
    }

    public void OnButtonPress() 
    {
        gameObject.SetActive(true);
    }

    public void OnButtonRelease() 
    {
        gameObject.SetActive(false);
    }
}
