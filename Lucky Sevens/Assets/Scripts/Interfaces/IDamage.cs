using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamage
{
    void takeDamage(float dmg, Transform position = null);
    void instaKill();
}
