using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(SetupUICamera))]
public class MenuSystemSetup : Editor
{
    SetupUICamera mSetupInstance;
    AspectRatioTarget mOldTarget, 
                      mCurrentTarget;
    public void Awake()
    {
        mSetupInstance = (SetupUICamera)target;
    }

    public override void OnInspectorGUI()
    {
        EditorGUILayout.BeginVertical(new GUIStyle("box"));
        EditorGUILayout.BeginHorizontal();

        mSetupInstance.mRatio = (AspectRatioTarget)EditorGUILayout.EnumPopup(mSetupInstance.mRatio);
        mCurrentTarget = mSetupInstance.mRatio;

        if (mOldTarget != mCurrentTarget)
        {
            mSetupInstance.ResetRatio();
            mOldTarget = mCurrentTarget;
        }

        
        
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.EndVertical();
    }
}
