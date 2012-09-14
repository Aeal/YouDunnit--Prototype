using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class ActionBeginGame : ActionBase
{
    public UIScreen mScreenToOpen;
    public bool mIsTopScreen;
    public int mSelected = 0;
	public int mIndexToLoad;

    // Action
    protected override void DoActualAction()
    {
		CharacterSetManager.GenerateSceneario();
    }
#if UNITY_EDITOR
    // Editor
    public override bool OnMenuActionGUI(UIMenuItem item)
    {
		GUILayout.Label("Begin Game Action");
    	return (base.OnMenuActionGUI(item));
    }
#endif
}
