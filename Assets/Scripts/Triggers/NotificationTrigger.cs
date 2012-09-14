using System;
using System.Collections;
using UnityEngine;

public class NotificationTrigger : MonoBehaviour
{
    public string MenuPrefabName = "Editor";
    public string NotificationPrefabName = "UI Notification";
    public string Message = "Message to display";

    GameObject m_notificationObject,
                m_menuSystemObject,
                m_textObject;
    TextMesh m_textMeshRef;
    Menu m_menuSystemRef;
    UIMenuItem m_menuItemRef;


    UIMenuItem notificationItem;
    bool    m_initalized = false;
    static bool mIsTriggered = false;

    // Call this to verify and attach assets needed for notication trigger
    void FindAttachAssets()
    {
        if ((m_notificationObject == null) ||
             (m_menuSystemObject == null))
        {
           //Debug.LogWarning("Notification Trigger object not set - looking it up manually.");

            if ((m_menuSystemObject = GameObject.Find(MenuPrefabName)) == null)
            {
                m_initalized = false;
               //Debug.LogError("Couldn't find a menu object named(" + MenuPrefabName + ")");
            }
            else if ((m_notificationObject = GameObject.Find(NotificationPrefabName)) == null)
            {
                m_initalized = false;
               //Debug.LogError("Couldn't find notication object(" + NotificationPrefabName + ")");
            }
            else if ((m_textObject = m_notificationObject.transform.FindChild("TextMesh").gameObject) == null)
            {
                m_initalized = false;
               //Debug.LogError("Couldn't find a text object on (" + NotificationPrefabName + ")");
            }
            else if ((m_menuSystemRef = m_menuSystemObject.GetComponent<Menu>()) == null)
            {
                m_initalized = false;
               //Debug.LogError("Couldn't find a menu system named(" + MenuPrefabName + ") on " + NotificationPrefabName);
            }
            else if ((m_textMeshRef = m_textObject.GetComponent<TextMesh>()) == null)
            {
                m_initalized = false;
               //Debug.LogError("Couldn't find a TextMesh on(" + NotificationPrefabName + ")");
            }
            else
            {
                // Got a valid menu system object and valid notification object
                UIScreen screen = null;

                // Find the GUI screen
                for (int i = 0; i < m_menuSystemRef.mScreenList.Count; i++)
                {

                    if (m_menuSystemRef.mScreenList[i].name == "GUI")
                    {
                        screen = m_menuSystemRef.mScreenList[i];
                        break;
                    }
                }

                if (screen == null)
                {
                    m_initalized = false;
                   //Debug.LogError("Can't find the GUI screen.");
                }
                else
                {
                    // Find the notification element
                    for (int i = 0; i < screen.mMenuItems.Count; i++)
                    {
                        if (screen.mMenuItems[i].name == NotificationPrefabName)
                        {
                            m_menuItemRef = screen.mMenuItems[i];
                            break;
                        }
                    }

                    if (m_menuItemRef == null)
                    {
                        m_initalized = false;
                       //Debug.LogError("Couldn't find custom notification object " + NotificationPrefabName);
                    }
                    else
                    {
                        m_initalized = true;
                    }
                }
            }
        }
    }

    //*************************************************************************
    void Start()
    {
        FindAttachAssets();
    }
    //*************************************************************************
    void OnTriggerEnter(Collider collider)
    {
        if (m_initalized == true)
        {
            if (collider.gameObject.tag == "MainCamera" && mIsTriggered == false)
            {
                m_menuItemRef.StartTransitionOn();
                m_textMeshRef.text = Message;
                mIsTriggered = true;
            }
        }
        else
        {
           //Debug.LogWarning("Notification triggered for " + Message + ", but had an error initialzing.");
        }

    }
    //*************************************************************************
    void OnTriggerStay(Collider collider)
    {
      /*  if (m_initalized == true)
        {
            if (collider.gameObject.tag == "MainCamera" && mIsTriggered == false)
            {
                   mIsTriggered = true;
            }
        }*/

    }

    void OnTriggerExit(Collider collider)
    {
        if (m_initalized == true)
        {
            
            if (collider.gameObject.tag == "MainCamera" && mIsTriggered == true)
            {
                m_menuItemRef.StartTransitionOff();
                
            }
            mIsTriggered = false;
        }
    }
    //*************************************************************************
}
