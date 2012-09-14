using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

public	class ConvertToPrefabWizard : ScriptableWizard
{
    private static ConvertToPrefabWizard instance;
    private static Vector2 scrollPos;
    private static GameObject target = Selection.activeObject as GameObject, prev;

    private new static string name = "",
                          dir = "/Resources/Prefabs",
                          ending = "_Prefab";
    [MenuItem("Ulib/Convert To Prefab Wizard")]
    private static void ConvertAssetToPrefabWizard()
    {
        if(instance == null)
            instance = DisplayWizard<ConvertToPrefabWizard>("Prefab Wizard");
       
            instance.Show();
            instance.Focus();
       
    }

    [MenuItem("Ulib/Convert Asset To Prefab %w")]
    private static void ConvertAssetToPrefab()
    {
        if(Selection.activeObject != null)
            CreatePrefab(Selection.activeGameObject, Selection.activeGameObject.name + ending, dir);
       
    }

    [MenuItem("Ulib/Convert Folder To Prefab")]
    private static void ConvertFolderToPrefabs()
    {
        string dir = EditorUtility.OpenFolderPanel("Select Folder", "Assets", "").Remove(0, Application.dataPath.Length - 6);
        if (dir.Length == 0)
        {
           //Debug.Log("No Folder Selected");
            return;
        }

        string[] files = Directory.GetFiles(dir);
        for (int index = 0; index < files.Length; index++)
        {
            if (!files[index].EndsWith(".meta")) continue;
            
            string assetToLoad = dir + "/" + Path.GetFileNameWithoutExtension(files[index]);
           //Debug.Log("Loading path: " + assetToLoad);
            GameObject loader = AssetDatabase.LoadAssetAtPath(assetToLoad, typeof (GameObject)) as GameObject;
            if (loader != null)
            {
               //Debug.Log("Inserting Asset");
                CreatePrefab(loader,loader.name+"_Prefab","Resources/Prefabs/TEST");
            }
            else
            {
               //Debug.Log(Path.GetFileNameWithoutExtension(files[index]) + " is not found in the database");
            }
        }

    }

    private void OnGUI()
    {
        
        target = EditorGUILayout.ObjectField(target, typeof (GameObject), true) as GameObject;
        name = EditorGUILayout.TextField("Name ", name);
        EditorGUILayout.BeginHorizontal();
        dir =  EditorGUILayout.TextField("Save To" , dir);
        EditorGUILayout.EndHorizontal();
        if(GUILayout.Button("Create Prefab"))
        {
            CreatePrefab(target,name,dir);
        }

    }

    public static bool checkAssetPath(string path, GameObject asset)
    {
        string fullpath = "Assets" + path + "/" + asset.name + ending + ".prefab";
       //Debug.Log("checking Path:" + fullpath);
       //Debug.Log(AssetDatabase.LoadAssetAtPath(fullpath, typeof(GameObject)));
        if (AssetDatabase.LoadAssetAtPath(fullpath, typeof(GameObject)) != null)
        {
            return EditorUtility.DisplayDialog("Are you sure?",
                    "The prefab already exists. Do you want to overwrite it?",
                    "Yes",
                    "No");
        }
        return true;
    }

    private static bool createAssetDestinationFolder(string path)
    {
        if (!DestinationExists("Assets"+path))
        {
            if (EditorUtility.DisplayDialog("Destination not found", "Create Destination Folder at: " + path + " ?", "yes", "no"))
            {
               //Debug.Log("Creating folder at: " + path);
                AssetDatabase.Refresh(ImportAssetOptions.ForceSynchronousImport);
                Directory.CreateDirectory(path);
                return true;
            }
            else return false;

        }
        return true;
    }

    private static void CreatePrefab(GameObject connectTo, string name, string path)
    {
        string absPath = Application.dataPath + path;
       //Debug.Log("Creating: " + connectTo.name + " at " + absPath);

        if (!createAssetDestinationFolder(absPath)) return;
        if(!checkAssetPath(path, connectTo)) return;

        Object newPrefab = EditorUtility.CreateEmptyPrefab("Assets" + path + "/" + name + ".prefab");
       //Debug.Log("Loading: Assets" + path + "/" + name + ".prefab" );
        
       
        EditorUtility.ReplacePrefab(Selection.activeGameObject, newPrefab, ReplacePrefabOptions.ConnectToPrefab);
        AssetDatabase.Refresh();

        

       
        
    }

    private static bool DestinationExists(string destination)
    {
       //Debug.Log("Checking: " + Application.dataPath + dir);
        return (Directory.Exists(Application.dataPath + dir));
    }
}

    


