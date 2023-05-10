using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicalButtonPress : MonoBehaviour
{
    [SerializeField] int lowerSpeed;
    [SerializeField] float displace;
    bool hit = false;
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player") || hit)
        {
            return;
        }
        hit = true;
        gameObject.transform.position = Vector3.Lerp(gameObject.transform.position, new Vector3(gameObject.transform.position.x, gameObject.transform.position.y - displace, gameObject.transform.position.z), Time.deltaTime * lowerSpeed);
    }
}
