using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenade2 : MonoBehaviour
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

    // Update is called once per frame
}
