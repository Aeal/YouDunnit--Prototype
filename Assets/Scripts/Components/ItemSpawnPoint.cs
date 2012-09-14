using UnityEngine;
using System.Collections.Generic;


public enum SpawnPointPosition
{
    Basement,
    FirstFloor,
    SecondFloor,
}
public class ItemSpawnPoint : MonoBehaviour
{
    public SpawnPointPosition spawnArea;
    public static List<ItemSpawnPoint> PossibleSpawns = new List<ItemSpawnPoint>();
    private const string PREFAB_FOLDER = @"Prefabs/CharacterItems/";
    // Use this for initialization

    public static void Clear()
    {
        PossibleSpawns = new List<ItemSpawnPoint>();
    }
	void Start () 
    {
//        Debug.Log("CreatingSpawnpoint");
	    PossibleSpawns.Add(this);
	    renderer.enabled = false;
    }
	public void Spawn(GameObject g)
	{
	    //Debug.Log("Spawning Item: " + g.name);
	    g.transform.parent   = null;
		g.transform.position = transform.position;
		g.transform.rotation = transform.rotation;
	    g.renderer.enabled   = true;
	}
   public void Initilize(CharacterItem itemToSpawn)
    {
        //Debug.Log("Spawning: " + itemToSpawn.ToString());
        //Load from resources here;
        GameObject characterItemPrefab =
            (GameObject)
            Instantiate(Resources.Load(PREFAB_FOLDER + itemToSpawn), transform.position, transform.rotation);
        Item characterItem = characterItemPrefab.GetComponent<Item>();
        if(!characterItem)
        {
            Debug.LogError("Item Prefab missing Item component in " +itemToSpawn.ToString() );
            return;
        }
        PossibleSpawns.Remove(this);
        characterItem.mAreaSpawn = spawnArea;
        Destroy(gameObject);

    }
}
