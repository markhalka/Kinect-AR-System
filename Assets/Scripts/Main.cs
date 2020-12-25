using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.UI;


using System.Text;
using System.Diagnostics;
using Debug = UnityEngine.Debug;

public class Main : MonoBehaviour
{

    public GameObject windowMenuObject;
    public GameObject IotMenu;
    public GameObject gestureObject;
    public GameObject handObject;

    WindowMenu windowMenu;

    LeftHand left;
    RightHand right;

    Gesture gesture;

    Hand[] hands;
    void Start()
    {
        mode.currentMode = modes.NONE;
        windowMenu = windowMenuObject.GetComponent<WindowMenu>();
        gesture = gestureObject.GetComponent<Gesture>();
        
        left = new LeftHand(IotMenu.GetComponent<IotMenu>());
        right = new RightHand(windowMenu, handObject);
        hands = new Hand[] { left, right };
    }


    public void Update()
    {
        handleGestures();

        WindowManager.checkResizing(right, left);

        if (mode.currentMode == modes.RESIZING)
        {
            return;
        }

        if (WindowManager.safeMode)
        {
            if (gesture.getBodyCount() > 1)
            {
                WindowManager.hideSafeWindows();
            }
            else
            {
                WindowManager.showSafeWindows();
            }
        }

        if (mode.currentMode == modes.MENU)
        {
            windowMenu.updateMenu(right.position);
        }

        WindowManager.moveCurrentWindow(right);
        if (handObject.activeSelf)
        {
            handObject.GetComponent<HandObject>().updateHandObject(right.position);
        }

        foreach (Hand hand in hands)
        {
            hand.updatePrevious();
            hand.update();
            hand.updateFist();
            hand.updateLasso();
            hand.updateSwipe();
        }
    }


    private void handleGestures()
    {
        left.currentState = gesture.LeftHandState;
        right.currentState = gesture.RightHandState;
        
        var posLeft = gesture.PosLeft;
        var posright = gesture.PosRight;

        right.position = new Vector3(posright.X, posright.Y, posright.Z);
        left.position = new Vector3(posLeft.X, posLeft.Y, posLeft.Z);
    }


   
}
