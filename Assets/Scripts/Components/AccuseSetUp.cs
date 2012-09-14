using UnityEngine;
using System.Collections;

public class AccuseSetUp : MonoBehaviour
{
	

	// Use this for initialization
	void Start ()
    {
		NonPlayableCharacter npc;
        var spawner = GetComponent<NPCSpawner>();
		if(spawner != null)
        	npc = spawner.GetComponentInChildren<NonPlayableCharacter>();
		else
			npc = GetComponent<NonPlayableCharacter>();
		
		
	
		Destroy(npc.gameObject.GetComponent<Conversation>());
        npc.gameObject.GetComponent<NavMeshAgent>().enabled = false;
        npc.gameObject.AddComponent<ClickedTrigger>();
        //AddAccuse Select Action here
        npc.gameObject.AddComponent<AccusePlayerInteraction>();
			
		
       
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
