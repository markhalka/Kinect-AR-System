using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class RightHand : Hand
{

    WindowMenu windowMenu;
    HandObject handObject;
    GameObject handObjectGb;

    public RightHand(WindowMenu windowMenu, GameObject handObjectGb) : base()
    {
        this.handObjectGb = handObjectGb;
        handObject = handObjectGb.GetComponent<HandObject>();
        this.windowMenu = windowMenu;
    }

    public override void update()
    {
        if (menu_count > HAND_THRESH && currentState != P_HandState.CLOSED)
        {
            if (mode.currentMode != modes.SELECT)
            {
                if (!handObjectGb.activeSelf && WindowManager.currentWindow == null)
                {
                    handObjectGb.SetActive(true);
                    mode.currentMode = modes.SELECT;
                    updateState();
                    menu_count = 0;
                }
            } 
        }
    }

    public override void updateLasso()
    {
       /* if (currentState == "Lasso" && canMove)
        {
            if (mode.currentMode == modes.MENU && window.currentWindow == null)
            {
                windowMenu.selectMenuOption();
                mode.currentMode = modes.NONE;
                updateState();
                return;
            }
            else
            {
                if (mode.currentMode != modes.SELECT)
                {
                    mode.currentMode = modes.SELECT;
                    updateState();
                }
                else
                {
                    if (window.currentWindow == null)
                    {
                        window.selectWindow();
                    }
                    else
                    {
                        RightHandObject.SetActive(false);
                        mode.currentMode = modes.NONE;
                        updateState();
                    }
                }
            }
        }*/
    }

    const float MENU_THRESH = 2;
    const float HAND_THRESH = 0.1f;
    float menu_count = 0;
    public override void updateFist()
    {
        if (currentState == P_HandState.CLOSED && canMove)
        {
            if(mode.currentMode == modes.MENU)
            {
                windowMenu.selectMenuOption();
                mode.currentMode = modes.SELECT;
            } else if(mode.currentMode == modes.SELECT)
            {
                handObject.selectWindow();
            }
            else 
            {
                menu_count += Time.deltaTime;
                if(menu_count >= MENU_THRESH)
                {
                    windowMenu.toggle();
                    updateState();
                    menu_count = 0;
                }              
            }            
        }
    }

    int count = 0;
    public override void updateSwipe()
    {
        if (mode.currentMode == modes.SELECT && canMove)
        {
            count++;
            if (count % SwipingGesture.THRESH == 0)
            {
                if (SwipingGesture.checkVector(position, true)) //THIS SHOULD BE TRUE BUT MY RIGHT HAND HURTS
                {
                    handObjectGb.SetActive(false);
                    mode.currentMode = modes.NONE;
                    updateState();
                }
                count = 0;
            }
        }
    }

    /*
     *  if (mode.currentMode == modes.SELECT)
            {
                windowMenu.destroyCurrentWindow();
                mode.currentMode = modes.NONE;
                updateState();
                return;
            }
     */
}
