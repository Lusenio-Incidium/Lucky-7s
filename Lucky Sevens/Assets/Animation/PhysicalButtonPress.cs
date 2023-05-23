using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicalButtonPress : MonoBehaviour
{
    [SerializeField] int lowerSpeed;
    [SerializeField] float displace;
    [SerializeField] Material activeMaterial;
    [SerializeField] Material deactiveMaterial;
    bool hit = false;
    Vector3 StartPosition;
    Vector3 EndPosition;


    private void Start()
    {
        StartPosition = transform.position;
        EndPosition = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y - displace, gameObject.transform.position.z);
    }
    private void Update()
    {
        if (hit && !(Vector3.Distance(transform.position, EndPosition) < .1f && Vector3.Distance(transform.position, EndPosition) > -.1f) && hit)
        {
            gameObject.transform.position = Vector3.Lerp(gameObject.transform.position, EndPosition, Time.deltaTime * lowerSpeed);

        }
        else if (!(Vector3.Distance(transform.position, StartPosition) < .1f && Vector3.Distance(transform.position, StartPosition) > -.1f) && !hit)
        {
            gameObject.transform.position = Vector3.Lerp(gameObject.transform.position, StartPosition, Time.deltaTime * lowerSpeed);


        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player") || hit)
        {
            return;
        }
        Disarm();
    }

    public void Reset()
    {
        GetComponent<MeshRenderer>().material = activeMaterial;
        hit = false;
    }
    public void Disarm()
    {
        GetComponent<MeshRenderer>().material = deactiveMaterial;
        hit = true;
    }
}
