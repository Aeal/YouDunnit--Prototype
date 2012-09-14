using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
public class ActionStopDiarySound : ActionBase
{


    // Action
    protected override void DoActualAction()
    {
		AudioSource curSource;
		GameObject[] diaries = GameObject.FindGameObjectsWithTag("Diary");
		for(int i = 0; i < diaries.Length; i++)
		{
			curSource = diaries[i].GetComponent<AudioSource>();
			if ( curSource != null)
			{
				curSource.Stop();
				Debug.Log("STOP AUDIO on" + curSource.gameObject);
			}
			
			
		}

    }

    // Editor
    #if UNITY_EDITOR
    public override bool OnMenuActionGUI(UIMenuItem item)
    {

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.BeginVertical();
        GUILayout.Label("Stop Diary Sound Action");
        EditorGUILayout.EndVertical();
        EditorGUILayout.EndHorizontal();
        return (base.OnMenuActionGUI(item));
    }
    #endif
};