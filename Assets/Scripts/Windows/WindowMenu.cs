using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WindowMenu : MonoBehaviour
{
   // public Window window;
    public GameObject windowContainer;
    public GameObject windowList;
    public GameObject openObject;
    public static Window window;

    Vector3 pastMenuPosition = new Vector3(0, 0);
    float minDistanceY = 0.1f;
    int menuIndex = 0;
    float currentMenuY = 0;

    public static int windowIds;


    public void Start()
    {
        windowIds = 0;
        window = new Window();
    }


    public void closeMenu()
    {
        pastMenuPosition = new Vector3(0, 0);
        mode.currentMode = modes.NONE;
        currentMenuY = 0;
        openObject.SetActive(false);
    }

    public void down()
    {
        windowList.transform.GetChild(menuIndex).GetComponent<Image>().color = new Color32(0, 0, 0, 100);
        menuIndex = normalizeIndex(menuIndex + 1);
        windowList.transform.GetChild(menuIndex).GetComponent<Image>().color = new Color(1, 0, 0, 1);
        Debug.LogError(menuIndex);
    }

    public void up()
    {
        windowList.transform.GetChild(menuIndex).GetComponent<Image>().color = new Color32(0, 0, 0, 100);
        menuIndex = normalizeIndex(menuIndex - 1);
        windowList.transform.GetChild(menuIndex).GetComponent<Image>().color = new Color(1, 0, 0, 1);  
    }

    int normalizeIndex(int index)
    {
        int length = windowList.transform.childCount;
        if (index < 0)
        {
            index += length;
        }
        if(index >= length)
        {
            index -= length;
        }

        return index;
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

    public void toggle()
    {
        if(WindowManager.currentWindow != null)
        {
            return;
        }

        if (openObject.activeSelf)
        {
            closeMenu();
        }
        else
        {
            createMenu();
        }
    }

    public void selectMenuOption()
    {
        windowList.transform.GetChild(menuIndex).GetComponent<Button>().onClick.Invoke();
        WindowManager.safeState = WindowManager.safeWindowState.PARTIAL;
        

    }

    public void createMenu()
    {
        mode.currentMode = modes.MENU;
        openObject.SetActive(true);
        StartCoroutine(initWindows());
    }
    IEnumerator initWindows()
    {
        yield return new WaitForSeconds(1);
        foreach (var b in openObject.GetComponentsInChildren<Button>())
        {
            b.onClick.AddListener(delegate { StartCoroutine(takeAdd()); });
        }
    }
    IEnumerator takeAdd()
    {
        yield return new WaitForSeconds(1);
        if (windowContainer.transform.childCount == 0)
            yield break;

        WindowManager.currentWindow = windowContainer.transform.GetChild(windowContainer.transform.childCount - 1).gameObject;
        closeMenu();
    }


    public void toggleIds(bool show)
    {
        foreach (var text in windowList.GetComponentsInChildren<TMPro.TMP_Text>())
        {
            Debug.LogError("setting to: " + show);
            text.gameObject.SetActive(show);
        }
    }

    public bool selectId(int id)
    {
        foreach (var w in windowList.GetComponentsInChildren<Window>())
        {
            if (windowList.GetComponent<Window>().id == id)
            {
                window = w;
                return true;
            }
        }
        return false;
    }

    public void showSafeWindows()
    {
        if (WindowManager.safeState == WindowManager.safeWindowState.SHOWN)
        {
            return;
        }

        foreach (Window w in windowList.GetComponentsInChildren<Window>())
        {
            if (w.getSafeMode())
            {
                w.gameObject.SetActive(true);
            }
        }

        WindowManager.safeState = WindowManager.safeWindowState.SHOWN;
    }

    public void hideSafeWindows()
    {
        if (WindowManager.safeState == WindowManager.safeWindowState.HIDDEN)
        {
            return;
        }

        foreach (Window w in windowList.GetComponentsInChildren<Window>())
        {
            if (w.getSafeMode())
            {
                w.gameObject.SetActive(false);
            }
        }
        WindowManager.safeState = WindowManager.safeWindowState.HIDDEN;
    }


}

