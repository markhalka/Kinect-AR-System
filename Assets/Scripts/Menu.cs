using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Windows.Kinect;
using System;
using System.Globalization;


//4. start working on the sound class 


//ok, so with the new joint code that you just wrote, you can track the right hand, you can use that to see where they are moving the object, and what their hand is over currently


//you need to make this code perfect, and ready for when you buy the projector and kinect tommorow 


//when you snap it should call openHomeMenu();

//when you move your arm, it should tell in which direction, and update that based on the angle of the arm moved
//call the function every 0.25 seconds, find the delta of the rotation in euler angles, and update the selection based on that 

//you can also use the steering example in the unity gestures 

//use gesture example click to select something (or maybe lasso?) but thats for the selection

//use the unity example to resize

//when you close your fist, it should deselect, or exit the current thing 
//you can use this from the basic gesture detection


//you can also use swipe left, or swipe right for something 


//when you wave, the system can turn on or off




//in the settings have a left and right hand option 









public class Menu : MonoBehaviour
{

    public GameObject menuContainer;
    public bool isMenuOpen = false;

    //when you click on a button, it will call a method here and pass its value
    //have a function that reads the value and decides what to do next based on that 


    //the current index of the menu, with the homemenu being 0
    public int menuIndex;
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
    public void handleMenuUpdate(string thing)
    {
        switch (currentMenu)
        {
            case menus.Home:
                switch (thing)
                {
                    //home menu options
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
                //view cameras:
                //based on how many current video feeds they have just have windows representing those feeds 

            //edit cameras:
            //1. change settings for the cameras:
            //a. have a list of names of cameras, have the status, have what it does, and where it is 
            //b. you can enable/disable cameras, and change what it does (where it sends the stream, and if it is apart of the different systems)

            //settings:
            //no clue

            //recordings:
            //1. just have the day and time over clips, you can swipe left or right, select to watch something, star it, or delete it 

            case menus.Security:
                switch (thing)
                {
                    case "View Cameras":
                        break;
                  /*  case "Edit Cameras":
                        openMenu("Edit Camera", new string[] { "Enable", "Disable", "Remove", "Change stream"});
                        break;*/
                    case "Settings":
                        openMenu("Camera Settings", new string[] { "Add Camera", "Enable All", "Disable All" });
                        break;
                    case "Recordings":
                        break;
                }
                break;

                //lights:
                //1. have a list of rooms (3d models)
                //2. on the right side, you have all the lights, you can swipe up or down to select them, and the lights are hilighted in the room (rooms) 
                //3. when you select a light, you can change the light level, and the light level is automatically updated in the 3d model of the room as well

            //door
            //4. the same 3d models of rooms are shown, same format, but it hilights the doors
            //5. the state of the door is shown in the 3d model, when you select a door you can see its status, and if you can, it gives you the option to open or close it

            //windows
            //same thing here 

            //edit devices:
            //1. same 3d model, but in the list it shows all 3d models (even the ones where the location is not certain )
            //2. for each 3d model you can see its status, what its connected to, and you can edit/change what systems its connected to or disable it, or remove it
            //3. you can also add a new device (work on that latter)

            //settings;
            //again, no idea 
            case menus.IOT:
                switch (thing)
                {
                    //these are all dependant on the room
                  /*  case "Lights": //slider
                        break;
                    case "Door": //open, close, lock, unlock (only shows the relevant ones)
                        break;
                    case "Windows": // -||-
                        break;
                    case "Blinds": //slider
                        break; */
                    case "Select Room": //just a list of the rooms (if there is more than 8, than there is a button that says more, which shows the rest of the rooms)
                        break;
                    case "Edit Devices": 
                        break;
                    case "Settings":
                        break;
                }
                break;

                //brightness
                //1. just shows a slider that you can select, and it changes the projector brighntes
                
            //sensor recalibration:
            //1. it opens the projector mapper thing, but it automates the whole process, and does everything automatically

            //device info
            //1. just says the status, update version, and a few extra "about us" info

            //gesture settings:
            //1. have a list of current gestures, the names, and maybe an animation of what it looks like
            //2. below it should say its status, and what it activates
            //3. there should be a button on the upper right that lets you create your own gesture, and assign what they do

            case menus.Settings:
                switch (thing)
                {
                    case "Brightness": //slider
                        break;
                    case "Sensor recalibration": //nothing
                        break;
                    case "Device info": //just some text
                        break;
                    case "Computer Synching"://button 
                        break;
                    case "Gesture settings"://todo later
                        break;
                }
                break;

                //1. 
            case menus.Effects:
                switch (thing)
                { 
                    case "View Effects":
                        break;
                    case "Create Effects":
                        break;
                }
                break;
        }
    }

    //ok, so now just make kindof the skeleton code for everything, no need to over complicate it 
    //this should take no more than an hour

    //than work on improving the gestures, finish that up, than you can start using the projection mapping with it as well 

    //you can also start thinking about the iot stuff

    //and you can take a look at c through walls as well

    //depending on what menu it is, call the right handle menu


    //this will call the correct update function for the right submenu

    //so basically when they are called, 
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
    //opens the menu
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
        if (!script.closeOnDeactivate)
        {
            showMenu(name, values);
        }
       

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

    //here is what you should do:
    //1. every three points, create the new circle
    //2. if it is substantially different, than redefine the cirlce, otherwise just use the point on the circle closest to the users point 
    //3. calculate the change in the angle from the previous start point, if it is larger than some threshold, than just use the next thing 

    //the first point will be the beggining of the circle, if something is cycled, than that is the new beggining of the circle

    Vector3 secondLast = new Vector3(0, 0);
    Vector3 last = new Vector3(0, 0);
    Vector3 zeroVector = new Vector3(0, 0);
    Circle currentCircle = null;
    float minDistance = 0.1f;


    float lastAngle = 0;
    int count = 0;
    int recalculateThresh = 10;
    public void calculateDirection(Vector3 hand)
    {

        if(hand == zeroVector)
        {
            return;
        }

        if(secondLast == zeroVector)
        {
            secondLast = hand;
            return;
        }
        if(last == zeroVector)
        {
            if(Vector3.Distance(secondLast, hand) > minDistance)
            {
                last = hand;
            }    
            return;
        }
        if(Vector3.Distance(hand, last) < minDistance)
        {
            return;
        }
        //Debug.LogError("calculating circle...");

      //  count++;

        if(currentCircle == null || count > recalculateThresh)
        {
            Vector3 circle = findCircle(secondLast, last, hand);
            currentCircle = new Circle();
            currentCircle.centerX = circle.x;
            currentCircle.centerY = circle.y;
            currentCircle.radius = circle.z;
            currentCircle.begin = secondLast;
            count = 0;
            return;
        } else
        {
            //check if the difference is huge
        }
        //find the point closest to the current circle

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

        var index = (int)((360-angle) / (360 / script.m_ButtonsNames.Length));

        if(index >= script.m_ButtonsNames.Length)
        {
            index = script.m_ButtonsNames.Length - 1;
        }
        if(index < 0)
        {
            index = 0;
        }

        script.m_SelectedIndex = index;
        Debug.LogError(index + " index");
        secondLast = last;
        last = hand;

    }

    void resetMenu()
    {
        currentCircle = null;
        lastAngle = 0;
        secondLast = new Vector3(0, 0);
        last = new Vector3(0, 0);
    }


    //next option in the menu, the direction indicates clockwise or counterclockwise
    public IEnumerator nextOption(int direction) { yield break; }


    //selects the current option
    public void selectOption()
    {
        //just invoke the curren selected option
        Debug.LogError("selecting: " + script.m_ButtonsNames[script.m_SelectedIndex]);
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
