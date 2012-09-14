using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ObjectReplacer : EditorWindow
{
    
    
    private GameObject replace, instance;
    private List<GameObject> results = new List<GameObject>();
    private System.Type type = typeof (GameObject);
    private Vector2 sliderPos;
    [MenuItem("Edit/Find and Replace Objects")]
    static void Init()
    {
        ObjectReplacer window = GetWindow<ObjectReplacer>();
        
    }


    void OnGUI()
    {
        if (replace == null)
            replace = Selection.activeGameObject;
        replace  = (GameObject)EditorGUILayout.ObjectField("Replace: ", replace , type, true, null);
        instance = (GameObject)EditorGUILayout.ObjectField("With : ", instance, type, true, null);

        if(GUILayout.Button("Find"))
        {
            Find();
        }

        if(results != null && results.Count != 0)
        {
            EditorGUILayout.BeginVertical("box");
            sliderPos = GUILayout.BeginScrollView(sliderPos);
            foreach (GameObject gameObject in results)
            {
                EditorGUILayout.BeginHorizontal();
                GUILayout.Label(gameObject.name);
                if (GUILayout.Button("Focus"))
                {
                    SceneView.lastActiveSceneView.FrameSelected();
                }
                GUILayout.Button("Replace");
                {
                    
                }
                EditorGUILayout.EndHorizontal();
            }
            GUILayout.EndScrollView();
            EditorGUILayout.EndVertical();
        }
        
    }

    void Find()
    {
         GameObject[] GameObjects = FindSceneObjectsOfType(type) as GameObject[];

        foreach (var gameObject in GameObjects)
        {
            if (gameObject.name == replace.name && !results.Contains(gameObject))
            {
                results.Add(gameObject);
            }

        }
    }

    private void QueryReplace(GameObject gameObject)
    {
        
        string displayString = " Are you sure you want to replace" + gameObject.name + " with " + instance.name + "?";
        if (EditorUtility.DisplayDialog("Replace Object?", displayString, "Yes", "No"))
        {
           //Debug.Log("REplacing...");
        }
    }
   
   
}

