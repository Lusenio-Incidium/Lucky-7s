using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemMover : MonoBehaviour, IButtonTrigger
{
    private enum MoveMethods 
    {
        RunOnce,
        Loop,
        TeleportToStart,
        Reverse
    }
    [SerializeField] Transform[] points;
    [SerializeField] MoveMethods moveStyle;
    [SerializeField] int movementSpeed;
    [SerializeField] bool spawnMoving;
    [SerializeField] bool buttonStartsMoving;
    [SerializeField] int delay;
    int stage;
    bool reverse;
    bool moving;
    // Update is called once per frame
    private void Start()
    {
        stage = 0;
        if(spawnMoving == true)
        {
            moving = true;
        }
        else
        {
            moving = false;
        }
        reverse = false;
    }
    void Update()
    {
        if (!moving)
        {
            return;
        }
        transform.position = Vector3.MoveTowards(transform.position, points[stage].position, Time.deltaTime * movementSpeed);
        if (Vector3.Distance(transform.position, points[stage].position) < .05f)
        {
            if (!reverse)
            {
                stage++;
            }
            else
            {
                stage--;
            }
            if (stage >= points.Length || stage < 0)
            {
                switch (moveStyle) 
                {
                    case MoveMethods.RunOnce:
                        moving = false;
                        return;
                    case MoveMethods.Loop:
                        stage = 0;
                        break;
                    case MoveMethods.TeleportToStart:
                        stage = 0;
                        StartCoroutine(Delay());
                        break;
                    case MoveMethods.Reverse:
                        reverse = !reverse;
                        if (!reverse)
                        {
                            stage++;
                        }
                        else
                        {
                            stage--;
                        }
                        StartCoroutine(Delay());
                        break;
                }

            }

        }
    }
    IEnumerator Delay()
    {
        moving = false;
        yield return new WaitForSeconds(delay);
        moving = true;
        if(moveStyle == MoveMethods.TeleportToStart)
        {
            transform.position = points[0].position;
        }
    }
    public void OnButtonPress()
    {
        if (moveStyle != MoveMethods.RunOnce)
        {
            if (!buttonStartsMoving)
            {
                moving = false;
            }
            else
            {
                moving = true;
            }
        }
        else
        {
            transform.position = points[0].position;
            moving = true;
        }
    }

        public void OnButtonRelease()
    {
        if (buttonStartsMoving)
        {
            moving = false;
        }
        else
        {
            moving = true;
        }
    }
}
