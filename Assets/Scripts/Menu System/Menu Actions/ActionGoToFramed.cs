using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class ActionGoToFramed : ActionBase
{
    public int frameCharacterIndex = 0;
    // Action
    protected override void DoActualAction()
    {
		Time.timeScale = 1;
        Application.LoadLevel("Framed"+CharacterSetManager.CurrentCharacterSet[frameCharacterIndex].ToString());
    }
#if UNITY_EDITOR
    // Editor
    public override bool OnMenuActionGUI(UIMenuItem item)
    {
        //mSceneToLoad = EditorGUILayout.TextField("Scene to load: ", mSceneToLoad);
        frameCharacterIndex = EditorGUILayout.IntField("Character index: ", frameCharacterIndex);
    	return (base.OnMenuActionGUI(item));
    }
#endif
}

