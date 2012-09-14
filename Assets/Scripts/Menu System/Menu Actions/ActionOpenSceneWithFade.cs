using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class ActionOpenSceneWithFade : ActionBase
{
    public UIScreen mScreenToOpen;
    public bool mIsTopScreen;
    public int mSelected = 0;
    //public int mIndexToLoad;
    public string mSceneToLoad = "";
    public GUIFader fader;
    public float fadeTime = 2.0f;
    public bool useFader;

    protected override void DoActualAction()
    {
        Time.timeScale = 1;
        if (useFader && fader != null)
        {
            fader.FadeTime = fadeTime;
            fader.StartFadeOut(mSceneToLoad);
        }
        else
        {
            Application.LoadLevel(mSceneToLoad);
        }
    }
#if UNITY_EDITOR
    // Editor
    public override bool OnMenuActionGUI(UIMenuItem item)
    {
        GUILayout.Label("Open Scene with fade Action");
        //fader = (GUIFader)EditorGUILayout.ObjectField("Fader to fade: ", fader, typeof(GUIFader));
        return (base.OnMenuActionGUI(item));
    }
#endif
}
