using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiplierHighlight : MonoBehaviour, IRandomizeHighlight
{
    [SerializeField] int ID;
    [SerializeField] int position;
    [SerializeField] Color highlightColor;

    public void OnHighlight()
    {
        GetComponent<MeshRenderer>().materials[0].SetColor("_EmissionColor", highlightColor);
        GetComponent<MeshRenderer>().materials[1].SetColor("_EmissionColor", highlightColor);
    }
    public void OffHighlight()
    {
        GetComponent<MeshRenderer>().materials[0].SetColor("_EmissionColor", Color.black);
        GetComponent<MeshRenderer>().materials[1].SetColor("_EmissionColor", Color.black);
    }
    public void OnSelect()
    {
        GetComponent<MeshRenderer>().materials[0].SetColor("_EmissionColor", highlightColor);
        GetComponent<MeshRenderer>().materials[1].SetColor("_EmissionColor", highlightColor);
    }
    public int GetPosition()
    {
        return position;
    }

    public int GetID()
    {
        return ID;
    }
}
