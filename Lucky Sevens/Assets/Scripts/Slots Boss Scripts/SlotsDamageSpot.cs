using UnityEngine;

public class SlotsDamageSpot : MonoBehaviour, IDamage
{
    [SerializeField] float health;
    [SerializeField] GameObject boom;
    [SerializeField] ParticleSystem spark;
    bool destroyed = false;

    public void takeDamage(float count, Transform pos = null)
    {
        if (destroyed)
        {
            return;
        }
        health -= count;
        BossManager.instance.currHP -= count;
        if (health <= 0)
        {
            destroyed = true;
            Instantiate(boom, transform.position, transform.rotation);
            BossManager.instance.onUnstun();
            BossManager.instance.onBossDamage();
        }
        else
        {
            Instantiate(spark, transform.position, transform.rotation);

        }
    }

    public void instaKill()
    {
        takeDamage(health);
    }
}
