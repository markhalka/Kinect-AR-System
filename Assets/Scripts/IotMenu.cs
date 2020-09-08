using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Windows.Kinect;
using System;
using System.Globalization;


public class IotMenu : MonoBehaviour
{

    public GameObject menuContainer;
    public bool isMenuOpen = false;
    public static string buttonCall;

    RadialMenu script;
    void Start()
    {
        script = menuContainer.GetComponent<RadialMenu>();
        buttonCall = "";
        isMenuOpen = false;
    }

    void Update()
    {
        if(buttonCall != "")
        {
            handleMenuUpdate(buttonCall);
            buttonCall = "";
        }
        handleKeyboard();
    }

    public enum menus {Home, Security, IOT, Settings, Effects };
    public menus currentMenu;
    public void handleMenuUpdate(string menuName)
    {
        switch (currentMenu)
        {
            case menus.Home:
                switch (menuName)
                {
                    case "Security":
                        openMenu("Security", new string[] { "View Cameras", "Edit Cameras", "Settings", "Recordings" });
                        break;
                    case "IOT":
                        openMenu("IOT", new string[] { "Lights", "Door", "Windows", "Blinds", "Select Room", "Edit Devices", "Settings" });
                        break;
                    case "Settings":
                        openMenu("IOT", new string[] { "Brightness", "Sensor recalibration", "Device info", "Computer Synching", "Gesture settings" });
                        break;
                    case "Effects":
                        openMenu("IOT", new string[] { "Edit Effects", "Select Effects", "Create Effects" });
                        break;
                }
                break;
              
            case menus.Security:
                switch (menuName)
                {
                    case "View Cameras":
                        break;
                    case "Settings":
                        openMenu("Camera Settings", new string[] { "Add Camera", "Enable All", "Disable All" });
                        break;
                    case "Recordings":
                        break;
                }
                break;
        
            case menus.IOT:
                switch (menuName)
                {
                    case "Select Room": //just a list of the rooms (if there is more than 8, than there is a button that says more, which shows the rest of the rooms)
                        break;
                    case "Edit Devices": 
                        break;
                    case "Settings":
                        break;
                }
                break;
            case menus.Settings:
                switch (menuName)
                {
                    case "Brightness":
                        break;
                    case "Sensor recalibration":
                        break;
                    case "Device info": 
                        break;
                    case "Computer Synching":
                        break;
                    case "Gesture settings":
                        break;
                }
                break;
            case menus.Effects:
                switch (menuName)
                { 
                    case "View Effects":
                        break;
                    case "Create Effects":
                        break;
                }
                break;
        }
    }

    #region submenus
    public void handleMenu()
    {
        switch (currentMenu)
        {
            case menus.Effects:
                break;
            case menus.Security:

                break;
            case menus.Settings:
                break;
            case menus.IOT:
                break;
        }
    }
    enum security { cameras, recordings};
    security secMenu;
    void handleEffects()
    {

    }

    void handleSecurity()
    {
        switch (secMenu)
        {
            case security.cameras:
                handleCamera();
                break;
            case security.recordings:
                handleRecording();
                break;
        }
    }

    void handleSettings()
    {

    }

    void handleIOT()
    {

    }

    #region Security

    public class Camera
    {
        public string ip;
        public string name;
        public string location;
        public string status;
    }

    public class Recording
    {
        public bool seen;
        public string date;
        public string location;
    }

    public List<Camera> cameras;
    public List<Recording> recordings;

    public GameObject recordingPanel;
    public GameObject seenContainer;
    public GameObject unseenContainer;
    public void openRecordings()
    {
        recordings = new List<Recording>(); //this is where you would list the recordings (same thing here)
        GameObject newRecordingPanel;
        foreach (var r in recordings)
        {
            if (r.seen)
            {
                newRecordingPanel = Instantiate(recordingPanel, seenContainer.transform);
            } else
            {
                newRecordingPanel = Instantiate(recordingPanel, unseenContainer.transform);
            }
            newRecordingPanel.transform.GetChild(0).GetComponent<TMPro.TMP_Text>().text = r.date;
            newRecordingPanel.transform.GetChild(1).GetComponent<TMPro.TMP_Text>().text = r.location;

        }
    }

    //open and initialize the camera thing 
    //
    public GameObject cameraPanel;
    public void openCamera()
    {
        cameras = new List<Camera>(); //this is where you would init cameras, these could be from a text file or list somewhere (maybe xml)


        foreach (var c in cameras)
        {
            GameObject newPanel = Instantiate(cameraPanel, cameraPanel.transform.parent);
            newPanel.transform.GetChild(0).GetComponent<TMPro.TMP_Text>().text = c.name;
            newPanel.transform.GetChild(1).GetComponent<TMPro.TMP_Text>().text = "status: " + c.status;
            newPanel.transform.GetChild(2).GetComponent<TMPro.TMP_Text>().text = "location: " + c.location;

        }
    }

    GameObject currentWindow;
    Camera currentCamera;
    //this method will handle the updates for the video menu
    public void handleCamera()
    {
        if(currentWindow != null)
        {
            //that camera is selected, now open the settings options for that camera, and then done 
            int index = 0;
            for(; index < cameraPanel.transform.childCount; index++)
            {
                if(cameraPanel.transform.GetChild(index).gameObject == currentWindow)
                {
                    break;
                }
            }
            currentCamera = cameras[index];
            currentWindow = null;
            openMenu("Camera settings", new string[] { }); //add the values here, and functions to deal with them
        }
    }

    Recording currentRecording;
    //this will handle the updates for the recording menu
    public void handleRecording()
    {
        if(currentWindow != null)
        {
            //that recording is selected, so just open the settings than done 
            //that camera is selected, now open the settings options for that camera, and then done 
            int index = 0;
            for (; index < recordingPanel.transform.childCount; index++)
            {
                if (recordingPanel.transform.GetChild(index).gameObject == currentWindow)
                {
                    break;
                }
            }
            currentRecording = recordings[index];
            currentWindow = null;
            openMenu("Recording settings", new string[] { }); //add the values here, and functions to deal with them
        }
    }
    #endregion

    #region Effects

    public class Effect
    {
        public string name;
        public string effectShaderName;

    }

    List<Effect> effects;

    void showEffects()
    {

    }



    #endregion

    #region IOT

    public enum IOTType { door, window, blind, light, other}
    public enum IOTValues { open, closed, locked, unlocked}

    public class IOTDevice
    {
        public string name;
        public string room;
        public IOTType type;
        public IOTValues value;

    }

    public class IOTRoom
    {
        public List<IOTDevice> devices;
        public string name;
    }

    List<IOTRoom> rooms;
    //initialize the room and all connected devices
    void openRoom()
    {
        //this will just call open menu
        List<string> deviceNames = new List<string>();
        foreach(var d in currentRoom.devices)
        {
            deviceNames.Add(d.name);
        }
        openMenu(currentRoom.name, deviceNames.ToArray());
    }

    IOTRoom currentRoom;
    void handleRoom()
    {
        //this will call open menu, and show all rooms
        rooms = new List<IOTRoom>(); //tihs will initialize the rooms (probably from a text file or something)
        List<string> names = new List<string>();
        foreach (var r in rooms)
        {
            names.Add(r.name);
        }
        openMenu("Rooms", names.ToArray());
    }

    //show the slider 
    void handleSlider()
    {

    }

    void handleMulti()
    {
        //show the options
        //this is the function called for blinds, windows, and door
    }



    #endregion

    #endregion

    public void openHomeMenu()
    {
        openMenu("Home", new string[] { "Security", "IOT", "Settings", "Effects" });
    }

    public void openMenu(string name, string[] values)
    {
        if (!isMenuOpen)
        {
            showMenu(name, values);
        } else
        {
            StartCoroutine(handleNewMenu(name, values));
        }   
    }

    void showMenu(string name, string[] values)
    {
        isMenuOpen = true;
        menuContainer.SetActive(true);
        var script = menuContainer.GetComponent<RadialMenu>();

        script.GenerateMenu(name, values);
        script.ActivateMenu();
    }

    IEnumerator handleNewMenu(string name, string[] values)
    {       
        script.closeOnDeactivate = false;
        script.DeactivateMenu();
        while (script.m_State != RadialMenu.State.Deactivated)
        {
            yield return new WaitForSeconds(0.5f);
        }
        showMenu(name, values);
    }



    //closes the current menu
    public void closeMenu()
    {
        isMenuOpen = false;
        menuContainer.GetComponent<RadialMenu>().closeOnDeactivate = true;
        menuContainer.GetComponent<RadialMenu>().DeactivateMenu();
        resetMenu();
    }


    public class Circle
    {
        public float radius;
        public float centerX;
        public float centerY;
        public Vector3 begin;
    }

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

    Vector3 secondLast = new Vector3(0, 0);
    Vector3 last = new Vector3(0, 0);
    Vector3 zeroVector = new Vector3(0, 0);
    Circle currentCircle = null;

    float minDistance = 0.1f;
    float lastAngle = 0;
    int count = 0;
    int recalculateThresh = 10;

    public void updateMenu(Vector3 hand)
    {
        if (hand == zeroVector)
        {
            return;
        }
        if (secondLast == zeroVector)
        {
            secondLast = hand;
            return;
        }
        if (last == zeroVector)
        {
            if (Vector3.Distance(secondLast, hand) > minDistance)
            {
                last = hand;
            }
            return;
        }
        if (Vector3.Distance(hand, last) < minDistance)
        {
            return;
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
            return;
        }

        double angle = calculateDirection(hand);
        updateIndexFromAngle(angle);
    }

    // finds which direction the user is moving their hand in
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

        if(angle > 360)
        {
            angle -= 360;
        }
        
        secondLast = last;
        last = hand;

        return angle;
    }
    
    void updateIndexFromAngle(double angle)
    {
        var index = (int)((360 - angle) / (360 / script.m_ButtonsNames.Length));

        if (index >= script.m_ButtonsNames.Length)
        {
            index = script.m_ButtonsNames.Length - 1;
        }
        if (index < 0)
        {
            index = 0;
        }

        script.m_SelectedIndex = index;
    }

    void resetMenu()
    {
        currentCircle = null;
        lastAngle = 0;
        secondLast = new Vector3(0, 0);
        last = new Vector3(0, 0);
    }


    public void selectOption()
    {
        handleMenuUpdate(script.m_ButtonsNames[script.m_SelectedIndex]);
        resetMenu();
    }

    public void handleKeyboard()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            if (!isMenuOpen)
            {
                openHomeMenu();
            } else
            {
                closeMenu();
            }
        }
    }
}
