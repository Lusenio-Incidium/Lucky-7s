using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunPickup : MonoBehaviour
{
    [SerializeField] GunStats gunStat;
    [SerializeField] MeshFilter model;
    [SerializeField] MeshRenderer mat;

    private void Start()
    {
        gunStat.bulletsLeft = gunStat.magSize;
        gunStat.ammunition = gunStat.magSize * 4;

        model.mesh = gunStat.model.GetComponent<MeshFilter>().sharedMesh;
        mat.material = gunStat.model.GetComponent<MeshRenderer>().sharedMaterial;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //other.GetComponent<GunSystem>().PickUpWeapon(gunStat);
            GunSystem gunSystem = other.GetComponent<GunSystem>();
            if (gunSystem != null)
            {
                gunSystem.PickUpWeapon(gunStat);
                Destroy(gameObject);
            }
        }
    }
}
