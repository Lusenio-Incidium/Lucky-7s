using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class damageTest : MonoBehaviour, IDamage,IStatusEffect
{
    [SerializeField] float HP;
    [SerializeField] Renderer model;
    [SerializeField] int speed;
    Color colorOrig = Color.white;
    [SerializeField] StatusEffectObj hitEffect;
    private float timePassed = 0;

    public void takeDamage(float dmg)
    {
        HP -= dmg;
        StartCoroutine(FlashColor());

        if (HP <= 0)
        {
            Destroy(gameObject);
        }
    }
    public void instaKill()
    {
        takeDamage(HP);
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
    }

    public void RemoveEffect()
    {
       hitEffect = null;
    }
    public IEnumerator BurnEffect()
    {
        timePassed = Time.time;
        if (hitEffect.duration != 0)
        {
            while (Time.time - timePassed <= hitEffect.duration)
            {
                if (hitEffect.damage != 0)
                {
                    yield return new WaitForSeconds(hitEffect.damagespeed);
                    takeDamage(hitEffect.damage);
                    if (timePassed >= hitEffect.duration)
                    {
                        RemoveEffect();
                    }
                }
            }
        }
    }
}
