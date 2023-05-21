using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenade2 : MonoBehaviour, IThrowable
{
    [SerializeField] GameObject explosion;
    [SerializeField] int timer;

    // Start is called before the first frame update
    IEnumerator Start()
    {
        yield return new WaitForSeconds(timer);
        Instantiate(explosion, transform.position, explosion.transform.rotation);
        Destroy(gameObject);
    }
    public void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("MainCamera"))
        {
           OnCollect();
        }
    }

    public void OnCollect()
    {
       if(Input.GetKeyDown(KeyCode.Mouse0))
       {
            rb.useGravity = false;
            gameObject.transform.parent = Parent;
       }
    }

    // Update is called once per frame
}
