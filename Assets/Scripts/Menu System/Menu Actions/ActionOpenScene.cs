using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class ActionOpenScene : ActionBase
{
    public UIScreen mScreenToOpen;
    public bool mIsTopScreen;
    public int mSelected = 0;
	//public int mIndexToLoad;
    public string mSceneToLoad = "";
    // Action
    protected override void DoActualAction()
    {
		Time.timeScale = 1;

            Application.LoadLevel(mSceneToLoad);
    }
#if UNITY_EDITOR
    // Editor
    public override bool OnMenuActionGUI(UIMenuItem item)
    {
        //mSceneToLoad = EditorGUILayout.TextField("Scene to load: ", mSceneToLoad);
    	return (base.OnMenuActionGUI(item));
    }
#endif
}

