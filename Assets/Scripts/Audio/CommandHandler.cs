using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandHandler : MonoBehaviour
{
    IotMenu iotMenu;
    public GameObject IotMenuGb;
    public GameObject windowMenuGb;
    public GameObject handObjectGb;
    public GameObject calanderImage;

    WindowMenu windowMenu;
    HandObject handObject;

    public AudioSource source;
    public AudioClip errorMessage;
    public AudioClip selectMessage;
    public AudioClip noCommand;
    public AudioClip completedSound;

    public void Start()
    {
        iotMenu = IotMenuGb.GetComponent<IotMenu>();
        windowMenu = windowMenuGb.GetComponent<WindowMenu>();
        handObject = handObjectGb.GetComponent<HandObject>();
    }

    public void openMenu()
    {
        if (mode.currentMode != modes.IOT_MENU)
        {
            iotMenu.openHomeMenu();
            completed();
        }
        else
        {
            commandError();
        }
    }
    public void closeMenu()
    {
        if (mode.currentMode == modes.IOT_MENU)
        {
            iotMenu.closeMenu();
            completed();
        }
        else
        {
            commandError();
        }
    }
    public void next()
    {
        if (mode.currentMode == modes.IOT_MENU)
        {
            iotMenu.next();
            completed();
        }
        else
        {
            commandError();
        }
    }
    public void back()
    {
        if (mode.currentMode == modes.IOT_MENU)
        {
            iotMenu.back();
            completed();
        }
        else
        {
            commandError();
        }
    }
    public void openWindow()
    {
        if (!windowMenu.openObject.activeSelf)
        {
            windowMenu.toggle();
            completed();
        }
        else
        {
            commandError();
        }
    }
    public void closeWindow()
    {
        if (windowMenu.openObject.activeSelf)
        {
            windowMenu.toggle();
            completed();
        }
        else
        {
            commandError();
        }
    }
    public void up()
    {
        if (mode.currentMode == modes.MENU)
        {
            windowMenu.up();
        }
        else
        {
            commandError();
        }
    }
    public void down()
    {
        if (mode.currentMode == modes.MENU)
        {
            windowMenu.down();
        }
        else
        {
            commandError();
        }
    }
    public void showCursor()
    {
        handObjectGb.SetActive(true);
    }
    public void hideCursor()
    {
        handObjectGb.SetActive(false);
    }
    public void delete()
    {
        if (WindowManager.currentWindow != null)
        {
            WindowManager.destroyCurrentWindow();
            completed();
        }
        else
        {
            selectError();
        }

    }
    public void grow()
    {
        if (WindowManager.currentWindow != null)
        {
            WindowManager.resizeWindow(WindowManager.currentWindow.GetComponent<Window>(), 0.1f);
            completed();
        }
        else
        {
            selectError();
        }
    }
    public void shrink()
    {
        if (WindowManager.currentWindow != null)
        {
            WindowManager.resizeWindow(WindowManager.currentWindow.GetComponent<Window>(), -0.1f);
            completed();
        }
        else
        {
            selectError();
        }
    }

    public void safeModeOn()
    {
        WindowManager.safeMode = true;
    }

    public void safeModeOff()
    {
        WindowManager.safeMode = false;
    }

    public void showSafeWindows()
    {
        windowMenu.showSafeWindows();
    }

    public void hideSafeWindows()
    {
        windowMenu.hideSafeWindows();
    }

    public void select()
    {
        if (mode.currentMode == modes.MENU)
        {
            windowMenu.selectMenuOption();
        }
        else if (mode.currentMode == modes.IOT_MENU)
        {
            iotMenu.selectOption();
        }
        else
        {
            commandError();
        }
    }

    public void commandError() //"the command could not be executed, please ensure you are using the right commnad"
    {
        source.clip = errorMessage;
        source.Play();
    }

    public void selectError() //"The command can only be exectued when a window is selected"
    {
        source.clip = selectMessage;
        source.Play();
    }

    public void audioError() //"command not found, please ensure you are using a valid command, and repeat"
    {
        source.clip = noCommand;
        source.Play();
    }

    public void completed() //"just a 'bing' sound"
    {
        source.clip = completedSound;
        source.Play();
    }

    public void showIds()
    {
        windowMenu.toggleIds(true);
    }

    public void hideIds()
    {
        windowMenu.toggleIds(false);
    }

    public void selectIotName(string name)
    {
        iotMenu.selectOption(name);
    }

    public bool selectId(int id)
    {
        return windowMenu.selectId(id);
    }

    public void calendar()
    {
        calanderImage.SetActive(true);
    }
}

