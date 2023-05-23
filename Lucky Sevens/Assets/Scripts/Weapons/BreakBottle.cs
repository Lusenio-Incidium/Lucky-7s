using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakBottle : MonoBehaviour
{
    [SerializeField] GameObject puddle;
    [SerializeField] bool smashed = false;
    // Start is called before the first frame update
    private void OnTriggerEnter(Collider other)
    {
        PickUpItem pop = gameObject.GetComponent<PickUpItem>();
        if(pop.PickedUpByPlayer())
        {
            bool yrue = other.CompareTag("Floor");
            if (yrue)
            {
                Instantiate(puddle, gameObject.transform.position, other.transform.rotation);
                Destroy(gameObject);
                smashed = true;
            }
        }
    }
}
