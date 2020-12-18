using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Windows.Kinect;
using System;
using System.Globalization;


// todo for this shit:
//1. read throught all iot menu stuff, and look at the hand gesture code, make sure it works as intended
//2. read throught the hand gesture code again (why not)
//3. double check the window interactions, especially movment, resizing and selecting, fix the code there 
//4. maybe take a look at making it voice controlled, import ibm watson, have speech to text, add keywords done 


public class IotMenu : MonoBehaviour
{

    public GameObject menuContainer;
    public bool isMenuOpen = false;
    public static string buttonCall;
    public Circle circle;

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


    void updateMenu()
    {
        double angle = circle.getAngle(hand);

        if(angle == 0)
        {
            return;
        }

        updateIndexFromAngle(angle);
    }
    
    // finds which direction the user is moving their hand in

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
        circle.resetCircle();
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
