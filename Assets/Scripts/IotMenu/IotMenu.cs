using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Windows.Kinect;
using System;
using System.Globalization;
using UnityEngine.Video;


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

    RadialMenu script;
    MenuOption home;
    void Start()
    {
        script = menuContainer.GetComponent<RadialMenu>();
        buttonCall = "";
        isMenuOpen = false;
        home = new Home();
        currentOption = home;
        getSubMenuNames(home);
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

    MenuOption currentOption;
    public GameObject thermostat;
    public GameObject video;
    public void handleMenuUpdate(string menuName)
    {



        currentOption = getOptionFromName(menuName);
        var options = getSubMenuNames(currentOption);
        if(options.Length == 0)
        {
            currentOption.handle();
            return;
        }

        openMenu(currentOption.name, options);
    }


    public string[] getSubMenuNames(MenuOption thing)
    {
        List<string> output = new List<string>();
        if(thing.submenus == null)
        {
            return new string[0];
        }
        for(int i = 0; i < thing.submenus.Count; i++)
        {
            output.Add(thing.submenus[i].name);
        }
        return output.ToArray();
    }

    public MenuOption getOptionFromName(string name)
    {
        if(currentOption.submenus == null)
        {
            return null;
        }

        foreach(MenuOption option in currentOption.submenus)
        {
            if (option.name == name)
            {
                return option;
            }
        }
        return null;
    }

    public void openHomeMenu()
    {
        mode.currentMode = modes.IOT_MENU;
        currentOption = home;
        openMenu(home.name, getSubMenuNames(home));
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
        mode.currentMode = modes.NONE;
        isMenuOpen = false;
        menuContainer.GetComponent<RadialMenu>().closeOnDeactivate = true;
        menuContainer.GetComponent<RadialMenu>().DeactivateMenu();
        currAngle = 0;
    }

    Vector3 prev = new Vector3(0, 0, 0);
    double currAngle = 0;
    double sensitivity = 20f;
    public void updateMenu(Vector3 hand)
    {
        if(prev == new Vector3(0, 0, 0))
        {
            prev = hand;
            return;
        }

        var dot = Vector3.Dot(hand, prev);
        var det = hand.x * prev.y - hand.y * prev.x;
        var angle = Mathf.Rad2Deg * Math.Atan2(det, dot) * sensitivity;

        currAngle += angle;

        if (currAngle < 0)
        {
            currAngle += 360;
        }

        if(currAngle > 360)
        {
            currAngle -= 360;
        }

        updateIndexFromAngle(currAngle);

        prev = hand;
    }
    
    // finds which direction the user is moving their hand in
    public void next()
    {
        script.m_SelectedIndex = normalizeIndex(script.m_SelectedIndex + 1);
    }

    public void back()
    {
        script.m_SelectedIndex = normalizeIndex(script.m_SelectedIndex - 1);
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

    int normalizeIndex(int index)
    {
        if(index < 0)
        {
            index += script.m_ButtonsNames.Length;
        }

        if(index >= script.m_ButtonsNames.Length)
        {
            index -= script.m_ButtonsNames.Length;
        }

        return index;
    }


    public void selectOption()
    {
        handleMenuUpdate(script.m_ButtonsNames[script.m_SelectedIndex]);
        currAngle = 0;
    }

    public void selectOption(string option)
    {
        if(currentOption.submenus == null)
        {
            Debug.LogError("cannot select option because there are none");
            return;
        }

        foreach(var menuOption in currentOption.submenus)
        {
            if(option.ToLower() == menuOption.name.ToLower())
            {
                handleMenuUpdate(menuOption.name);
            }
        }
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
