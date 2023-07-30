using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeleteDebris : MonoBehaviour
{
    [SerializeField] float lingerTime;
    [SerializeField] float fadeOutTime;
    Color origColor;
    Renderer[] pieces;
    bool fade = false;
    float currTime;

    void Start()
    {
        pieces = gameObject.GetComponentsInChildren<Renderer>();
        Destroy(gameObject, lingerTime + fadeOutTime);
        StartCoroutine(Delay());
        origColor = gameObject.GetComponentInChildren<Renderer>().material.color;
    }

    // Update is called once per frame
    void Update()
    {

        if (fade == true)
        {
            currTime += Time.deltaTime;
            foreach(Renderer render in pieces)
            {
                render.material.color = Color.Lerp(origColor, new Color(origColor.r, origColor.g, origColor.b, 0), currTime / fadeOutTime);
            }
        }
    }

    IEnumerator Delay()
    {
        yield return new WaitForSeconds(lingerTime);
        fade = true;
    }
}
