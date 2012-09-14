using UnityEngine;
using System.Collections;
[RequireComponent(typeof(LoadingScreen))]
public class ParlorLoader : MonoBehaviour
{

	// Use this for initialization
	void Start () 
    {
        var load = GetComponent<LoadingScreen>();
        CharacterSetManager.GenerateSceneario();
        Debug.Log("Loading Parlor set: " + CharacterSetManager.SetID);
        load.StartLoad("Parlor" + CharacterSetManager.SetID);
		
		//WatchItemIndex.ItemsInPlay.Clear();
		//MansionResults.Characters.Clear();
		MansionResults.Clear();
		AccusationResults.Clear();
		
	}
	
	// Update is called once per frame
	void Update ()
    {
	
	}
}
