using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IRandomizeAction
{
    public int GetPosition();
    public void OnSelect();

    public int GetID();
}
