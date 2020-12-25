using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwipingGesture
{
    // Start is called before the first frame update
    static Vector3 zeroVector = new Vector3(0,0,0);
    static Vector3 leftStartDistance = zeroVector;
    static Vector3 rightStartDistance = zeroVector;

    static Vector3 leftPrevDistance = zeroVector;
    static Vector3 rightPrevDistance = zeroVector;

    //its in meters
    // it updates crazy fast

    const float MIN_DISTANCE = 0.3f;
    const float MIN_DISTANCE_PER_TICK = 0.05f;
    const float MIN_DELTA = 0.12f;
    public const int THRESH = 5;

    public static void resetAll()
    {
      //  resetLeft();
        //resetRight();
    }


   public static void resetLeft(Vector3 pos)
    {
        leftStartDistance = pos;
        leftPrevDistance = pos;
    }

   public static void resetRight(Vector3 pos)
    {
        rightStartDistance = pos;
        rightPrevDistance = pos;
    }

    static bool sameDirection(Vector3 a, Vector3 b)
    {
        a = Vector3.Normalize(a);
        b = Vector3.Normalize(b);
        Vector3 cross = Vector3.Cross(a, b);
        if(cross.magnitude < MIN_DELTA)
        {
            return true;
        }
        return false;
    }

    public static bool checkVector(Vector3 pos, bool right)
    {
   
        Vector3 prevVector = leftPrevDistance;
        Vector3 startVector = leftStartDistance;
        if (right)
        {
            if (rightStartDistance == zeroVector || rightPrevDistance == zeroVector)
            {
                rightStartDistance = pos;
                rightPrevDistance = pos;
                return false;
            }
            prevVector = rightPrevDistance;
            startVector = rightStartDistance;
        } else
        {
            if(leftStartDistance == zeroVector || leftPrevDistance == zeroVector)
            {
                leftStartDistance = pos;
                leftPrevDistance = pos;
                return false;
            }
        }

   
        float distance = Vector3.Distance(pos, startVector);
        float prev_distance = Vector3.Distance(pos, prevVector);
  

        if (right)
        {
            rightPrevDistance = pos;
        }
        else
        {
            leftPrevDistance = pos;
        }

        if (!sameDirection(pos, prevVector))
        {
            if (right)
            {
                resetRight(pos);
            } else
            {
                resetLeft(pos);
            }
        }
     
       if(prev_distance < MIN_DISTANCE_PER_TICK)
        {
            if (right)
            {
                resetRight(pos);
            } else
            {
                resetLeft(pos);
            }
        }

        if (distance > MIN_DISTANCE)
        {
            return true;
        }

        return false;
    }
}
