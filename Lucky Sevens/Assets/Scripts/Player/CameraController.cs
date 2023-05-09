using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    //Variables
    [Header("- - - Sensitivity Settings - - -")]
    [SerializeField] int sensHor;
    [SerializeField] int sensVert;

    [Header("- - - Angle Lock Settings - - -")]
    [SerializeField] int lockVerMin;
    [SerializeField] int lockVerMax;

    [Header("- - - Misc Settings - - -")]
    [SerializeField] bool invertY;

    float xrotation;

    // Start is called before the first frame update
    void Start()
    {
        //Cursor Lock
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        //Grab mouse input
        float mouseY = Input.GetAxis("Mouse Y") * Time.deltaTime * sensVert;
        float mouseX = Input.GetAxis("Mouse X") * Time.deltaTime * sensHor;

        //Adding our input into our rotation float and inverting if option is on
        if (invertY)
            xrotation += mouseY;
        else
            xrotation -= mouseY;



        //Clamping cam so it can not rotate out of control.
        xrotation = Mathf.Clamp(xrotation, lockVerMin, lockVerMax);

        //rotate camera on the x axis
        transform.localRotation = Quaternion.Euler(xrotation, 0, 0);

        //Apply rotation to our player (y axis)
        transform.parent.Rotate(Vector3.up * mouseX);
    }
}