using System;
using UnityEngine;
using System.Collections;
using Object = UnityEngine.Object;

public class  MenuEventArgs : EventArgs
{
    public GameObject ObjectInCenterOfScreen;

    public MenuEventArgs(GameObject objectinCenter)
    {
        ObjectInCenterOfScreen = objectinCenter;
    }
}


public class BasicMenuInput 
{
    private static BasicMenuInput    mInstance;

    public static BasicMenuInput Instance
    {
        get 
        {
            if (mInstance != null)
            {
                return mInstance;
            }

           //Debug.LogError("No Menu Input handler found.");
            return null;
        }
    }
	
	public void Start()
	{
		Debug.Log ("Starting Input");
	}
    public delegate void DelegateMenuMousePress();
    public delegate void DelegateMenuMouseRelease();
    public delegate void DelegateMenuEscapePress();

    public event DelegateMenuMousePress     OnMenuClickPressHandle;
    public event DelegateMenuMouseRelease   OnMenuClickReleaseHandle;
    public event DelegateMenuEscapePress    OnMenuEscapePressHandle;

    public static void CreateInput()
    {
        mInstance = new BasicMenuInput();
    }

    public void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            OnMenuSystemClickPress();
        }

        if (Input.GetMouseButtonUp(0))
        {
            OnMenuSystemClickRelease();
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            OnMenuEscapeKeyPress();
        }


    }

    protected virtual void OnMenuSystemClickPress()
    {

        if (OnMenuClickPressHandle != null)
        {
            OnMenuClickPressHandle();
        }
        else
        {
           //Debug.LogWarning("Nothing handling menu click presses!");
        }
    }

    protected virtual void OnMenuSystemClickRelease()
    {
        if (OnMenuClickReleaseHandle != null)
        {
            OnMenuClickReleaseHandle();
        }
        else
        {
           //Debug.LogWarning("Nothing handling menu click releases!");
        }
    }

    protected virtual void OnMenuEscapeKeyPress()
    {
        if (OnMenuEscapePressHandle != null)
        {
            OnMenuEscapePressHandle();
        }
        else
        {
           //Debug.LogWarning("Nothing handling menu escape key press!");
        }
    }
}
