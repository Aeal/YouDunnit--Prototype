using UnityEngine;
using System.Collections;

public class OpenUIScreen : MonoBehaviour {

    public UIScreen screenToOpen;
	// Use this for initialization
	void Start () 
    {
        var gObject = gameObject.GetComponent<ActionOpenScreen>();
        if (gObject == null)
        {
            gObject = gameObject.AddComponent<ActionOpenScreen>();
        }
        gObject.mScreenToOpen = screenToOpen;
        
        gObject.ExternalAction();
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
