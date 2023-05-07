using System.Collections;
using System.Collections.Generic;
using System.Security.Authentication.ExtendedProtection;
using UnityEngine;

public class BlockHighlight : MonoBehaviour, IRandomizeHighlight
{
    [SerializeField] int _position;
    [Range(0.1f, 1)][SerializeField] float _blinkPeriod;
    [SerializeField] Material _whileHighlighted;
    [SerializeField] Material _whileNotHighlighted;
    [SerializeField] Material _whileSelected;
    bool blinkOn;

    float _currBlinkPeriod;
    void Start()
    {
        _currBlinkPeriod = _blinkPeriod;
    }
    public void OnHighlight()
    {
        gameObject.GetComponent<Renderer>().material = _whileHighlighted;
    }
    public void OffHighlight()
    {
        gameObject.GetComponent<Renderer>().material = _whileNotHighlighted;
    }
    public void OnSelect()
    {
        _currBlinkPeriod -= Time.deltaTime;
        if (_currBlinkPeriod <= 0)
        {
            if (blinkOn)
            {
                gameObject.GetComponent<Renderer>().material = _whileNotHighlighted;
                blinkOn = false;
            }
            else
            {
                gameObject.GetComponent<Renderer>().material = _whileSelected;
                blinkOn = true;
            }
            _currBlinkPeriod = _blinkPeriod;
        }
    }
    public int GetPosition()
    {
        return _position;
    }
}
