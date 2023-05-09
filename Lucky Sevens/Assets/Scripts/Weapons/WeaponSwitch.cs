using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSwitch : MonoBehaviour
{
    public int selectedWeapon = 0;

    // Start is called before the first frame update
    void Start()
    {
        SelectWeapon();
    }


    // Update is called once per frame
    void Update()
    {
        int previousSelectedWeapon = selectedWeapon;

        //seeing what gun player is on to go both directions with mouse wheel
        if (Input.GetAxis("Mouse ScrollWheel") > 0f)
        {
            if (selectedWeapon >= transform.childCount - 1)
            {
                selectedWeapon = 0;
            }
            else
            {
                selectedWeapon++;
            }
        }

        else if (Input.GetAxis("Mouse ScrollWheel") < 0f)
        {
            if (selectedWeapon <= 0)
            {
                selectedWeapon = transform.childCount - 1;
            }
            else
            {
                selectedWeapon--;
            }
        }

        if (previousSelectedWeapon != selectedWeapon)
        {
            SelectWeapon();
        }

    }
    private void SelectWeapon()
    {
        int childCount = transform.childCount;

        for (int i = 0; i < childCount; i++)
        {

            Transform weapon = transform.GetChild(i);

            if (i == selectedWeapon)
            {
                weapon.gameObject.SetActive(true);
                GameManager.instance.gunSystem = weapon.gameObject.GetComponent<GunSystem>();
            }

            else
            {
                weapon.gameObject.SetActive(false);
            }
        }
    }
}
