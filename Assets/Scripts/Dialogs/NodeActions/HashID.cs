using System;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using Random = System.Random;


[ExecuteInEditMode]
public class HashID : MonoBehaviour
{

    private static Dictionary<string,HashID> hashIDComponents = new Dictionary<string, HashID>();
    public string mHashID = "";

    public string HashId
    {
        get { return mHashID; }
    }
#if UNITY_EDITOR
    [MenuItem("Utility/Log Hash IDs")]
    private static void LogIDS()
    {
        foreach (KeyValuePair<string, HashID> hashIDComponent in hashIDComponents)
        {
            Debug.Log("Game Object: " + hashIDComponent.Value.name + " ID: " + hashIDComponent.Key);
        }
    }
    [MenuItem("Utility/HideShow Hash IDs")]
    private static void HideShow()
    {
        foreach (KeyValuePair<string, HashID> hashIDComponent in hashIDComponents)
        {
            if(hashIDComponent.Value.hideFlags == HideFlags.HideInInspector)
            {
                hashIDComponent.Value.hideFlags = 0;
            }
            else
            {
                hashIDComponent.Value.hideFlags = HideFlags.HideInInspector;
            }

        }
    }

    [MenuItem("Utility/RemoveHashComponents %g")] 
    public static void RemoveAllHashIDs()
    {
        foreach (HashID component in Selection.activeGameObject.GetComponents<HashID>())
        {
            DestroyImmediate(component,true);
        }
    }


    public static void DestroyHashIDComponent(string id)
    {
        Debug.Log("Removing hash ID: " + id);
        if (hashIDComponents.ContainsKey(id))
        {
            DestroyImmediate(hashIDComponents[id]);
            hashIDComponents.Remove(id);
        }
        

    }


#endif
   
    public static void Clear()
    {
        //hashIDComponents = new Dictionary<string, HashID>();
    }
    private static string GenerateHashID(string hashID = "")
    {
        string hid = hashID;
        //do
        //{
            Random random = new Random();
            hid += (char)('a' + random.Next(0, 26));
            hid += (char)('a' + random.Next(0, 26));
            hid +=
                (Math.Round((DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0)).TotalSeconds *
                            new Random().Next(0, 1000)).ToString());
        //} while ((hashIDComponents.ContainsKey(hashID) || hashID == ""));

        return hid;
    }

    private void Start()
    {
        
        
        AddToDictionary();
    }

    private void AddToDictionary()
    {
        if (mHashID == "")
        {
            Debug.Log(gameObject.name + " hash ID has not been set, please use initilize to se the ID");
            return;
        }
        if (hashIDComponents.ContainsKey(mHashID))
        {
            Debug.LogWarning("Duplicate Hash ID found. ID: " + mHashID);
        }
        else
        {
            hashIDComponents.Add(mHashID, this);
            //Debug.Log("Adding: " + mHashID + " To Table" );
        }
    }

    public string Initilize(string hashID = "")
    {
        mHashID = GenerateHashID(hashID);
        AddToDictionary();
        return mHashID;
    }

    //public static GameObject GetGameObjectFromHashID(string hashID)
    //{
    //    Debug.Log("Getting Component: " + hashID);
    //    if (hashIDComponents.ContainsKey(hashID))
    //    {
    //        return hashIDComponents[hashID].gameObject;
    //    }
       
    //    Debug.LogError("No gameobject found with " + hashID + " hashID");
    //    return null;
        
    //}

    public void OnGUI()
    {
        if (!hashIDComponents.ContainsKey(mHashID)) 
            AddToDictionary();
    }
	
	private void OnDestroy()
	{
		if(hashIDComponents.ContainsKey(mHashID))
			hashIDComponents.Remove(mHashID);
	}
    
}