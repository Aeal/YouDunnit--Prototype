using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
public class ActionOpenScreen : ActionBase
{
    public UIScreen mScreenToOpen;
    public bool mIsTopScreen;
    public int mSelected = 0;

    // Action
    protected override void DoActualAction()
    {
        if (mScreenToOpen != null)
        {
            mScreenToOpen.ScreenMenu.SetActiveScreen(mScreenToOpen); // SetActiveScreen
        }
        else
        {
           //Debug.LogError("Error: Not setting a screen active because it is null.");
        }
    }

    // Editor
    #if UNITY_EDITOR
    public override bool OnMenuActionGUI(UIMenuItem item)
    {

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.BeginVertical();
        GUILayout.Label("Open Screen Action");

        int oldSelected = mSelected;

        mSelected = EditorGUILayout.Popup("Screen to Open", mSelected, item.ParentScreen.ScreenMenu.mScreenListNames.ToArray());
        if (oldSelected != mSelected || mScreenToOpen == null)
        {
            mScreenToOpen = item.ParentScreen.ScreenMenu.mScreenList[mSelected];
        }
        EditorGUILayout.EndVertical();
        EditorGUILayout.EndHorizontal();
        return (base.OnMenuActionGUI(item));
    }
    #endif
}

/* UIScreen screenToLoad;
 if (mSelectedScreenIndex >= 0)
 {
     screenToLoad = mParentScreen.mMenuRef.mScreenList[mSelectedScreenIndex];
     if (screenToLoad != null)
     {
         screenToLoad.mMenuRef.SetActiveScreen(screenToLoad, true);
     }
     else
     {
        //Debug.LogError("Could not get requested screen from list.");
     }
 }
 else
 {
    //Debug.LogError("Could not find specified screen in " + gameObject.name);
 }*/