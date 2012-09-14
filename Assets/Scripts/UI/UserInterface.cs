/*using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

//**********************************************************************
// UserInterface.cs
// Author: Tyler Ortiz
// Date: 11/06/2011
// Purpose: Draw all user interface elements on the screen.
//**********************************************************************
//**********************************************************************
public class UserInterface : MonoBehaviour //: ComponentSingleton<UserInterface>
{
    #region Members


    private static UserInterface instance;

    public static UserInterface Instance
    {
        get {return instance ?? (instance = new GameObject(typeof (UserInterface).ToString()).AddComponent<UserInterface>());}
    }

    public virtual void Init()
    {

    }
    // Editor variables
	public 	int 		_TimerMinutes = 5,
			            _TimerSeconds = 0;
    public GUIStyle     _UIStyle;


    float               mSecondAngle,
                        mMinuteAngle,
                        mScreenWidth,
                        mScreenHeight,
                        mCurTime;
    string              mTime;
    
    // Instance References
    Messaging           mMessagingRef = null;
	MainCharacter       mMainCharacterRef = null;
	

    // UI Element Positioning
    Vector2             mItem1Pos,
                        mItem2Pos,
                        mTimerPos,
						mPivotPos,
                        mCheckBox1Pos,
                        mCheckBox2Pos,
                        mCheckBox3Pos;

    // Texture Containers with Planes
    public Texture2D           mUIOverlay = null,
                               mItemOverlay = null,
                               mCheckBoxTexture = null,
                               mSecondHandTexture = null,
                               mMinuteHandTexture = null;
    GameObject          mUIOverlayPlane = null,
                        mUIItemOverlayPlane = null,
                        mCheckBoxPlane = null,
                        mMinuteHandPlane = null,
                        mSecondHandPlane = null;

    internal Rect       mViewRect = new Rect(Screen.width*.25f,Screen.height*.5f,Screen.width*.5f,Screen.height*.25f),					
                        mItem1Rect,
                        mItem2Rect,
                        mItem1RectSelect,
                        mItem2RectSelect,
                        mCheckBox1Rect,
                        mCheckBox2Rect,
                        mCheckBox3Rect,
                        mSecondHandRect,
                        mMinuteHandRect,
						mTimerRect;

    #endregion
	
	#region Accessors / Modifiers

    public Rect MainViewRect
    {
        // Rect of the user interface, starts at top left of interface, other positions are all based off this.
        get { return mViewRect; }
    }

	#endregion

    #region Methods
    //******************************************************************
	void Start()
	{
        mMessagingRef = MainCharacter.Instance.gameObject.GetComponent<Messaging>();
        mMainCharacterRef = MainCharacter.Instance;
        _UIStyle = new GUIStyle();
        if (mMainCharacterRef == null)
        {
           //Debug.LogError("Error: Initializing UserInterface, main character is NULL!");
        }
        else if (mMessagingRef == null)
        {
           //Debug.LogError("Error: Initializing UserInterface, messaging is NULL!");
        }
        else
        {
            // Get the UIOverlay texture and load it into a plane
            Object overlayObject = Resources.Load("UserInterface/Elements/Textures/UIOverlay", typeof(Texture2D));
            mUIOverlay = overlayObject as Texture2D;
            mUIOverlayPlane = GameObject.CreatePrimitive(PrimitiveType.Plane);
            mUIOverlayPlane.renderer.material.mainTexture = mUIOverlay;

            Object itemOverlayObject = Resources.Load("UserInterface/Elements/Textures/UIItemGlow", typeof(Texture2D));
            mItemOverlay = itemOverlayObject as Texture2D;
            mUIItemOverlayPlane = GameObject.CreatePrimitive(PrimitiveType.Plane);
            mUIItemOverlayPlane.renderer.material.mainTexture = mItemOverlay;

            Object checkBoxObject = Resources.Load("UserInterface/Elements/Textures/UIStrike", typeof(Texture2D));
            mCheckBoxTexture = checkBoxObject as Texture2D;
            mCheckBoxPlane = GameObject.CreatePrimitive(PrimitiveType.Plane);
            mCheckBoxPlane.renderer.material.mainTexture = mCheckBoxTexture;

            Object secondHandObject = Resources.Load("UserInterface/Elements/Textures/UISecondHand", typeof(Texture2D));
            mSecondHandTexture = secondHandObject as Texture2D;
            mSecondHandPlane = GameObject.CreatePrimitive(PrimitiveType.Plane);
            mSecondHandPlane.renderer.material.mainTexture = mSecondHandTexture;

            Object minuteHandObject = Resources.Load("UserInterface/Elements/Textures/UIMinuteHand", typeof(Texture2D));
            mMinuteHandTexture = minuteHandObject as Texture2D;
            mMinuteHandPlane = GameObject.CreatePrimitive(PrimitiveType.Plane);
            mMinuteHandPlane.renderer.material.mainTexture = mMinuteHandTexture;

            _UIStyle.normal.textColor = Color.black;
            _UIStyle.fontSize = 24;
            _UIStyle.fontStyle = FontStyle.Bold;
            _UIStyle.alignment = TextAnchor.MiddleCenter;
            mCurTime = (float)(new TimeSpan(0, _TimerMinutes, _TimerSeconds).TotalSeconds);

            InitPositions();
        }

	}
    //******************************************************************
    void InitPositions()
    {
        mTime = "";
        mItem1Pos = new Vector2(511, 67);
        mItem2Pos = new Vector2(591, 67);
        mTimerPos = new Vector2(730, 70);
        mCheckBox1Pos = new Vector2(474, 66);
        mCheckBox2Pos = new Vector2(474, 88);
        mCheckBox3Pos = new Vector2(474, 112);
        ResizeInterface();
    }
    //******************************************************************
    void ResizeInterface()
    {
        mScreenHeight = Screen.height;
        mScreenWidth = Screen.width;

        float diffX = (Screen.width - mUIOverlay.width) / 2.0f,
                diffY = (Screen.height - mUIOverlay.height);

        mViewRect = new Rect(diffX, diffY, mUIOverlay.width, mUIOverlay.height);
        mItem1Rect = new Rect(mViewRect.x + mItem1Pos.x, mViewRect.y + mItem1Pos.y, 64.0f, 64.0f);
        mItem2Rect = new Rect(mViewRect.x + mItem2Pos.x, mViewRect.y + mItem2Pos.y, 64.0f, 64.0f);
        mItem1RectSelect = new Rect(mViewRect.x + mItem1Pos.x - 5, mViewRect.y + mItem1Pos.y - 3, 64.0f, 64.0f);
        mItem2RectSelect = new Rect(mViewRect.x + mItem2Pos.x - 5, mViewRect.y + mItem2Pos.y - 3, 64.0f, 64.0f);
        mCheckBox1Rect = new Rect(mViewRect.x + mCheckBox1Pos.x - 1, mViewRect.y + mCheckBox1Pos.y - 1, 16.0f, 16.0f);
        mCheckBox2Rect = new Rect(mViewRect.x + mCheckBox2Pos.x - 1, mViewRect.y + mCheckBox2Pos.y - 1, 16.0f, 16.0f);
        mCheckBox3Rect = new Rect(mViewRect.x + mCheckBox3Pos.x - 1, mViewRect.y + mCheckBox3Pos.y - 1, 16.0f, 16.0f);
        mSecondHandRect = new Rect(0, 0, mSecondHandTexture.width, mSecondHandTexture.height);
        mMinuteHandRect = new Rect(0, 0, mMinuteHandTexture.width, mMinuteHandTexture.height);
        mTimerRect = new Rect(mViewRect.x + mTimerPos.x - 100, mViewRect.y + mTimerPos.y - 50, 200, 100);
        mPivotPos = new Vector2(mViewRect.x + mTimerPos.x, mViewRect.x + mTimerPos.y);

        mMessagingRef.ResizeMessaging(mViewRect);
    }
	//******************************************************************
	void Update()
	{
        int curMinutes = -1,
                curSeconds = -1;
        string secondString = "";

        if ((mCurTime - Time.deltaTime) > 0)
        {
            mCurTime -= Time.deltaTime;
        }
        else
        {
            //mMainCharacterRef.SetTimeUp(true);
        }

        curMinutes = Mathf.FloorToInt((mCurTime / 60.0f));
        curSeconds = Mathf.FloorToInt(mCurTime) - (curMinutes * 60);

        secondString = "";
        if (curSeconds < 10)
        {
            secondString += "0";
        }

        secondString += curSeconds.ToString();
        mTime = curMinutes.ToString() + ":" + secondString;

        if (mScreenWidth != Screen.width || mScreenHeight != Screen.height)
        {
            ResizeInterface();
        }
	}
	//******************************************************************
	void OnGUI()
	{
        GUI.depth = 10;
        GUI.DrawTexture(mViewRect, mUIOverlay);

        GUI.Label(mTimerRect, mTime, _UIStyle);

        // Draw inventory slot silhouettes and selection rectangle.
        for (int i = 0; i < Inventory.Instance.PlayerInventory.Count; i++)
        {
            if ( Inventory.Instance.PlayerInventory[i] == Inventory.Instance.EquippedItem )
            {
                if ( i == 0 )
                    GUI.DrawTexture(mItem1RectSelect, mItemOverlay);
                if ( i == 1)
                    GUI.DrawTexture(mItem2RectSelect, mItemOverlay);
            }
			
            if ( i == 0)
            {
                GUI.DrawTexture(mItem1Rect, Inventory.Instance.PlayerInventory[i].mItemIcon, ScaleMode.ScaleToFit);
            }
            if (i == 1)
            {

                GUI.DrawTexture(mItem2Rect, Inventory.Instance.PlayerInventory[i].mItemIcon, ScaleMode.ScaleToFit);
            }

        }

//        for (int i = 0; i < GameManager.Instance.CurrentNumStrikes; i++)
//        {
//            switch (i)
//            {
//                case 0:
//                    GUI.DrawTexture(mCheckBox1Rect, mCheckBoxTexture);
//                    break;
//                case 1:
//                    GUI.DrawTexture(mCheckBox2Rect, mCheckBoxTexture);
//                    break;
//                case 2:
//                    GUI.DrawTexture(mCheckBox3Rect, mCheckBoxTexture);
//                    break;
//            }
//        }

	}
    //******************************************************************
    #endregion
}
//**********************************************************************
*/