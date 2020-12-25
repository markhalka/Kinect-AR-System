using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hand
{
    public int GESTURE_TIMEOUT = 60;

    public P_HandState currentState
    { get; set; }

    public Vector3 position
    { get; set; }

    public bool canMove
    { get; set; }

    public float count
    { get; set; }

    public Hand()
    {
        canMove = true;
        position = new Vector3(0, 0);
        currentState = P_HandState.UNKOWN;
        count = 0;
    }

    const int DELAY = 1; //1 second delay between gestures

    public void updateState()
    {
        canMove = false;
        count = 0;
    }
    public virtual void update() { }

    public void updatePrevious()
    {
        count += Time.deltaTime;
        if (count > DELAY)
        {
            count = 0;
            canMove = true;
        }
    }

    public virtual void updateLasso() { }

    public virtual void updateFist() { }

    public virtual void updateSwipe() { }

}
