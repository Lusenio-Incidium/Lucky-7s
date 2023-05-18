using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IRandomizeHighlight
{
    public void OnHighlight();
    public void OffHighlight();
    public void OnSelect();
    public int GetPosition();

    public int GetID();
}
