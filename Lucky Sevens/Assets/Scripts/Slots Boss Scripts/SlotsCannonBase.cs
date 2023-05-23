using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//
// Hello, Just warning you that this script is absolutely terrible. I know a lot of my other scripts invoke a similar reaction, but this is especially bad.
// If you are going to modify it I'd recommend either rewritting it or trying to find an alternative method for your solution.
// The current point of this script is to move the square underneath the cannon.
// -Luse
//

public class SlotsCannonBase : MonoBehaviour
{
    [SerializeField] SlotsWeakPoint cannon;
    [SerializeField] Transform hiddenWallPosition; //I'm tired and I don't want to work with angles. 
    [SerializeField] int displacment;
    [SerializeField] int retreatSpeed;
    [SerializeField] int moveSpeed;
    bool move;
    bool up;
    bool isHome;
    bool goInsideWall;
    bool isInWall;
     bool actionOut;
    bool actionIn;

    Vector3 origLocation;
    private void Start()
    {
        move = false;
        origLocation = transform.position;
        //goInsideWall = true;
    }
    private void Update()
    {
        if (move && !isInWall) //IF given the instrution to move AND is not in the wall
        {
            isHome = false;
            MoveVertically();
        }
        else if (goInsideWall && isHome) //IF given the instruction to move AND is "Home"
        {
            returnToWall();
        }
        else
        {
            ReturnToNeutral();
        }
    }

    void MoveVertically()
    {
        if (up) 
        {
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(transform.position.x, origLocation.y + displacment, transform.position.z), Time.deltaTime* moveSpeed);
            if (Vector3.Distance(transform.position, new Vector3(transform.position.x, origLocation.y + displacment, transform.position.z)) < .05f)
            {
                up = !up;
            }
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(transform.position.x, origLocation.y - displacment, transform.position.z), Time.deltaTime * moveSpeed);
            if (Vector3.Distance(transform.position, new Vector3(transform.position.x, origLocation.y - displacment, transform.position.z)) < .05f)
            {
                up = !up;
            }
        }
    }

    void returnToWall()
    {
        transform.position = Vector3.MoveTowards(transform.position, hiddenWallPosition.position, Time.deltaTime * retreatSpeed);
        if (Vector3.Distance(transform.position, hiddenWallPosition.position) < .05f)
        {
           isInWall = true;
        }
    }

    void ReturnToNeutral()
    {
        transform.position = Vector3.MoveTowards(transform.position, origLocation, Time.deltaTime * retreatSpeed);
        if (Vector3.Distance(transform.position, origLocation) < .05f)
        {
            isHome = true;
            isInWall = false;
            transform.position = origLocation;
            if (!goInsideWall && actionOut)
            {
                cannon.IsOut();
                actionOut = false;
            }
            else if (actionIn)
            {
                cannon.GoingIn();
                actionIn = false;
            }
        }
        else
        {
            isHome = false;
        }
    }
    public void HideCannon()
    {
        move = false;
        goInsideWall = true;
        actionIn = true;
        ReturnToNeutral();
    }

    public void BringOutCannon()
    {
        goInsideWall = false;
        actionOut = true;

    }
    public void StartMoving()
    {
        move = true;
    }
}
