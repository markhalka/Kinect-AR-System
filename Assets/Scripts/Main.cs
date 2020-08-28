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





        //3. fix the hand
        //1. this is partially fixed in the first 2
        //2. make the movment smaller (so it is less jittery)
        //3. make sure that the hand only moves on 2 axis

        //4. get the projection mapping working well
        //1. try and get the wobble effect working well (that would be cool)




    public Audio audio;
    public Gesture gesture;
    public Menu menu;

    public GameObject windowContainer;
    public GameObject windowList;
    public static GameObject currentWindow = null;

    

    void Start()
    {
        Debug.LogError("here");
      /*  Process[] processlist = Process.GetProcesses();
        Debug.LogError(processlist.Length + " lenght");
        foreach (Process process in processlist)
        {
            if (!string.IsNullOrEmpty(process.MainWindowTitle))
            {
                Debug.LogError("Process: " + process.ProcessName + " ID:  " + process.Id + "Window title: " + process.MainWindowTitle);
            }
        } */
         CloseFirefoxWindow();
    }

    #region minimizeWindows





        public static void CloseFirefoxWindow()
        {
            string processName = "PaintDotNet";
            CloseWindow(processName);
        }


        public static void CloseWindow(string processName)
        {
            Process[] processes = Process.GetProcessesByName(processName);
        Debug.LogError(processes.Length + " length");
            if (processes.Length > 0)
            {
                foreach (var process in processes)
                {
                    IDictionary<IntPtr, string> windows = List_Windows_By_PID(process.Id);
                    foreach (KeyValuePair<IntPtr, string> pair in windows)
                    {
                        var placement = new WINDOWPLACEMENT();
                        GetWindowPlacement(pair.Key, ref placement);

                        if (placement.showCmd == SW_SHOWMINIMIZED)
                        {
                        Debug.LogError("maxamizing");
                            //if minimized, show maximized
                            ShowWindowAsync(pair.Key, SW_SHOWMAXIMIZED);
                        }
                        else
                        {
                        Debug.LogError("minimizing");
                        //default to minimize
                        ShowWindowAsync(pair.Key, SW_SHOWMINIMIZED);
                        }
                    }
                }
            }
        }


        private const int SW_SHOWNORMAL = 1;
        private const int SW_SHOWMINIMIZED = 2;
        private const int SW_SHOWMAXIMIZED = 3;

        private struct WINDOWPLACEMENT
        {
            public int length;
            public int flags;
            public int showCmd;
            public System.Drawing.Point ptMinPosition;
            public System.Drawing.Point ptMaxPosition;
            public System.Drawing.Rectangle rcNormalPosition;
        }

        [DllImport("user32.dll")]
        private static extern bool ShowWindowAsync(IntPtr hWnd, int nCmdShow);

        private delegate bool EnumWindowsProc(IntPtr hWnd, int lParam);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint lpdwProcessId);

        [DllImport("USER32.DLL")]
        private static extern bool EnumWindows(EnumWindowsProc enumFunc, int lParam);

        [DllImport("USER32.DLL")]
        private static extern int GetWindowText(IntPtr hWnd, StringBuilder lpString, int nMaxCount);

        [DllImport("USER32.DLL")]
        private static extern int GetWindowTextLength(IntPtr hWnd);

        [DllImport("USER32.DLL")]
        private static extern bool IsWindowVisible(IntPtr hWnd);

        [DllImport("USER32.DLL")]
        private static extern IntPtr GetShellWindow();

        [DllImport("USER32.DLL")]
        private static extern bool GetWindowPlacement(IntPtr hWnd, ref WINDOWPLACEMENT lpwndpl);


    public static IDictionary<IntPtr, string> List_Windows_By_PID(int processID)
    {
        IntPtr hShellWindow = GetShellWindow();
        Dictionary<IntPtr, string> dictWindows = new Dictionary<IntPtr, string>();

        EnumWindows(delegate (IntPtr hWnd, int lParam)
        {
                //ignore the shell window
                if (hWnd == hShellWindow)
            {
                return true;
            }

                //ignore non-visible windows
                if (!IsWindowVisible(hWnd))
            {
                return true;
            }

                //ignore windows with no text
                int length = GetWindowTextLength(hWnd);
            if (length == 0)
            {
                return true;
            }

            uint windowPid;
            GetWindowThreadProcessId(hWnd, out windowPid);

                //ignore windows from a different process
                if (windowPid != processID)
            {
                return true;
            }

            StringBuilder stringBuilder = new StringBuilder(length);
            GetWindowText(hWnd, stringBuilder, length + 1);
            dictWindows.Add(hWnd, stringBuilder.ToString());

            return true;

        }, 0);

        return dictWindows;
    }
    

#endregion

IEnumerator initWindows()
    {
        yield return new WaitForSeconds(1);//WaitForEndOfFrame();
        foreach(var b in openObject.GetComponentsInChildren<Button>())
        {
            b.onClick.AddListener(delegate { StartCoroutine(takeAdd()); });
        }
       /* Transform curr = openObject.transform.GetChild(0).GetChild(0).GetChild(0);
        for(int i = 0; i < curr.childCount; i++)
        {
            Debug.LogError("added something");
            curr.GetChild(i).GetComponentInChildren<Button>().onClick.AddListener(delegate { takeAdd(); });
        } */
    }

    IEnumerator takeAdd()
    {
        yield return new WaitForSeconds(1);//WaitForEndOfFrame();

        if (windowContainer.transform.childCount == 0)
            yield break;

        currentWindow = windowContainer.transform.GetChild(windowContainer.transform.childCount-1).gameObject;
        Debug.LogError(currentWindow.name + " is the current selected window");
        closeMenu();

    }

    void click3D()
    {
        if (Input.GetMouseButtonDown(0))
        {

            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (UnityEngine.Physics.Raycast(ray, out hit))
            {

                for (int i = 0; i < windowContainer.transform.childCount; i++)
                {
                    if (windowContainer.transform.GetChild(i).name == hit.transform.gameObject.name)
                    {

                        currentWindow = windowContainer.transform.GetChild(i).gameObject;
                        Debug.LogError("selected: " + currentWindow.name + "!");
                        break;
                    }
                }
            }
        }
    }

    Vector2 windowVec = new Vector2(0, 0);
    Vector2 nullVector = new Vector2(0, 0);
    void Update()
    { 

      
        handleKeyboardInput();
        handleGestures();
  
        
      //  click3D();
    }

    public Canvas canvas;
    public Vector3 mousePosition()
    {
        /*   Vector3 pos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 1);
           Vector2 tempOut = new Vector2(0, 0);

           RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.GetComponent<RectTransform>(), Input.mousePosition, Camera.main, out tempOut);
           return tempOut;*/

        Vector3 mousePos = Input.mousePosition;
        mousePos.z = Camera.main.nearClipPlane;
        return Camera.main.ScreenToWorldPoint(mousePos);
    }




    //ok, now you need to be able to move windows around, and select the right ones
    //to select, you need the position and orentiation of your hand

    //test the position and orientation of hand object, and raycasting 


    //test selecting a window 
    //test deleting the window



    //than you need to make the submenu shit, and get all that working

    //than you need to add better gestures

    //ok, so for the updating position, instead of having absolute values, just add the delta
    //if the delta is 0 for a long time, rest the object to have 0,0 position and no rotation 

    //when creating the window, select which one you want just by moving your hand up or down 

    //then when it is created, move the window to 0,0, and move it based on the delta of the position of the users hand

    //then fist wil deselect the current menu

    //ok, what you can do instead is just have a 2d dot on the screen, which is moved by the position of the hand (leave the rotation for now)

    //ok, so here what you need to do next


    //3. add the sub menus
    //4. begin making the gestures better

    //the system activates / deactivates when you snap (to keep it from constatnly looking for gestures)


    //try this out with the projector 
    //start adding some effects as well (try and get wobble to work)
    //


    //todo:
    
    //3. redo the calulations for the circle in left menu

        //to make the windows 

        //ok, so when a window is selected, have a command that sets it into interactable mode


   //ok, so now todo:
   //1. find the pid of all the windows shown in that list
   //2. if they are selected, make sure they are maximized
   //3. 


     
       


        //ok, keep the timeout, just have one hand that you keep track of, and just make sure that for certain gestuers the same one doesnt appear twice 




    int gestureTimeout = 60;
    int gestureCountLeft = 0;
    int gestureCountRight = 0;

    string lastLeft = "";
    string lastRight = "";

    bool selectMode = false;
    bool menuMode = false;
    bool wasResizing = false;
    public float snapTime = 0;
    public bool detectedSnap = false;
    public float snapThresh = 2;

    bool canLeft = true;
    bool canRight = true;

    public void handleGestures()
    {
        if (audio.didSnap())
        {
            detectedSnap = true;
            snapTime = 0;
        }

        if (detectedSnap)
        {
            snapTime += Time.deltaTime;
           
            if(snapTime > snapThresh)
            {
                detectedSnap = false;
                snapTime = 0;
            }
        }
        string left = gesture.leftHandState;
        string right = gesture.rightHandState;

        var posLeft = gesture.posLeft;
        var posright = gesture.posRight;
        var rightVec = new Vector3(posright.X, posright.Y, posright.Z);
        var leftVec = new Vector3(posLeft.X, posLeft.Y, posLeft.Z);
        var rotRight = gesture.rotRight;

        if (left == "Closed" && right == "Closed") //than you can resize the window
        {
            Debug.LogError("resizing");
            resizeWindow(10 * Vector3.Distance(rightVec, leftVec));
            wasResizing = true;
            return;
        }
        else
        {
            if (wasResizing)
            {
                gestureCountLeft = 0;
                lastLeft = left;
                
                wasResizing = false;
            }
        }

        if (selectMode)
        {
            updateHandObject(rightVec, new Quaternion(rotRight.X, rotRight.Y, rotRight.Z, rotRight.W));
        }

        if (menuMode)
        {
            updateMenu(rightVec);
        }


        if (menu.isMenuOpen)
        {
            menu.calculateDirection(leftVec);
        }

        if (currentWindow != null)
        {
            Debug.LogError("moving window...");
            moveWindow(rightVec);
            
        }

        if (left == lastLeft || left == "Unkown" && gestureCountLeft < gestureTimeout)
        {
            
            gestureCountLeft++;
            canLeft = false;
        } else
        {
            lastLeft = "";
            canLeft = true;
        }

        if (right == lastRight || right == "Unkown" && gestureCountRight < gestureTimeout)
        {
            gestureCountRight++;
            canRight = false;
        }
        else
        {
            lastRight = "";
            canRight = true;
        }

        if (left == "Lasso" && canLeft)
        {
            if (menu.isMenuOpen)
            {
                menu.selectOption();

                lastLeft = left;
                gestureCountLeft = 0;          
            }                
        }

        if (right == "Lasso" && canRight)
        {
            Debug.LogError("right lasso");
            if (menuMode && currentWindow == null)
            {
                Debug.LogError("selected option");
                selectMenuOption();
                menuMode = false;

                lastRight = right;
                gestureCountRight = 0;
             
                return;
            } else
            {
                if (selectMode == false)
                {
                    selectMode = true;

                    lastRight = right;
                    gestureCountRight = 0;
                } else
                {
                    if(currentWindow == null)
                    {
                        checkSelect();
                    } else
                    {
                        handObject.SetActive(false);
                        selectMode = false;

                        lastRight = right;
                        gestureCountRight = 0;

                    }    
                }              
            } 
        }

    
      
        if(left == "Closed" && currentWindow == null && canLeft)
        {
            
            if (detectedSnap)
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

                lastLeft = left;
                gestureCountLeft = 0;
            }     
        }  

        if (right == "Closed" && canRight)
        {
            
            if (selectMode)
            {
                if(currentWindow != null)
                {
                    destroyWindow(currentWindow);
                }
                handObject.SetActive(false);
                selectMode = false;

                lastRight = right;
                gestureCountRight = 0;
                return;
            }
            
            if (lastRight != "Closed")
            {
                if (currentWindow != null)
                {
                    currentWindow = null;
                   // Debug.LogError("would close window");
                    //destroyWindow(currentWindow); //close the current window
                }
                else
                {
                    if (openObject.activeSelf)
                    {
                        closeMenu();
                    }
                    else
                    {
                        Debug.LogError("created window menu again");
                        createWindow();
                    }
                }

                lastRight = right;
                gestureCountRight = 0;
            }         
        }
    }

    void selectMenuOption()
    {
     
        windowList.transform.GetChild(menuIndex).GetComponent<Button>().onClick.Invoke();
        
    }

    void closeMenu()
    {
        pastMenuPosition = new Vector3(0, 0);
        menuMode = false;
        currentMenuY = 0;
        openObject.SetActive(false);
    }

    //here, depending on if the hand is going up or down, just select different things 
    Vector3 pastMenuPosition = new Vector3(0, 0);
    float minDistanceY = 0.1f;
    int menuIndex = 0;
    float currentMenuY = 0;
    void updateMenu(Vector3 position)
    {
        if(pastMenuPosition == new Vector3(0, 0))
        {
            pastMenuPosition = position;
            return;
        }
 
        var delta = pastMenuPosition.y - position.y;
        if(Mathf.Abs(delta) < minDistanceY)
        {
            return;
        }

        currentMenuY += delta;

        windowList.transform.GetChild(menuIndex).GetComponent<Image>().color = new Color32(0, 0, 0, 100);
        menuIndex = (int)(windowList.transform.childCount * delta * 2); //not -1 becasue the first one is the default shit 


        if (menuIndex < 0)
        {
            menuIndex = 0;
        }

        if(menuIndex >= windowList.transform.childCount)
        {
            menuIndex = windowList.transform.childCount - 1;
        }

        Debug.LogError(menuIndex + " current menu index");


        windowList.transform.GetChild(menuIndex).GetComponent<Image>().color = new Color(1, 0, 0, 1);

    }

    void startSelectMode()
    {
        selectMode = true;
        //hide everything else
        
    }

    void endSelectMode()
    {
        selectMode = false;
        currentWindow.GetComponent<Image>().color = new Color32(0, 0, 0, 100);
    }

    void selectWindow(GameObject curr)
    {
        if(curr.GetComponent<Image>() != null)
        curr.GetComponent<Image>().color = new Color(1, 0, 0, 1);
        currentWindow = curr;
    }

    public GameObject handObject;
    Vector3 previousPosition;
    Vector3 previousRotation;
    float minPosChange = 0.05f;
    float minRotChange = 10;
    float maxRotChange = 90; //to stop the annoying axis flipping 
    int minThresh = 50;
    int minCount = 0;
    float speed = 10;
    float handSens = 2.5f;
    public void updateHandObject(Vector3 position, Quaternion rotation)
    {
        if (!handObject.activeSelf)
        {

            handObject.SetActive(true);
            handObject.transform.position = new Vector3(0, 0, 0);
            previousPosition = position;
            return;
        }
        /*  if(minCount > minThresh)
          {
              //reset position
              handObject.transform.position = new Vector3(0, 0, 0);
              handObject.transform.rotation = Quaternion.Euler(0, 0, 0);
              minCount = 0;
              return;
          }
          if(Vector3.Distance(position, handObject.transform.position) < minPosChange)
          {
              minCount++;
              return;
          }
          var currRot = rotation.eulerAngles;
          var change = Vector3.Distance(previousRotation, currRot);
          if (change < minRotChange || change > maxRotChange)
          {
              minCount++;
              return;
          }

        var currRot = rotation.eulerAngles;
        var change = Vector3.Distance(previousRotation, currRot);
        */
        
       // handObject.transform.Translate((previousPosition - position) * speed * handSens);
        handObject.transform.position += new Vector3((-previousPosition.x + position.x) * speed * handSens, (-previousPosition.y + position.y) * speed * handSens, 0);//(handObject.transform.position.x, handObject.transform.position.y, 0); 
        handObject.transform.position = new Vector3(handObject.transform.position.x, handObject.transform.position.y, 0);

        // handObject.transform.Rotate(previousRotation - currRot);

        //   previousRotation = currRot;
        previousPosition = position;
        
    }
    public void checkSelect()
    {
        RaycastHit hit;
        // Does the ray intersect any objects excluding the player layer
        if (Physics.Raycast(handObject.transform.position, handObject.transform.TransformDirection(Vector3.down), out hit, Mathf.Infinity))
        {
            Debug.DrawRay(handObject.transform.position, handObject.transform.TransformDirection(Vector3.down) * hit.distance, Color.yellow);
            selectWindow(hit.collider.gameObject);
        }
        else
        {
            Debug.DrawRay(handObject.transform.position, handObject.transform.TransformDirection(Vector3.down) * 1000, Color.white);
            Debug.Log("Did not Hit");
        }
    }


    public void handleKeyboardInput()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            createWindow();
        }
        if (Input.GetKeyDown(KeyCode.M))
        {
            moveWindow(new Vector2(50,50));
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            destroyWindow(currentWindow);
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            resizeWindow(-0.5f);
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            resizeWindow(0.5f);
        }

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            scrollWindow(1);
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            scrollWindow(-1);
        }

        if (Input.GetMouseButtonDown(0))
        {
            currentWindow = null;
        }
    }


    public GameObject openObject;
    public void createWindow()
    {
        menuMode = true;
        openObject.SetActive(true);
        StartCoroutine(initWindows());
    }

    public void updateWindowPosition()
    {
        if (currentWindow != null)
        {
           // currentWindow.transform.localPosition = mousePosition();
        }  
    }

    public void destroyWindow(GameObject window)
    {
        if (window == currentWindow)
        {
            currentWindow = null;
        }
            Destroy(window);
    }

    float previous = 0;

    public void resizeWindow(float current)
    {
        if (currentWindow == null)
            return;

        if (previous == 0)
            previous = current;

        float delta = previous - current;
      
        currentWindow.GetComponent<uWindowCapture.UwcWindowTexture>().scalePer1000Pixel -= delta;

        previous = current;
    }

    Vector3 previousHand = new Vector3(0, 0, 0);
    public void moveWindow(Vector3 current)
    {
        if (currentWindow == null)
            return;

        if(previousHand == new Vector3(0, 0, 0))
        {
            previousHand = current;
        }

        Vector3 delta = previousHand - current;

        currentWindow.transform.Translate(-delta.x * 50, -delta.y * 50, 0);
   

        previousHand = current;
    }

    public void scrollWindow(int direction)
    {

    }



}
