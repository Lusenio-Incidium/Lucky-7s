using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    //Variables
    [Header("- - - Sensitivity Settings - - -")]
    //[SerializeField] int sensHor;
    //[SerializeField] int sensVert;
    [Range(1,10)][SerializeField] float sensitivity;

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
        if(MainMenuManager.instance != null)
        sensitivity = MainMenuManager.instance.sensitivity;
    }

    // Update is called once per frame
    void Update()
    {
        //Grab mouse input
        float mouseY = Input.GetAxis("Mouse Y") * Time.deltaTime * sensitivity * 300;
        float mouseX = Input.GetAxis("Mouse X") * Time.deltaTime * sensitivity * 300;

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
        transform.parent = GameManager.instance.player.transform;
        transform.parent.Rotate(Vector3.up * mouseX);
    }

    public void ApplyRecoil(float recoilAmount)
    {
        float recoilRotation = -recoilAmount * Time.deltaTime * 400;

        if (invertY)
        {
            xrotation -= recoilRotation;
        }
        else
        {
            xrotation += recoilRotation;
        }

        xrotation = Mathf.Clamp(xrotation, lockVerMin, lockVerMax);

        transform.localRotation = Quaternion.Euler(xrotation, 0, 0);

    }

    public float GetSensitivity()
    {
        return sensitivity;
    }
    public void UpdateSensitivity(float newSens)
    {
        sensitivity = newSens;
    }
}
