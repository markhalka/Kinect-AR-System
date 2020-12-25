using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class LeftHand : Hand
{

    IotMenu menu;
    SnapState snapState;
    public LeftHand(IotMenu menu) : base()
    {
        this.menu = menu;
        snapState = new SnapState();
    }

    public override void update()
    {
        snapState.checkSnap();
        if (menu.isMenuOpen)
        {
            menu.updateMenu(position);
        }
    }

    public override void updateFist()
    {
        if (currentState == P_HandState.CLOSED && WindowManager.currentWindow == null && canMove)
        {
            snapState.detectedSnap = true;
            if (snapState.detectedSnap)
            {
                if (!menu.isMenuOpen)
                {
                    menu.openHomeMenu();
                    SwipingGesture.resetLeft(position); 
                }
                else
                {
                    menu.selectOption();
                    updateState();
                }
                updateState();
            }
        }
    }
    int count = 0;
   

    public override void updateSwipe()
    {
        if (menu.isMenuOpen && canMove)
        {
            count++;
            if (count % SwipingGesture.THRESH == 0)
            {
                if (SwipingGesture.checkVector(position, false))
                {
                    menu.closeMenu();
                    updateState();
                }
                count = 0;             
            }
        }
    }
}
