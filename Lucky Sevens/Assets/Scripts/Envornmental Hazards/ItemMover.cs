using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemMover : MonoBehaviour, IButtonTrigger, ICannonKey, IBattle
{
    private enum Functions
    {
        None,
        Start,
        Stop
    }
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
    [Header("Trigger Functions")]
    [SerializeField] Functions onButtonPress;
    [SerializeField] Functions onButtonRelease;
    [SerializeField] Functions onBattleBegin;
    [SerializeField] Functions onBattleEnd;
    [SerializeField] Functions onCannonDeath;

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
                        Debug.Log("Stopped");
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

    private Functions FunctionAction(Functions function)
    {
        switch (function) 
        {
            case Functions.None:
                return Functions.None;
            case Functions.Start:
                moving = true;
                Debug.Log("Triggered SG2");
                break;
            case Functions.Stop:
                moving = false;
                break;

        }
        return function;
    }

    public void OnButtonPress()
    {
        onButtonPress = FunctionAction(onButtonPress);
    }
    public void OnButtonRelease()
    {
        onButtonRelease = FunctionAction(onButtonRelease);

    }
    public void OnCannonDeath()
    {
        onCannonDeath = FunctionAction(onCannonDeath);
    }

    public void OnBattleBegin()
    {
        onBattleBegin = FunctionAction(onBattleBegin);
        Debug.Log("Triggered SG1");
    }

    public void OnBattleEnd()
    {
        onBattleEnd = FunctionAction(onBattleEnd);
    }
}
