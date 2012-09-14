using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class ActionQuitGame : ActionBase
{
    public UIScreen mScreenToOpen;
    public bool mIsTopScreen;
    public int mSelected = 0;

    // Action
    protected override void DoActualAction()
    {
		Time.timeScale = 1;

		Application.LoadLevel(0);
		GameManager.Instance.DestroySingleton();
    }
#if UNITY_EDITOR
    // Editor
    public override bool OnMenuActionGUI(UIMenuItem item)
    {
		GUILayout.Label("Quit Game Action");
    	return (base.OnMenuActionGUI(item));
    }
#endif
}
