using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpItem : MonoBehaviour
{
    [SerializeField] Transform objPicked;
    [SerializeField] Transform cameraTrans;

    [SerializeField] bool canGrab;
    [SerializeField] bool wasPickedUp;
    [SerializeField] bool isPickedUp;

    [SerializeField] Rigidbody rb;

    private void OnTriggerStay(Collider other)
    {
        if(other.CompareTag("MainCamera"))
        {
            canGrab = true;
            wasPickedUp = true;
            GameManager.instance.gunSystem.SetReadyToShoot(false);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("MainCamera"))
        {
            if (!isPickedUp)
            {
                GameManager.instance.gunSystem.SetReadyToShoot(true);
                canGrab = false;
            }
        }
    }

    void Update()
    {
        if(canGrab)
        {
            if(Input.GetKeyDown(KeyCode.Mouse0))
            {
                rb.velocity = Vector3.zero;
                rb.Sleep();
                objPicked.parent = cameraTrans;
                rb.useGravity = false;
                isPickedUp = true;
            }
            if (Input.GetKeyUp(KeyCode.Mouse0))
            {
                objPicked.parent = null;
                rb.useGravity = true;
                isPickedUp = false;
            }
            if(isPickedUp)
            {
                if(Input.GetKeyDown(KeyCode.Mouse1))
                {
                    objPicked.parent = null;
                    rb.useGravity = true;
                    rb.velocity = GameManager.instance.playerScript.GetThrowPower() * cameraTrans.forward * Time.deltaTime;
                    isPickedUp = false;
                }
            }
        }
    }
    public bool PickedUpByPlayer()
    {
        return wasPickedUp;
    }
}
