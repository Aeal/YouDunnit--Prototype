using System.Collections.Generic;
using UnityEngine;
using System.Collections;

public class NPCSpawner : MonoBehaviour
{
    static List<NPCSpawner> spawners = new List<NPCSpawner>();
    public bool LookAtGameObect;
    public GameObject objectToLookAt;
    private const string characterPath = "Prefabs/Characters/";
    // Use this for initialization
    public static void Initilize()
    {
        
        foreach (CharacterName name in CharacterSetManager.CurrentCharacterSet)
        {
            //Pass a character to a Spawn()function
            //Debug.Log("Tying to spawn: " + name);
			if (name == CharacterName.Butler)continue;
            foreach (NPCSpawner npcSpawner in spawners)
            {
                if (npcSpawner.hasSpawned) continue;
                npcSpawner.Spawn(name);
                break;
            }
            
        }
	}

    private bool hasSpawned = false;
    float startX, startZ;
	public void Spawn(CharacterName characterToSpawn)
	{
        startX = transform.rotation.eulerAngles.x;
        startZ = transform.rotation.eulerAngles.z;
	    //spawn logic
	    hasSpawned = true;
        //Debug.Log("Spawning: " + characterToSpawn);
        var gameObject =(GameObject) Instantiate(Resources.Load(characterPath + characterToSpawn), transform.position, transform.rotation);
        if (LookAtGameObect)
        {
            gameObject.transform.parent = transform;
        }

        
	}

    public void Start()
    {
       spawners.Add(this);
    }

    public void Update()
    {
        if (LookAtGameObect)
        {

            transform.LookAt(objectToLookAt.transform);
           
        }
    }

    private void OnDestroy()
    {
        spawners.Remove(this);
    }
}
