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


    public GameObject windowContainer;
    public GameObject windowList;
    public GameObject openObject;

    public static Window window; //must be public to allow windowClick to work
    static SnapState snapState;
    static modes currentMode;
    static IotMenu menu;
    static WindowMenu windowMenu;

    LeftHand left;
    RightHand right;
    Audio audio;
    Gesture gesture;

    void Start()
    {
        snapState = new SnapState();
        window = new Window();
        currentMode = modes.NONE;
        menu = new IotMenu();
        windowMenu = new WindowMenu(window, windowContainer, windowList, openObject);
        left = new LeftHand();
        right = new RightHand(handObject);
    }

    public void Update()
    {
        handleGestures();
        checkResizing();
        
        if (currentMode == modes.RESIZING)
        {
            return;
        } else if (currentMode == modes.SELECT)
        {
            updateHandObject(right.position);
        } else if (currentMode == modes.MENU)
        {
            windowMenu.updateMenu(right.position);
        }

        if (menu.isMenuOpen)
        {
            menu.updateMenu(left.position);
        }

        if (window.currentWindow != null)
        {
            window.moveWindow(right.position);
        }       

        right.updatePrevious();
        right.updateFist();
        right.updateLasso();

        left.updatePrevious();
        left.updateLasso();
        left.updateFist();
    }

    class Hand
    {
        public int GESTURE_TIMEOUT = 60;
        public string lastState 
        { get; set; }
        public string currentState
        { get; set; }
        public Vector3 position
        { get; set; }
        public Vector3 lastPosition
        { get; set; }
        public bool canMove
        { get; set; }
        public int count
        { get; set; }

        public Hand()
        {
            canMove = true;
            position = new Vector3(0, 0);
            currentState = "";
            lastState = "";
            count = 0;
        }

        public void updateState()
        {
            lastState = currentState;
            count = 0;
        }

        public void updatePrevious()
        {
            if (currentState == lastState || currentState == "Unkown" && count < GESTURE_TIMEOUT)
            {
                count++;
                canMove = false;
            }
            else
            {
                lastState = "";
                canMove = true;
            }
        }

        public void updateLasso() { }
        public void updateFist() { }

    }
    class LeftHand : Hand
    {

        public void updateLasso()
        {
            if (currentState == "Lasso" && canMove)
            {
                if (menu.isMenuOpen)
                {
                    menu.selectOption();
                    updateState();
                }
            }
        }

        public void updateFist()
        {
            if (currentState == "Closed" && window.currentWindow == null && canMove)
            {

                if (snapState.detectedSnap)
                {
                    if (!menu.isMenuOpen)
                    {
                        Debug.LogError("opening menu");
                        menu.openHomeMenu();
                    }
                    else
                    {
                        Debug.LogError("closing window");
                        menu.closeMenu();
                    }
                    updateState();
                }
            }
        }
    }
    class RightHand : Hand
    {
        GameObject RightHandObject;
        public RightHand(GameObject HandObjectRef)
        {
            RightHandObject = HandObjectRef;
        }
        public void updateLasso()
        {
            if (currentState == "Lasso" && canMove)
            {
                if (currentMode == modes.MENU && window.currentWindow == null)
                {
                    windowMenu.selectMenuOption();
                    currentMode = modes.NONE;
                    updateState();
                    return;
                }
                else
                {
                    if (currentMode != modes.SELECT)
                    {
                        currentMode = modes.SELECT;
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
                            currentMode = modes.NONE;
                            updateState();
                        }
                    }
                }
            }
        }

        public void updateFist()
        {
            if (currentState == "Closed" && canMove)
            {

                if (currentMode == modes.SELECT)
                {
                    window.destroyWindow(window.currentWindow);
                    currentMode = modes.NONE;
                    updateState();
                    return;
                }

                if (lastState != "Closed")
                {
                    windowMenu.cancelMenu();
                    updateState();
                }
            }
        }
    }
    class SnapState
    {
        public float SNAP_THRESH = 2;
        public float snapTime
        { get; set; }

        public bool detectedSnap
        { get; set; }

        public SnapState()
        {
            snapTime = 0;
            detectedSnap = false;
        }
    }
    enum modes { MENU, SELECT, RESIZING, NONE}
    class WindowMenu : MonoBehaviour
    {
        Window window;
        GameObject windowContainer;
        GameObject windowList;
        GameObject openObject;

        Vector3 pastMenuPosition = new Vector3(0, 0);
        float minDistanceY = 0.1f;
        int menuIndex = 0;
        float currentMenuY = 0;

        public WindowMenu(Window windowRef, GameObject windowContainerRef, GameObject windowListRef, GameObject openObjectRef)
        {
            window = windowRef;
            windowContainer = windowContainerRef;
            windowList = windowListRef;
            openObject = openObjectRef;
        }

        public void cancelMenu()
        {
            if (window.currentWindow != null)
            {
                window.currentWindow = null;
            }
            else
            {
                if (openObject.activeSelf)
                {
                    closeMenu();
                }
                else
                {
                    createWindow();
                }
            }
        }
        void closeMenu()
        {
            pastMenuPosition = new Vector3(0, 0);
            currentMode = modes.NONE;
            currentMenuY = 0;
            openObject.SetActive(false);
        }
        public void updateMenu(Vector3 position)
        {
            if (pastMenuPosition == new Vector3(0, 0))
            {
                pastMenuPosition = position;
                return;
            }

            var delta = pastMenuPosition.y - position.y;
            if (Mathf.Abs(delta) < minDistanceY)
            {
                return;
            }

            currentMenuY += delta;

            windowList.transform.GetChild(menuIndex).GetComponent<Image>().color = new Color32(0, 0, 0, 100);
            menuIndex = (int)(windowList.transform.childCount * delta * 2); 


            if (menuIndex < 0)
            {
                menuIndex = 0;
            }

            if (menuIndex >= windowList.transform.childCount)
            {
                menuIndex = windowList.transform.childCount - 1;
            }

            windowList.transform.GetChild(menuIndex).GetComponent<Image>().color = new Color(1, 0, 0, 1);
        }

        public void selectMenuOption()
        {
            windowList.transform.GetChild(menuIndex).GetComponent<Button>().onClick.Invoke();
        }
        public void createWindow()
        {
            currentMode = modes.MENU;
            openObject.SetActive(true);
            StartCoroutine(initWindows());
        }
        IEnumerator initWindows()
        {
            yield return new WaitForSeconds(1);//WaitForEndOfFrame();
            foreach (var b in openObject.GetComponentsInChildren<Button>())
            {
                b.onClick.AddListener(delegate { StartCoroutine(takeAdd()); });
            }
        }
        IEnumerator takeAdd()
        {
            yield return new WaitForSeconds(1);//WaitForEndOfFrame();
            if (windowContainer.transform.childCount == 0)
                yield break;

            window.currentWindow = windowContainer.transform.GetChild(windowContainer.transform.childCount - 1).gameObject;
            closeMenu();
        }
    }

    public void checkSnap()
    {
        if (audio.didSnap())
        {
            snapState.detectedSnap = true;
            snapState.snapTime = 0;
        }

        if (snapState.detectedSnap)
        {
            snapState.snapTime += Time.deltaTime;

            if (snapState.snapTime > snapState.SNAP_THRESH)
            {
                snapState.detectedSnap = false;
                snapState.snapTime = 0;
            }
        }
    }

    bool checkResizing()
    {
        if (left.currentState == "Closed" && right.currentState == "Closed") //than you can resize the window
        {
            window.resizeWindow(10 * Vector3.Distance(right.position, left.position));
            return true;
        }
        else
        {
            if (currentMode == modes.RESIZING)
            {
                left.updateState();
            }
        }
        return false;
    }


    private void handleGestures()
    {
        checkSnap();

        left.currentState = gesture.LeftHandState;
        right.currentState = gesture.RightHandState;

        var posLeft = gesture.PosLeft;
        var posright = gesture.PosRight;

        right.position = new Vector3(posright.X, posright.Y, posright.Z);
        left.position = new Vector3(posLeft.X, posLeft.Y, posLeft.Z);
    }


    public GameObject handObject;
    float speed = 10;
    float handSens = 2.5f;
    public void updateHandObject(Vector3 position)
    {
        if (!handObject.activeSelf)
        {
            handObject.SetActive(true);
            handObject.transform.position = new Vector3(0, 0, 0);
            right.lastPosition = position;
            return;
        }    
        
        handObject.transform.position += new Vector3((-right.lastPosition.x + position.x) * speed * handSens, (-right.lastPosition.y + position.y) * speed * handSens, 0);//(handObject.transform.position.x, handObject.transform.position.y, 0); 
        handObject.transform.position = new Vector3(handObject.transform.position.x, handObject.transform.position.y, 0);

        right.lastPosition = position;
        
    }
}
