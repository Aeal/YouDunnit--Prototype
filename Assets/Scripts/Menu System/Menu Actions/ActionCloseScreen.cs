using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
public class ActionCloseScreen : ActionBase
{
    public UIScreen ScreenToClose;
    public int      Selected = 0;

    // Action
    protected override void DoActualAction()
    {
        if (GameManager.Instance != null)
        {
            if (!GameManager.Instance.GetComponent<Reticle>().enabled)
                GameManager.Instance.GetComponent<Reticle>().enabled = true;
        }

        if (ScreenToClose != null)
        {			
            ScreenToClose.ScreenMenu.SetInactiveScreen(ScreenToClose); // SetActiveScreen
        }
        else
        {
           //Debug.Log("Action Close Screen: Specified screen is null.");
        }

    }

    // Editor
    #if UNITY_EDITOR
    public override bool OnMenuActionGUI(UIMenuItem item)
    {

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.BeginVertical();
        GUILayout.Label("Close Screen Action");

        int oldSelected = Selected;

        Selected = EditorGUILayout.Popup("Screen to Close", Selected, item.ParentScreen.ScreenMenu.mScreenListNames.ToArray());
        if (oldSelected != Selected || ScreenToClose == null)
        {
            ScreenToClose = item.ParentScreen.ScreenMenu.mScreenList[Selected];
        }
        EditorGUILayout.EndVertical();
        EditorGUILayout.EndHorizontal();
        return (base.OnMenuActionGUI(item));
    }
    #endif
};