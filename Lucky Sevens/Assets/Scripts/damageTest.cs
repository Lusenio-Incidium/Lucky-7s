using System.Collections;
using System.Collections.Generic;
using UnityEditor.UIElements;
using UnityEngine;

public class damageTest : MonoBehaviour, IDamage,IStatusEffect
{
    [SerializeField] int HP;
    [SerializeField] Renderer model;
    [SerializeField] int speed;
    Color colorOrig = Color.white;
    [SerializeField] StatusEffectObj hitEffect;
    private float timePassed = 0;

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
    public void ApplyStatusEffect(StatusEffectObj data)
    {
        hitEffect = data;
        StartCoroutine(BurnEffect());
        RemoveEffect();
    }

    public void RemoveEffect()
    {
       hitEffect = null;
    }
    public IEnumerator BurnEffect()
    {
        timePassed = Time.time;
        while (Time.time - timePassed <= hitEffect.duration)
        {
            if (hitEffect.damage != 0)
            {
                yield return new WaitForSeconds(hitEffect.damagespeed);
                takeDamage(hitEffect.damage);
            }
        }
    }
}
