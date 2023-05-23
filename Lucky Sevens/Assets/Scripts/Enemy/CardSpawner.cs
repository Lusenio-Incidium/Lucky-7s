using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CardSpawner : MonoBehaviour
{
    [Range(1, 5)][SerializeField] float _spinSpeed;
    [SerializeField] GameObject _enemy;
    bool spinning;
    bool spawnPending;
    private void Start()
    {
        spinning = true;
    }

    private void Update()
    {
        if(spinning)
        gameObject.transform.Rotate(0, _spinSpeed, 0 * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<PlayerController>().takeDamage(5);
        }
        if (!spawnPending)
        {
            StartCoroutine(Spawn());
        }
    }
    IEnumerator Spawn()
    {
        spawnPending = true;
        spinning = false;
        Instantiate(_enemy, new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, gameObject.transform.position.z), gameObject.transform.rotation);
        yield return new WaitForSeconds(1);
        Destroy(gameObject);
    }
}
