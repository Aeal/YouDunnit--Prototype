using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class ActionCloseMenuItem: ActionBase
{
    public UIMenuItem mMenuItemToClose;
    public bool mIsTopScreen;
    public int mSelected = 0;
	public int mIndexToLoad;

    // Action
    protected override void DoActualAction()
    {
		mMenuItemToClose.StartTransitionOff();
    }

    // Editor
    #if UNITY_EDITOR

    public override bool OnMenuActionGUI(UIMenuItem item)
    {
		GUILayout.Label("Close Menu Item Action");
        mMenuItemToClose = (UIMenuItem)EditorGUILayout.ObjectField("Menu item to close", mMenuItemToClose.gameObject, typeof(UIMenuItem));
    	return (base.OnMenuActionGUI(item));
    }

    #endif
}
