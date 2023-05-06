using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SpawnConditions
{
    enum SpawnLocations 
    {
        Roof = 0,
        BottomSlot,
        TopSlot
    }

    [SerializeField] GameObject Object;
    [SerializeField] SpawnLocations Locations;
}
