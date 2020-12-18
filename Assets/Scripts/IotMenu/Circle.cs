using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

public class Circle 
{
    public float radius;
    public float centerX;
    public float centerY;
    public Vector3 begin;



    // this method finds the center and the radius of the circle that the user draws in the air to control the menu
    Vector3 findCircle(Vector3 pa, Vector3 pb, Vector3 pc)
    {
        double x1 = pa.x;
        double x2 = pb.x;
        double x3 = pc.x;

        double y1 = pa.y;
        double y2 = pb.y;
        double y3 = pc.y;


        NumberFormatInfo setPrecision = new NumberFormatInfo();
        setPrecision.NumberDecimalDigits = 3; // 3 digits after the double point

        double x12 = x1 - x2;
        double x13 = x1 - x3;

        double y12 = y1 - y2;
        double y13 = y1 - y3;

        double y31 = y3 - y1;
        double y21 = y2 - y1;

        double x31 = x3 - x1;
        double x21 = x2 - x1;

        double sx13 = (double)(Math.Pow(x1, 2) -
                        Math.Pow(x3, 2));

        double sy13 = (double)(Math.Pow(y1, 2) -
                        Math.Pow(y3, 2));

        double sx21 = (double)(Math.Pow(x2, 2) -
                        Math.Pow(x1, 2));

        double sy21 = (double)(Math.Pow(y2, 2) -
                        Math.Pow(y1, 2));

        double f = ((sx13) * (x12)
                + (sy13) * (x12)
                + (sx21) * (x13)
                + (sy21) * (x13))
                / (2 * ((y31) * (x12) - (y21) * (x13)));
        double g = ((sx13) * (y12)
                + (sy13) * (y12)
                + (sx21) * (y13)
                + (sy21) * (y13))
                / (2 * ((x31) * (y12) - (x21) * (y13)));

        double c = -(double)Math.Pow(x1, 2) - (double)Math.Pow(y1, 2) -
                                    2 * g * x1 - 2 * f * y1;
        double h = -g;
        double k = -f;
        double sqr_of_r = h * h + k * k - c;

        // r is the radius
        double r = Math.Round(Math.Sqrt(sqr_of_r), 5);

        //center is h,k 
        //radius is r
        return new Vector3((float)h, (float)k, (float)r);
    }


    Vector3 zeroVector = new Vector3(0, 0);
    Vector3 secondLast = new Vector3(0, 0);
    Vector3 last = new Vector3(0, 0);
    Circle currentCircle = null;

    float minDistance = 0.1f;
    float lastAngle = 0;
    int count = 0;
    int recalculateThresh = 10;

    public double calculateDirection(Vector3 hand)
    {
        float vX = hand.x - currentCircle.centerX;
        float vY = hand.y - currentCircle.centerY;
        double magV = Mathf.Sqrt(vX * vX + vY * vY);
        double aX = currentCircle.centerX + vX / magV * currentCircle.radius;
        double aY = currentCircle.centerY + vY / magV * currentCircle.radius;

        double radian = Math.Atan2(aY - currentCircle.centerY, aX - currentCircle.centerX);
        double angle = radian * (180 / Math.PI);
        if (angle < 0.0)
            angle += 360.0;

        if (angle > 360)
        {
            angle -= 360;
        }

        secondLast = last;
        last = hand;

        return angle;
    }

    public double getAngle(Vector3 hand)
    {
        if (hand == zeroVector)
        {
            return 0;
        }
        if (secondLast == zeroVector)
        {
            secondLast = hand;
            return 0;
        }
        if (last == zeroVector)
        {
            if (Vector3.Distance(secondLast, hand) > minDistance)
            {
                last = hand;
            }
            return 0;
        }
        if (Vector3.Distance(hand, last) < minDistance)
        {
            return 0;
        }

        if (currentCircle == null || count > recalculateThresh)
        {
            Vector3 circle = findCircle(secondLast, last, hand);
            currentCircle = new Circle();
            currentCircle.centerX = circle.x;
            currentCircle.centerY = circle.y;
            currentCircle.radius = circle.z;
            currentCircle.begin = secondLast;
            count = 0;
            return 0;
        }

        double angle = calculateDirection(hand);
        return angle;
    }

    public void resetCircle()
    {
        currentCircle = null;
        lastAngle = 0;
        secondLast = new Vector3(0, 0);
        last = new Vector3(0, 0);
    }

}
