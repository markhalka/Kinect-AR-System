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
    public string rightHandState = "-";
    public string leftHandState = "-";
    public CameraSpacePoint posRight;
    public CameraSpacePoint posLeft;
    public Windows.Kinect.Vector4 rotRight;



    // Start is called before the first frame update
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


    //when the audio detects a snap, it double checks the users hands to make sure that it was a snap
    public bool doubleCheckSnap() { return false; }


    //check if the user is moving their wrist, and if so in which direction. 0 for clockwise, 1 for counterclockwise, -1 for no movment
    public int checkWrist() { return -1; }


    //check if the did a selecting motion
    public bool checkSelect() { return false; }


    //check if the user made a resizing motion with their thumb and index finger. 0 for smaller, 1 for bigger, -1 for no motion
    public int checkResize() { return -1; }


    //check if the user is pointing at a screen, if so return the index of the screen. index 0 is the computer screen, otherwise, return -1
    public int checkPoint() { return -1; }


    //check if the user is making a fist
    public bool checkFist() { return false; }


    //check if the user wants to move the window in any direction, and return the direction
    public Vector2 checkMove() { return new Vector2(0, 0); }


    //checks if the user wants to create a new window, when the user points at the screen, and swipes in any direction
    //return the direction the user swiped the window in if yes
    public Vector2 checkCreateWindow() { return new Vector2(0, 0); }


    //see if the user wants to destroy a window, by pointing at a window, and closing their fist. true if it shoudl be destroyed, false otherwise
    public bool checkDestroyWindow() { return false; }


    //this checks to see if the user is using their index finger to scroll on a given window. It return 0 if scrolling up, 1 if scrolling down
    //and -1 if not scrolling
    public int checkScroll() { return -1; }


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

                 posLeft = body.Joints[JointType.HandLeft].Position;
                 posRight = body.Joints[JointType.HandRight].Position;
                 rotRight = body.JointOrientations[JointType.HandRight].Orientation;


             //   gameObject.transform.position = new Vector3(pos.X, pos.Y); //this getts the position of your hands

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
            rightHandState = output;
        }
        else
        {
            leftHandState = output;
        }
    }


}
