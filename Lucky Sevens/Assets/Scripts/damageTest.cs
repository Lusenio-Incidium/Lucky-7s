using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class damageTest : MonoBehaviour, IDamage
{
    [SerializeField] int HP;
    [SerializeField] Renderer model;
    Color colorOrig;
    public void takeDamage(int dmg)
    {
        HP -= dmg;
        StartCoroutine(FlashColor());

        if (HP <= 0)
        {
            Destroy(gameObject);
        }
    }
    IEnumerator FlashColor()
    {
        model.material.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        model.material.color = colorOrig;
    }
}
