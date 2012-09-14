// ****************************************************************************
// UIMenuItem - Menu item which belongs to a particular UIScreen
// Author: Tyler Ortiz
// Date: 1/25/12
// ****************************************************************************
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// ****************************************************************************
// Menu Item Properties
// ****************************************************************************
public enum UIMenuItemPositionState
{
    OffScreen,          // Position to be at if this item is to go off screen
    OnScreen,           // Position to be at if this item is on the screen
};
public enum UIMenuItemState
{
	Undefined = -1,
	Idle,
	Hover,
	Active,
};
// ****************************************************************************
// Menu Item Action Properites
// ****************************************************************************
public class UIActionRegisterArgs : EventArgs
{
    public UIMenuItemInteractionType    InteractionType;
    public KeyCode                      Key;

    public UIActionRegisterArgs(UIMenuItemInteractionType type = UIMenuItemInteractionType.MouseClick, KeyCode key = KeyCode.None)
    {
        Key = key;
        InteractionType = type;
    }
}
// ****************************************************************************
// Class Properties
// ****************************************************************************
[RequireComponent (typeof(MeshRenderer))]
[RequireComponent (typeof(MeshFilter))]
[Serializable]
//*****************************************************************************
public class UIMenuItem : MonoBehaviour
{
    #region Members
	// ************************************************************************
    // Item Properties
    // ************************************************************************
    [SerializeField]

    public ActionBase[] mItemActions;     // Register of listeners and actions
	public  UIScreen mParentScreen;
	public float 	mTime, 
					mTime2,
					mStopwatch;
    [SerializeField]
    private Vector3     mOnScreenPosition,  // Position this should be at on the screen
                        mOffScreenPosition; // Position this should be at off the screen
    [SerializeField]
    private Material    mItemTexture;

    // ************************************************************************
    // Component References
    // ************************************************************************
    private MeshRenderer    mMeshRenderer;
    private MeshFilter      mMeshFilter;

	
    // ************************************************************************
    // Event Handling
    // ************************************************************************
    public delegate void    DelegateMenuItemClick(UIActionRegisterArgs args);
    public delegate void    DelegateMenuKeypress(UIActionRegisterArgs args);
	public delegate void	DelegateCustomAction();
	public delegate void 	DelegateScreenOnActive();

    public event            DelegateMenuItemClick       OnMenuItemClickHandle;
    public event            DelegateMenuKeypress        OnMenuKeypressHandle;
	public event 			DelegateCustomAction		OnMenuCustomHandle;
	public event 			DelegateScreenOnActive		OnMenuItemActive;
    // ************************************************************************
    // Editor helper members
    // ************************************************************************
    protected bool            mItemIsBeingEdited = false;                 // Is this object being viewed / edited?
	public bool 			IsCustomMenuItem = false;
    public int              mSelectedScreenIndex = -1;          // Selected screen to load ( if any )
    public float            mStandardTweenTime = 2.0f;          // Amount of time tweens should be completed in.
    protected bool          mItemInTransition = false;       // Has the transition started?
    protected UIMenuItemPositionState mUIPositionState = UIMenuItemPositionState.OnScreen;
    
    public bool BeingEdited
    {
        get { return mItemIsBeingEdited; }
        set { mItemIsBeingEdited = value; }
    }
    public bool IsTransitionFinished
    {
        get { return !mItemInTransition; }
    }
	//public List<UIMenuAction> mItemActions;     // Register of listeners and actions
    #endregion
    //*************************************************************************
    // Item Transition Methods 
    //*************************************************************************
    public void StartTransitionOn()
    {
		//if ( mItemInTransition == true )
		{	
			iTween.Stop(gameObject);
        }
		iTween.MoveTo(this.gameObject, iTween.Hash("looptype", iTween.LoopType.none, "EaseType", iTween.EaseType.easeOutQuad, "islocal", true, "position", mOnScreenPosition, "time", mTime, "oncomplete", "CallbackTransitionFinished", "oncompletetarget", this.gameObject, "ignoretimescale", true));
		mItemInTransition = true;
    }
    //*************************************************************************
    public void StartTransitionOff()
    {
        //if (mItemInTransition == true)
        {
            iTween.Stop(gameObject);
        }
        iTween.MoveTo(this.gameObject, iTween.Hash("looptype", iTween.LoopType.none, "EaseType", iTween.EaseType.easeInQuad, "islocal", true, "position", mOffScreenPosition, "time", mTime2, "oncomplete", "CallbackTransitionFinished", "ignoretimescale", true, "onocompletetarget", this.gameObject));
        mItemInTransition = true;
    }
    //*************************************************************************
    public bool GetTransitionFinished()
    {
        return (!mItemInTransition);
    }
    //*************************************************************************
    // GameObject Methods
    //*************************************************************************
    void Awake()
    {
        if (mItemActions == null) mItemActions = gameObject.GetComponents<ActionBase>();
            //mItemActions = new List<UIMenuAction>();
        //iTween.Stop(gameObject);
    }
    //*************************************************************************
    void Start ()
    {
		ActionBase action;
		for (int i = 0; i < mItemActions.Length; i++)
		{
		
			action = (ActionBase)mItemActions[i];
			if (action == null)
			{
				//Debug.LogWarning("action" + i + " is null");
				continue;
			}
			
			// Any action involving the mouse add a box collider
			if (action.EventType == UIMenuItemInteractionType.MouseClick || action.EventType == UIMenuItemInteractionType.MouseKeyboard)
			{
				if ( gameObject.GetComponent<BoxCollider>() == null ) 
				gameObject.AddComponent<BoxCollider>();
			}
		}
		
		if ( IsCustomMenuItem == true )
		{
			this.gameObject.transform.localPosition = mOffScreenPosition;
		}
		else
		{
	        // Set starting position
	        if (ParentScreen.mActive == true)
	        {
	            this.gameObject.transform.localPosition = mOnScreenPosition;
	        }
	        else
	        {
	            this.gameObject.transform.localPosition = mOffScreenPosition;
	        }
		}

        // Hook up events
        foreach (ActionBase property in mItemActions)
        {
            if (property == null) continue;
            AddAction(property.EventType, new DelegateMenuItemClick(property.DoAction), new DelegateMenuKeypress(property.DoAction));
        }
	}

    void AddAction( UIMenuItemInteractionType type, 
                    DelegateMenuItemClick mouseClickAction = null, 
                    DelegateMenuKeypress keyPressAction = null )
    {
        if (keyPressAction == null && mouseClickAction == null)
        {
           //Debug.LogError("Passed incorrect arguments to AddAction. Action needs a listener!");
            return;
        }
        
        switch (type)
        {
		case UIMenuItemInteractionType.None:
			//OnMenuItemActive += StartTransitionOn;
			break;
            case UIMenuItemInteractionType.KeyboardPress:
            {
                if (keyPressAction != null)
                {
                    OnMenuKeypressHandle += keyPressAction;
                }
                else
                {
                   //Debug.LogWarning("A menu key press action specified is invalid.");
                }

                break;
            }
            case UIMenuItemInteractionType.MouseClick:
            {
                if (mouseClickAction != null)
                {
                    OnMenuItemClickHandle += mouseClickAction;
                }
                else
                {
                   //Debug.LogWarning("A menu item click action specified is invalid.");
                }
                break;
            }
            case UIMenuItemInteractionType.MouseKeyboard:
            {
                if (mouseClickAction != null && keyPressAction != null)
                {
                    OnMenuItemClickHandle += mouseClickAction;
                    OnMenuKeypressHandle += keyPressAction;
                }
                else
                {
                   //Debug.LogWarning("A menu item mouse + keyboard action specified is invalid.");
                }
                break;
            }
        }

    }
	public void BeginStopWatch()
	{
		mStopwatch = 3.0f;
	}
    //*************************************************************************
    void Update()
    {
		if ( IsCustomMenuItem == true )
		{
			/*if ( mStopwatch > 0.0f)
			{
				mStopwatch-= .1f;
			}
			else if ( mStopwatch != 0 )
			{
				mStopwatch = 0.0f;
				StartTransitionOff();
			}*/
		}
		/*if ( ParentScreen.ScreenMenu.mInTransition == true || mItemInTransition == true)
		{
			
		}
		else
		{
		 */
		if (ParentScreen.Active == true)
		{
			 if (mItemActions.Length > 0)
        	{
            if (Input.anyKeyDown)
            {
                KeyCode key;
                //TITO I COMMENTED THIS YOU -Tyler
                foreach (ActionBase action in mItemActions)
                {
                    if (action == null) continue;
                    key = action.ActionKey;

                    if (Input.GetKeyDown(key))
                    {
							//Debug.Log(gameObject.name + "YOU HIT " + key);
                        OnKeypress(new UIActionRegisterArgs(UIMenuItemInteractionType.KeyboardPress, key));
                    }

                }
            }
        	}
		}
		//}
       
    }
    // ************************************************************************
    void OnDestroy()
    {
        iTween.Stop(gameObject);
    }
    // ************************************************************************
    // Interfaces for UIScreen to fire this item's events
    // ************************************************************************
    public void OnClick()
    {
        // Called from UIScreen
        if (OnMenuItemClickHandle != null)
        {
            OnMenuItemClickHandle(new UIActionRegisterArgs());
        }
        else
        {
           //Debug.LogWarning("UI Menu Item " + this.gameObject.name + " was never assigned an action.");
        }

    }
    public void OnKeypress(UIActionRegisterArgs args)
    {
		
		if ( ParentScreen.Active)
		{
			//Debug.Log(gameObject.name + "PRESSING KEY AND IS ACTIVE");

            if (OnMenuKeypressHandle != null)
            {
                OnMenuKeypressHandle(args);
            }
            else
            {
               //Debug.LogWarning("UI Menu Item " + args.Key + " was never assigned an action.");
            }
		}
    }

    //*************************************************************************
    #region Internal Callback Methods
    //*************************************************************************
    public void CallbackTransitionFinished()
    {
        // This will be called by iTween
        mItemInTransition = false;
		//iTween.Stop();
    }
    //*************************************************************************
    #endregion
    //*************************************************************************
    #region Editor Helpers
    //*************************************************************************
    void UpdateMaterial()
    {
        if (mMeshRenderer != mItemTexture)
        {

            if (mMeshRenderer != null)
            {
                if (mItemTexture != null)
                {
                    mMeshRenderer.material = mItemTexture;
                }
            }
            else
            {
                mMeshRenderer = this.gameObject.GetComponent<MeshRenderer>();
            }
        }
    }
    //*************************************************************************
    public void UpdatePosition()
    {
        switch (mUIPositionState)
        {
            case UIMenuItemPositionState.OffScreen:
                this.gameObject.transform.localPosition = mOffScreenPosition;
                break;
            case UIMenuItemPositionState.OnScreen:
                this.gameObject.transform.localPosition = mOnScreenPosition;
                break;
        }
    }
    //*************************************************************************
    #endregion
    //*************************************************************************
    #region Accessors / Modifiers
    //*************************************************************************
    public Material ItemTexture
    {
        get { return mItemTexture; }
        set
        {
            mItemTexture = value;
            UpdateMaterial();
        }
    }
    //*************************************************************************
    public Vector3 ItemScreenPositionOn
    {
        get { return mOnScreenPosition; }
        set
        {
            mOnScreenPosition = value;
            UpdatePosition();
        }
    }
    //*************************************************************************
    public Vector3 ItemScreenPositionOff
    {
        get { return mOffScreenPosition; }
        set
        {
            mOffScreenPosition = value;
            UpdatePosition();
        }
    }
    //*************************************************************************
    public UIMenuItemPositionState ItemPositionState
    {
        get { return mUIPositionState; }
        set { mUIPositionState = value; }
    }
    //*************************************************************************
    public bool EditorView
    {
        get { return BeingEdited; }
        set { BeingEdited = value; }
    }

    public UIScreen ParentScreen
    {
        get 
        { 
            if (mParentScreen == null)
            { 
                mParentScreen = this.gameObject.transform.parent.parent.GetComponent<UIScreen>();
            }
            return mParentScreen; 
        }
        set { mParentScreen = value; }
    }
    //*************************************************************************
    #endregion
    //*************************************************************************
}
