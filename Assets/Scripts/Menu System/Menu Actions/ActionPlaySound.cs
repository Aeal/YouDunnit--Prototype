using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
public class ActionPlaySound : ActionBase
{
    public AudioClip ClipToPlay;
    private AudioSource source;

    // Action
    protected override void DoActualAction()
    {
        if(source == null)
        {
            source = gameObject.AddComponent<AudioSource>();
        }

        if(!source.isPlaying)
        {
            source.clip = ClipToPlay;
            source.Play();
        }

    }

    // Editor
    #if UNITY_EDITOR
    public override bool OnMenuActionGUI(UIMenuItem item)
    {

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.BeginVertical();
        GUILayout.Label("Play Sound Action");
        ClipToPlay = (AudioClip) EditorGUILayout.ObjectField("Sound To Play: ", ClipToPlay, typeof (AudioClip), true);
        EditorGUILayout.EndVertical();
        EditorGUILayout.EndHorizontal();
        return (base.OnMenuActionGUI(item));
    }
    #endif
};