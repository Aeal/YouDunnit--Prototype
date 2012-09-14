using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using System.Text;

	
	

public enum Waits 
{
	Audio, 
    Animation,
    Subtitle, 
    Time
}
	
public enum Speakers
{
	Player,
    NPC
}
public class ConversationPackage
{
    public List<DialogItem> items;
    public int versionNumber = 0;
    public ConversationPackage()
    {
    }

    public ConversationPackage(List<DialogItem> dialogItems, int version)
    {
        items = dialogItems;
        versionNumber = version;
    }
}

[Serializable]	
public class DialogItem
{
	public bool 			show = true;
    public bool             skippable = false;
	public Speakers 		speaker = Speakers.NPC;
	public string			AudioClipRef;
	public string 			subtitle = "(enter subtitle text)";
	public string			AnimationToPlay;
    [XmlIgnore]
    public AudioClip        spoken = null;

    public Waits            wait = Waits.Audio;
	public bool 			is_branch = false;
	public List<String> 	choice;
	public List<int> 		jump;
    public float            waitTime = 0;
    public List<NodeAction> onNodeFocusedActions = new List<NodeAction>(),
                            onNodeLeaveActions   = new List<NodeAction>();
    public bool mHasPreloaded = false;

	public DialogItem()
	{

	}

    public void PreloadAll()
    {
        if (AudioClipRef != String.Empty)
        {
            spoken = (AudioClip)Resources.Load(AudioClipRef);
            if (spoken == null)
                Debug.Log("There was an error loading " + AudioClipRef);
            mHasPreloaded = true;
        }
        else
        {
            Debug.Log("NOT DETECTING AUDIO CLIP");
        }

       

    }
	
	public void Initialize()
	{
        //Debug.Log("Initilizing Dialog Item: " + subtitle);
        
        if (AudioClipRef != String.Empty)
        {
            spoken = (AudioClip)Resources.Load(AudioClipRef);
            if (spoken == null)
                Debug.Log("There was an error loading " + AudioClipRef);
        }      
       
        
	    DialogItem self = this;
        //Debug.Log("Initilizing On focused actions");
	    foreach (NodeAction onNodeFocusedAction in onNodeFocusedActions)
	    {
	        onNodeFocusedAction.Initilize(ref self, NodeActionType.OnDialogFocused);
	    }
        //Debug.Log("Initilizing On Leave actions");
        foreach (NodeAction onNodeLeaveAction in onNodeLeaveActions)
        {
            onNodeLeaveAction.Initilize(ref self, NodeActionType.OnDialogLeave);
        }
	}

    public void Update()
    {
        foreach (NodeAction onNodeFocusedAction in onNodeFocusedActions)
        {
            onNodeFocusedAction.Update();
        }
    }

#if UNITY_EDITOR
    public void BuildForSerialize()
    {
        foreach (NodeAction onNodeFocusedAction in onNodeFocusedActions)
        {
            onNodeFocusedAction.BuildForSerialize();
        }
        foreach (NodeAction onNodeLeaveAction in onNodeLeaveActions)
        {
            onNodeLeaveAction.BuildForSerialize();
        }
        Debug.Log("Checking for audio clip");
        if (spoken != null)
        {
            //Debug.Log("Audio clip found fetching resource path");
            AudioClipRef = Utility.GetResourcesPath(spoken);

            //Debug.Log("Fetch results: " + AudioClipRef);
        }
    }
#endif
}


[Serializable]	
public class Conversation : MonoBehaviour
{
	public delegate void OnConversationStartedHandle(object sender, EventArgs e);
	public static event OnConversationStartedHandle OnConversationStarted;
	
	public delegate void OnConversationEndedHandle(object sender, EventArgs e);
	public static event OnConversationEndedHandle OnConversationEnded;
	public Conversation(){}

	public List<DialogItem> dialog;
    
	// other variables
    public static bool Talking = false;
    public bool        Repeat  = false; // whether or not we repeat the conversation when the player comes a 2nd time
    [XmlIgnore]
	public GUISkin MySkin;
	public float subtitle_width = 0.66f;
	public float subtitle_offset = 2.590078f;
	public float subtitle_margin = 0.22f;
	
	
	private Vector3    CamOrigPos;
	private Quaternion CamOrigRot;



	private bool       firsttime = true,
	                   branching = false,
                       jumped = false,
                       isTalking = false;

    private AudioSource currentPlayingClip;
	private int         currentIndex = 0;
    private DialogItem  focusedNode;
    private Timer       WaitTimer = new Timer();
	//******************************************************************
    private void Start()
	{
        WaitTimer.TimerHandle += IncrementNode;
        MySkin = Resources.Load("UserInterface/Conversation/ConversationUI") as GUISkin;
	}
    //******************************************************************
    public void ClearCurrentDialog()
    {
        dialog = null;
    }
    //******************************************************************
    //TODO make this take a param for the convo
    public void TalkedTo(TextAsset conversationToRun)
    {

		if(OnConversationStarted != null)
		{
			OnConversationStarted(this, EventArgs.Empty);
		}
        dialog =  LoadConvo(conversationToRun);
        StartConversation();
    }
    public void TalkedTo(List<DialogItem> nodes)
    {
        Screen.showCursor = true;
        Screen.lockCursor = false;
        if (OnConversationStarted != null)
        {
            OnConversationStarted(this, EventArgs.Empty);
        }
        dialog = nodes;
        StartConversation();
    }
	//******************************************************************
    private void StartConversation()
	{
	    // run the conversation tree	
        Talking = true;
        isTalking = true;
	    currentIndex = 0;
        Talk(dialog[currentIndex]);
	}
	//******************************************************************
    private void Talk(DialogItem d)
    {
        jumped = false;
        //if (d.speaker == Speakers.Player)
        //{
        //    Talking = false;
        //    branching = true;
        //}
        //if (d.speaker == Speakers.NPC) 
        //{
        //    branching = false;
        //    Talking = true;
        //}
        //check thos
        if(currentPlayingClip != null)Destroy(currentPlayingClip);
        focusedNode = d;
        //Debug.Log("Talking");
        RunNodeAction(d, NodeActionType.OnDialogFocused);
        //float WaitTime = 0f;
       
        if(d.is_branch)return;
        switch (d.wait)
        {
            case Waits.Audio:
                currentPlayingClip = SoundManager.Play3DSoundWithCallback(gameObject, d.spoken, new SoundCallBack(IncrementNode)); ;
                break;
            case Waits.Time:
                WaitTimer.StartTimer(d.waitTime);
                break;

        }
       
    }
    //******************************************************************
    private void IncrementNode()
    {
        //Debug.Log("DEBUG TALKING: "+ Talking);
        //if(!Talking)return;
        //Debug.Log("Branching");
        //branching = true;

        //Run here to make sure we do not ovveride the branching but before we increment the index 
        //so we run the proper actions
        RunNodeAction(dialog[currentIndex], NodeActionType.OnDialogLeave);
        if (!jumped)
        {
            currentIndex++;
        }
        if(Talking)
            Talk(dialog[currentIndex]);
        
        
    }
	//******************************************************************
	public void ReturnToScene() 
    {
        //Debug.Log("Returning to Scene");
		Talking = false;
	    isTalking = false;
	    currentIndex = 0;
		Screen.showCursor = false;
        Screen.lockCursor = true;
        if (OnConversationEnded != null)
            OnConversationEnded(this, EventArgs.Empty);
    }
	/* ========== Camera Handling and GUI ========== */
    private int MIN_WIDTH = 60, MIN_HEIGHT = 20;
	//******************************************************************
    private void OnGUI()
	{
        GUI.skin = MySkin;
	    if (!isTalking) return;

	    

	    // estimate the height we will need
	    int fontsize = GUI.skin.label.fontSize;
			
	    if (fontsize==0)
	    {
	        // default font size - how do we get this? TODO
	        fontsize=24;
	    }
	    if (!dialog[currentIndex].is_branch)
	    {				
	        float chars_per_line = (float)Screen.width*0.8f / (float)fontsize;
	        float lines = 1.2f*Mathf.Ceil((float)(dialog[currentIndex].subtitle.Length)/(chars_per_line*2f)); // 1.2 lines-with-spacing times lines required, estimating that width of characters is 1/2 height at average

	        GUILayout.BeginArea(new Rect(Screen.width*(1f-subtitle_width)/2f, Screen.height*.75f, Screen.width*subtitle_width, (float)(fontsize*lines)+ subtitle_margin),GUIContent.none, ("box"));
	        GUILayout.Label(dialog[currentIndex].subtitle);
            if(dialog[currentIndex].skippable)
            {
                if(GUILayout.Button("Skip"))
                {
                    currentPlayingClip.Stop();
                }
            }
	        GUILayout.EndArea();
	    } 
	    if(dialog[currentIndex].is_branch)
	    {
	        float lines = 1.6f*(float)dialog[currentIndex].choice.Count; // need more space, for the buttons

            GUILayout.BeginArea(new Rect(Screen.width * (1f - subtitle_width) / 2f, Screen.height * .75f, Screen.width * subtitle_width, (float)(fontsize * lines) + subtitle_margin), GUIContent.none, ("box"));
                
	        for (int i=0; i< dialog[currentIndex].choice.Count; i++)
	        {
	            if (GUILayout.Button( dialog[currentIndex].choice[i], GUILayout.MinWidth(MIN_WIDTH), GUILayout.MinHeight(MIN_HEIGHT)))
	            {
	                
                    //Node Action end call
                    RunNodeAction(dialog[currentIndex], NodeActionType.OnDialogLeave);
                    Debug.Log("index at branch = " + currentIndex);
                    currentIndex = dialog[currentIndex].jump[i];
	                Talk(dialog[currentIndex]);

	            }
	        }
	        GUILayout.EndArea();				
	    }
	}

    public void SetDialogIndex(int index)
    {
        jumped = true;
        currentIndex = index;

    }
	//******************************************************************
    private void RunNodeAction(DialogItem Node, NodeActionType type)
    {
        //Debug.Log("Running node actions: " + type);
        switch (type)
        {
                case NodeActionType.OnDialogFocused:
                    foreach (NodeAction dialogItem in Node.onNodeFocusedActions)
                    {
                        dialogItem.DoAction();
                    }
                break;

                case NodeActionType.OnDialogLeave:
                    foreach (NodeAction dialogItem in Node.onNodeLeaveActions)
                    {
                        dialogItem.DoAction();
                    }
                break;
        }
    }
    //******************************************************************
    public static List<DialogItem> LoadConvo(TextAsset asset)
    {
        List<DialogItem> dialog;
        using (StringReader reader = new StringReader(asset.text))
        {
            var convoDialog = (ConversationPackage)new XmlSerializer(typeof(ConversationPackage),
                     Utility.GetClasses(typeof(NodeAction)).ToArray()).Deserialize(reader);
            dialog = convoDialog.items;
            reader.Close();
        }
        // Initialize to get the resources for the audio clips and animations...
        foreach (var item in dialog)
        {
            item.Initialize();
        }

        return dialog;
    }

    //private void PreloadAllConvo()
    //{
    //    NonPlayableCharacter npc = GetComponent<NonPlayableCharacter>();
    //    List<DialogItem> tempItems;

    //    //////////////////////////////////////////
    //    using (StringReader reader = new StringReader(npc.mCaughtItem1Convo.text))
    //    {
    //        var convoDialog = (ConversationPackage)new XmlSerializer(typeof(ConversationPackage),
    //                 Utility.GetClasses(typeof(NodeAction)).ToArray()).Deserialize(reader);
    //        tempItems = convoDialog.items;
    //        reader.Close();
    //    }
    //    // Initialize to get the resources for the audio clips and animations...
    //    foreach (var item in tempItems)
    //    {
    //        item.PreloadAll();
    //    }
    //    //////////////////////////////////
    //    using (StringReader reader = new StringReader(npc.mCaughtItem2Convol.text))
    //    {
    //        var convoDialog = (ConversationPackage)new XmlSerializer(typeof(ConversationPackage),
    //                 Utility.GetClasses(typeof(NodeAction)).ToArray()).Deserialize(reader);
    //        tempItems = convoDialog.items;
    //        reader.Close();
    //    }
    //    // Initialize to get the resources for the audio clips and animations...
    //    foreach (var item in tempItems)
    //    {
    //        item.PreloadAll();
    //    }

    //    //////////////////////////////////
    //    using (StringReader reader = new StringReader(npc.mDefaultConvo.text))
    //    {
    //        var convoDialog = (ConversationPackage)new XmlSerializer(typeof(ConversationPackage),
    //                 Utility.GetClasses(typeof(NodeAction)).ToArray()).Deserialize(reader);
    //        tempItems = convoDialog.items;
    //        reader.Close();
    //    }
    //    // Initialize to get the resources for the audio clips and animations...
    //    foreach (var item in tempItems)
    //    {
    //        item.PreloadAll();
    //    }

    //}

    private void Update()
    {
        if(focusedNode != null)
            focusedNode.Update();
    }

    private void OnDestroy()
    {
        WaitTimer.TimerHandle -= IncrementNode;
    }
}
