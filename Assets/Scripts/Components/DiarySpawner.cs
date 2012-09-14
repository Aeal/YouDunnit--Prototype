using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class DiarySpawner : MonoBehaviour
{
    private static List<DiarySpawner> spawners = new List<DiarySpawner>();
    
	// Use this for initialization
	void Start () 
    {
        //Debug.Log("Adding diary spawner");
	    spawners.Add(this);
	}
	
	// Update is called once per frame
	void Update ()
    {
	
	}

    public static bool SpawnDiary(string CharacterName)
    {
        //Debug.Log("SPAWNING DIARY FOR " + CharacterName);
        GameObject diary =
        Resources.Load("Prefabs/Diaries/Diary_" + CharacterName) as GameObject;

        if(diary == null)
        {
            Debug.Log("No Diary found for " + CharacterName);
            return false;
        }
		
		int spawnIndex = Utility.GetRandom(0, spawners.Count-1);
		DiarySpawner spawner = null;
		if(spawners.Count - 1 > spawnIndex)
        	spawner = spawners[spawnIndex];

        if(spawner == null)
        {
            //Debug.Log("No spawners found to spawn ");
            return false;
        }

        var g = Instantiate(diary, spawner.transform.position, spawner.transform.rotation);
        if(g == null)
        {
            //Debug.Log("Instantiation Failed!");
            return false;
        }
        spawners.Remove(spawner);
        Destroy(spawner);
        return true;
    }

    void OnDestroy()
    {
        Debug.Log("Diary Spawner Destroyed");
        
    }
}
