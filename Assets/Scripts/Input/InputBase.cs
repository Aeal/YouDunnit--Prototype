using System;
using UnityEngine;
using System.Collections;
using Object = UnityEngine.Object;

public class  ClickedEventArgs : EventArgs
{
    public GameObject TargetObject;

    public ClickedEventArgs(GameObject targetObject)
    {
        TargetObject = targetObject;
    }
}


public class Inputbase
{
	public bool Disabled = false;
    private const int ClickableLayerMask = 1 << 14;
    private const int ItemLayerMask = 1 << 21;
    private const float raycastDistance = 4.0f;
    private static Inputbase instance;

    public virtual void Reset()
    {
	
    }
    public static Inputbase Instance
    {
        get 
        {
            if(instance == null)
            {
               //Debug.LogError("No input base object is made use CreateInput<T> to make a new instance");
                return null;
            }
                return instance;
        }
    } 

    public static void CreateInput<T>() where T : Inputbase, new()
    {
       //Debug.Log("Creating input instance");
        if(instance == null)
            instance = new T();
        else
        {
           //Debug.Log("There is already an input instance");
        }
    }

    public delegate void MoveUpPressedHandler     (object sender, EventArgs e);
    public delegate void MoveDownPressedHandler   (object sender, EventArgs e);
    public delegate void MoveLeftPressedHandler   (object sender, EventArgs e);
    public delegate void MoveRightPressedHandler  (object sender, EventArgs e);

    public delegate void MoveUpReleasedHandler    (object sender, EventArgs e);
    public delegate void MoveDownReleasedHandler  (object sender, EventArgs e);
    public delegate void MoveLeftReleasedHandler  (object sender, EventArgs e);
    public delegate void MoveRightReleasedHandler (object sender, EventArgs e);

    public delegate void InputResetHandler();

    public delegate void ActionButtonPressed      (object sender, ClickedEventArgs e);
    public delegate void ActionButtonReleased     (object sender, ClickedEventArgs e);


    public event MoveUpPressedHandler     OnMoveUpPressedHandle;
    public event MoveDownPressedHandler   OnMoveDownPressedHandle;
    public event MoveLeftPressedHandler   OnMoveLeftPressedHandle;
    public event MoveRightPressedHandler  OnMoveRightPressedHandle;

    public event MoveUpReleasedHandler    OnMoveUpReleasedHandle;
    public event MoveDownReleasedHandler  OnMoveDownReleasedHandle;
    public event MoveLeftReleasedHandler  OnMoveLeftReleasedHandle;
    public event MoveRightReleasedHandler OnMoveRightReleasedHandle;

    public event ActionButtonPressed      OnActionButtonPressedHandle;
    public event ActionButtonReleased     OnActionButtonReleasedHandle;

    public event InputResetHandler OnInputReset;
    public virtual void Update()
    {
        
    }


    protected virtual void RaiseOnInputReset()
    {
        if (OnInputReset != null)
            OnInputReset();
    }
    protected virtual void OnMoveUpPressed()
    {
        if (OnMoveUpPressedHandle != null)
            OnMoveUpPressedHandle(this,EventArgs.Empty);
    }

    protected virtual void OnMoveDownPressed()
    {
        if (OnMoveDownPressedHandle != null)
            OnMoveDownPressedHandle(this, EventArgs.Empty);
    }

    protected virtual void OnMoveLeftPressed()
    {
        if (OnMoveLeftPressedHandle != null)
            OnMoveLeftPressedHandle(this, EventArgs.Empty);
    }

    protected virtual void OnMoveRightPressed()
    {
        if (OnMoveRightPressedHandle != null)
            OnMoveRightPressedHandle(this, EventArgs.Empty);
    }

    protected virtual void OnMoveUpReleased()
    {
        if (OnMoveUpReleasedHandle != null)
            OnMoveUpReleasedHandle(this, EventArgs.Empty);
    }

    protected virtual void OnMoveDownReleased()
    {
        if (OnMoveDownReleasedHandle != null)
            OnMoveDownReleasedHandle(this, EventArgs.Empty);
    }

    protected virtual void OnMoveLeftReleased()
    {
        if (OnMoveLeftReleasedHandle != null)
            OnMoveLeftReleasedHandle(this, EventArgs.Empty);
    }

    protected virtual void OnMoveRightReleased()
    {
        if (OnMoveRightReleasedHandle != null)
            OnMoveRightReleasedHandle(this, EventArgs.Empty);
    }

    protected virtual void OnActionButtonPressed()
    {
        if(OnActionButtonPressedHandle != null)
            OnActionButtonPressedHandle(this, new ClickedEventArgs(GetClickedObject()));
    }

    protected virtual void OnActionButtonReleased()
    {
        if (OnActionButtonReleasedHandle != null)
            OnActionButtonReleasedHandle(this, new ClickedEventArgs(GetClickedObject()));
    }

    protected GameObject GetClickedObject()
    {

        RaycastHit hitInfo;
        if (Physics.Raycast(Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0)), out hitInfo, raycastDistance, ClickableLayerMask | ItemLayerMask))
        {
           //Debug.Log("Hit " + hitInfo.collider.gameObject.name);
            return hitInfo.collider.gameObject;
        }
        
        return null;
    }

    
}
