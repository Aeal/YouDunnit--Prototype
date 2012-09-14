using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[RequireComponent(typeof(UserInterface))]
public class Messaging : MonoBehaviour
{
    
    #region Members
    int           mMaxMessages = 1024;        // Maximum number of messages storable
    int           mMaxOnScreen = 3;
    float         mTransitionTime = 0.75f;
    private int         mDepth = 5;
    private const int   mMaxMessageLength = 44;
    Vector2             mMessagingPos,
                        mNotificationPos;
    //UserInterface       mUIRef;
    public Rect         mMessagingRect,
                        mNotificationRect;
    GUIStyle            mNotificationStyle,
                        mLogStyle;
    List<string>        mMessages;
    public bool         mNotificationDisplay = false;
    public string       mNotificationString;
    float               mTimeToDisplay,
                        mStart;
    Color               mCurrentColor;
    private bool        mTransitionHalfComplete = false;
    #endregion

    #region Methods
    //******************************************************************
    void Init()
    {
        // All variables that should be changed reside here.
        
        //** Style for the "notification" system above the main GUI area.
        mNotificationStyle = new GUIStyle();
        mNotificationStyle.fontSize = 24;
        mNotificationStyle.normal.textColor = Color.white;
        mNotificationStyle.fontStyle = FontStyle.BoldAndItalic;
        mNotificationStyle.alignment = TextAnchor.UpperCenter;
        
        //** Style for the "log" system seen in the GUI area.
        mLogStyle = new GUIStyle();
        mLogStyle.fontSize = 18;
        mLogStyle.normal.textColor = Color.black;
        mLogStyle.fontStyle = FontStyle.Bold;
        mLogStyle.alignment = TextAnchor.UpperLeft;
        

        // GUI Item Positions
        mNotificationPos    = new Vector2(0, 20);
        mMessagingPos       = new Vector2(20, 67);
        mCurrentColor       = Color.white;

        //ResizeMessaging(mUIRef.mViewRect);
    }
    //******************************************************************
    void Start()
    {
        // Initialize containers and references
        mMessages = new List<string>();
        //mUIRef = this.gameObject.GetComponent<UserInterface>();
        //mMessagingRect = mUIRef.mViewRect;

        Init();
    }
    //******************************************************************
	public bool AddMessage(string message)
	{
        bool ret = true;

        if (mMessages.Count > mMaxMessages)
        {
            mMessages.RemoveAt(0);
            mMessages.Add(message);
            ret = true;
        }
        else if (message.Length < 1)
        {
            ret = false;
        }
        else if (message.Length > mMaxMessageLength)
        {
            ret = false;
            mMessages.Add("ERR: Message too long! (Messaging.cs)");
        }
        else
        {
            mMessages.Add(message);
        }

        return (ret);
    }
    //******************************************************************
    public bool ShowNotifiction(string notification, float timeToShow)
    {

        bool ret = false;
        if (notification.Length < 1)
        {

        }
        else if (timeToShow < 0.5)
        {

        }
        else
        {
            mCurrentColor.a = 1;
            mNotificationDisplay = true;
            mNotificationString = notification;
            mTransitionHalfComplete = false;
            mTimeToDisplay = timeToShow + 2 * (mTransitionTime);
            mStart = Time.time;

            ret = true;
        }

        return (ret);
    }
    //******************************************************************
    public void ResizeMessaging(Rect newRect)
    {
        // Update both GUI area rectanles to reflect the global UI scheme in UserInterface
        mNotificationRect = newRect;
        mNotificationRect.x += mNotificationPos.x;
        mNotificationRect.y += mNotificationPos.y;

        mMessagingRect = newRect;
        mMessagingRect.x += mMessagingPos.x;
        mMessagingRect.y += mMessagingPos.y;
    }
    //******************************************************************
	void Update()
	{
        if (mNotificationDisplay == true)
        {
            // Handles fading...
            if (mTransitionHalfComplete == false)
            {
                if (mCurrentColor.a < 1.0f)
                {
                    mCurrentColor.a += ( mTransitionTime / 2) * Time.deltaTime;
                }
                else
                {
                    mTransitionHalfComplete = true;
                }
            }
            else
            {
                if (mCurrentColor.a > 0.0f)
                    mCurrentColor.a -= (mTransitionTime / 2) * Time.deltaTime;
            }
            if ((mTimeToDisplay + mStart) < (Time.time ))
            {

                mNotificationDisplay = false;
            }

        }
	}

    //******************************************************************
    void OnGUI()
    {
        GUI.depth = mDepth;

        // Notification Display
        if (mNotificationDisplay == true)
        {
            GUI.color = mCurrentColor;
            GUI.Label(mNotificationRect, mNotificationString, mNotificationStyle);
        }

        // Messaging Display
        GUI.color = Color.white;
        GUI.BeginGroup(mMessagingRect);
       
        Rect temp = new Rect(0,0, mMessagingRect.width, 28);
        int i = 0;
        if ( mMessages.Count < mMaxOnScreen )
        {
            i = 0;
        }
        else
        {
            i = (mMessages.Count - mMaxOnScreen);
        }

        for (int k = 0; i < mMessages.Count; i++, k++)
        {
            temp.y = 0;
            temp.y += k * 20;
            GUI.Label(temp, mMessages[i], mLogStyle);
        }
        GUI.EndGroup();

    }
   
    //******************************************************************
    #endregion
}
