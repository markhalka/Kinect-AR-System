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
    private string rightHandState = "-";
    private string leftHandState = "-";
    private CameraSpacePoint posRight;
    private CameraSpacePoint posLeft;

    public string RightHandState { get => rightHandState; set => rightHandState = value; }
    public string LeftHandState { get => leftHandState; set => leftHandState = value; }
    public CameraSpacePoint PosRight { get => posRight; set => posRight = value; }
    public CameraSpacePoint PosLeft { get => posLeft; set => posLeft = value; }

    void Start()
    {
        if (BodySrcManager == null)
        {
            Debug.LogError("no body source manager assigned");
        }
        else
        {
            bodyManager = BodySrcManager.GetComponent<BodySourceManager>();
        }
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

                updateHandState(body.HandLeftState, false);
                updateHandState(body.HandRightState, true);           
            }
        }
    }

    void updateHandState(HandState state, bool right)
    {
        string output = "";

        switch (state)
        {
            case HandState.Open:
                output = "Open";
                break;
            case HandState.Closed:
                output = "Closed";
                break;
            case HandState.Lasso:
                output = "Lasso";
                break;
            case HandState.Unknown:
                output = "Unknown...";
                break;
            case HandState.NotTracked:
                output = "Not tracked";
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
