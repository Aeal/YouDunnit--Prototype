using UnityEngine;
using System.Collections;

public class StartConvo : MonoBehaviour
{
    public TextAsset ConvoToRun;
    public bool RunOnStart;
    private Conversation conversation;
	// Use this for initialization
	void Start ()
	{
	   if(RunOnStart)RunConvo();

	}
	
    public void RunConvo()
    {
         conversation = gameObject.GetComponent<Conversation>();
        if(conversation == null)
        {
            Debug.Log("No Conversation found");
            return;
            
        }
        if(ConvoToRun == null)
        {
            Debug.LogError("No Text to load");
            return;
        }
        conversation.TalkedTo(ConvoToRun);
    }
}
