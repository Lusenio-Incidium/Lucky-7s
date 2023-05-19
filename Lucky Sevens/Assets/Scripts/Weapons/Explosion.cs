using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    [SerializeField] int damage;
    [SerializeField] int pushAmount;

    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, 0.15f);    
    }

    // Update is called once per frame
    private void OnTriggerEnter(Collider other)
    {
        IPhysics physics = other.GetComponent<IPhysics>();
        if(physics !=  null)
        {
            Vector3 dir = other.transform.position - transform.position;
            physics.TakePush(dir * pushAmount);
        }
    }
}
