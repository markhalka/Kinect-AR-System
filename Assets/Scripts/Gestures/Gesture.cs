using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Windows.Kinect;
using Joint = Windows.Kinect.Joint;

public class Gesture : MonoBehaviour
{
    public GameObject BodySrcManager;
    private BodySourceManager bodyManager;
    private Body[] bodies;

    // Find the hand states
    private P_HandState rightHandState = P_HandState.UNKOWN;
    private P_HandState leftHandState = P_HandState.UNKOWN;
    private CameraSpacePoint posRight;
    private CameraSpacePoint posLeft;

    public P_HandState RightHandState { get => rightHandState; set => rightHandState = value; }
    public P_HandState LeftHandState { get => leftHandState; set => leftHandState = value; }
    public CameraSpacePoint PosRight { get => posRight; set => posRight = value; }
    public CameraSpacePoint PosLeft { get => posLeft; set => posLeft = value; }


    SwipingGesture swipe;

    void Start()
    {
        swipe = new SwipingGesture();
        if (BodySrcManager == null)
        {
            Debug.LogError("no body source manager assigned");
        }
        else
        {
            bodyManager = BodySrcManager.GetComponent<BodySourceManager>();
        }
    }
   
    public int getBodyCount()
    {
        return bodies.Length;
    }

    void Update()
    {
        if (bodyManager == null)
        {
            return;
        }

        bodies = bodyManager.GetData();

        if (bodies == null)
        {
            return;
        }

        foreach (var body in bodies)
        {
            if (body == null)
                continue;

            if (body.IsTracked)
            {

                PosLeft = body.Joints[JointType.HandLeft].Position;
                PosRight = body.Joints[JointType.HandRight].Position;

                var leftElbow = body.Joints[JointType.ElbowLeft].Position;
                var rightElbow = body.Joints[JointType.ElbowRight].Position;

                if(PosLeft.Y >= leftElbow.Y)
                {
                    updateHandState(body.HandLeftState, false);
                }

                if(PosRight.Y >= rightElbow.Y)
                {
                    updateHandState(body.HandRightState, true);
                }              
            }
        }
    }

    void updateHandState(HandState state, bool right)
    {
        P_HandState output = P_HandState.UNKOWN;

        switch (state)
        {
            case HandState.Open:
                output = P_HandState.OPEN;
                break;
            case HandState.Closed:
                output = P_HandState.CLOSED;
                break;
            case HandState.Lasso:
                output = P_HandState.LASSO;
                break;
            case HandState.Unknown:
                output = P_HandState.UNKOWN;
                break;
            case HandState.NotTracked:
                output = P_HandState.UNKOWN;
                break;
            default:
                break;
        }

        if (right)
        {
            RightHandState = output;
        }
        else
        {
            LeftHandState = output;
        }
    }
}
