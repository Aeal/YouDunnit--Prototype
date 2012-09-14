using System;
using UnityEngine;
using System.Collections;
using UnityEditor;

public class MotiveEditor : EditorWindow
{

    private static Motive mCurrentMotive;

    [MenuItem("Window/Motive Editor")]
    static void Init()
    {
        MotiveEditor window = GetWindow<MotiveEditor>();
        window.maxSize = new Vector2(250,300);
        mCurrentMotive = new Motive();
    }

    private void DrawFileControls()
    {
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("New"))
        {
            if (EditorUtility.DisplayDialog("Are you sure?", "OK", "Cancel"))
            {
                mCurrentMotive = new Motive();
            }
        }
        if (GUILayout.Button("Save"))
        {
            string path = EditorUtility.SaveFilePanel("Save Current Level", "", "NewLevel", "txt");
            if (path.Length != 0)
            {
                Motive.Serialize(path, mCurrentMotive);
            }
        }
        if (GUILayout.Button("Load"))
        {
            string path = EditorUtility.OpenFilePanel("Load New Level", "", "txt");

            if (path.Length != 0)
            {
                mCurrentMotive = Motive.Deserialize(path);
            }

        }
        EditorGUILayout.EndHorizontal();
    }


    public void OnGUI()
    {
        DrawFileControls();
        mCurrentMotive.Character = (CharacterName)EditorGUILayout.EnumPopup("Character", mCurrentMotive.Character, GUILayout.MaxWidth(250));

        mCurrentMotive.SuspicionModifier = EditorGUILayout.FloatField("Suspicion Modifier",
                                                                      mCurrentMotive.SuspicionModifier,
                                                                      GUILayout.MaxWidth(250));

        mCurrentMotive.DialogText = EditorGUILayout.TextArea(mCurrentMotive.DialogText, new[]{GUILayout.MaxWidth(250),GUILayout.MaxHeight(400)});
        
    }

}
