using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;


public class MainCharacter : Character
{
    /// <summary>
    /// Class Name: MainCharacter
    /// Author: TC_FALLOWS
    /// Last Edit: TORTIZ 10/23/2011
    /// Purpose: Adds player's properties such as adding/subtracting suspicion and input for equipping and dropping items
    /// </summary>

	#region Members

    private static  MainCharacter instance = null;
    public  static  MainCharacter Instance
    {
        get { return instance; }
    }
	public bool 			mIsIntroScene;								// Checks to see if you're in the intro scene

	
	// Private
    private bool             mIsItemEquipped; 							// Are we equipping an item?

	internal bool 			mIsDetected = false,						// Is the player being detected by an NPC
                            mCanTalk = true;
	private Conversation	mConversationRef = null;					// Conversation reference

    #endregion
	
	#region Accessors / Modifiers
	
	public bool 		IsItemEquipped() 	 		{ return mIsItemEquipped; }
	public bool			IsIntroScene() 		 		{ return mIsIntroScene; }
	public float 		GetPlayerSuspicion() 		{ return mSuspicionAmount; }
	public bool			IsDetected()		 		{ return mIsDetected; }
	public void			SetDetected(bool detection) { mIsDetected = detection; }
	
	#endregion
	
	#region Methods
	//******************************************************************
    private void Start()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
            return;
        }
		
	    mIsItemEquipped = false;		
		
        Conversation.OnConversationStarted += DisableClicking;
        Conversation.OnConversationEnded   += EnableClicking;
        GameManager.OnGamePaused += DisableClicking;
        GameManager.OnGameResume += EnableClicking;
        GameManager.OnGameEnding += DisableClicking;
        GameManager.OnGameCancelEnd += EnableClicking;
        mCharacterName = CharacterName.MainCharacter;
		
		Screen.showCursor = false;
        //Screen.lockCursor = false;

    }

    private void EnableClicking(object sender, EventArgs e)
    {
        mCanTalk = true;
    }

    private void DisableClicking(object sender, EventArgs e)
    {
        mCanTalk = false;
    }
    //******************************************************************
    //TODO Move this logic to the NPC
    
	//******************************************************************
    private void OnDestroy()
    {
        Conversation.OnConversationStarted -= DisableClicking;
        Conversation.OnConversationEnded -= EnableClicking;

        GameManager.OnGamePaused -= DisableClicking;
        GameManager.OnGameResume -= EnableClicking;
        GameManager.OnGameEnding -= DisableClicking;
        GameManager.OnGameCancelEnd -= EnableClicking;
    }
	#endregion

    public bool IsTalking { get; set; }
}