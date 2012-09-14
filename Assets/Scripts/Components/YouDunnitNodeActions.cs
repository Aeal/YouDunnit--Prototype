using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

[Serializable]
public class InitilizeEndState : NodeAction
{
     public override void DoAction()
    {
         GameManager.Accuse();
    }
}

[Serializable]
public class FadeToScene : SingleHash
{
    public string mSceneToGoTo;
    public override void DoAction()
    {
        var fader = nodeTarget.GetComponent<GUIFader>();
        fader.StartFadeOut(mSceneToGoTo);
        base.DoAction();
    }

#if UNITY_EDITOR
    public override void OnInspectorGUI()
    {
        mSceneToGoTo = EditorGUILayout.TextField("Scene to go to: ", mSceneToGoTo);
        base.OnInspectorGUI();
    }
#endif
}

[Serializable]
public class FadeToCamera : SingleHash
{
    public string cameraToSwitchToTag = " ", currentCameraTag = " ";
    private Camera currentCamera, nextCamera;
    public override void DoAction()
    {
        var fader = nodeTarget.GetComponent<GUIFader>();
        if (fader != null)
        {
            fader.StartFadeOut();
            fader.timer.TimerHandle += timer_TimerHandle;
        }
        base.DoAction();
    }

    void timer_TimerHandle()
    {
        currentCamera.active = false;
        nextCamera.active = true;
        

    }
    public override void Initilize()
    {
        currentCamera = GameObject.FindGameObjectWithTag(currentCameraTag).GetComponent<Camera>();
        nextCamera    = GameObject.FindGameObjectWithTag(cameraToSwitchToTag).GetComponent<Camera>();
        base.Initilize();
    }
#if UNITY_EDITOR
    public override void OnInspectorGUI()
    {
        cameraToSwitchToTag = EditorGUILayout.TextField("Camera to go to: ", cameraToSwitchToTag);
        currentCameraTag = EditorGUILayout.TextField("Camera currently at: ", currentCameraTag);
        base.OnInspectorGUI();
    }
#endif
}


[Serializable]
public class EndConvoActions : SingleHash
{
	
    public override void  DoAction()
    {
        Debug.Log("Ending convo actions");
        if(Inputbase.Instance != null)
            Inputbase.Instance.Reset();
        if (MainCharacter.Instance != null)
        {
            MainCharacter.Instance.mIsDetected = false;
            MainCharacter.Instance.IsTalking = false;
        }

        Conversation convo = nodeTarget.GetComponent<Conversation>();
        if(convo != null)
        {
            Debug.Log("Returning to scene");
           convo.ReturnToScene();
        }
        else
        {
            Debug.LogError("No conversation component found on object: " + nodeTarget.name);
        }
        
 	    base.DoAction();
    }
}

