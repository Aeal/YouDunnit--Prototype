using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


/// <summary>
/// [Deprecated]
/// This class has been deprecated and replaced by the node actions system
/// </summary>
class ConversationActions : MonoBehaviour
{
	GameObject 		mPlayerRef = null;
	MainCharacter   mMainCharacterRef = null;
	Conversation	mConversationRef = null;
	public bool 	mTurnTowardsPlayer = true;
	public bool 	mStopMoving = true;
	public bool 	mReverseModel = false;
	
	public bool 	mReverse = false;

    public delegate void OnConversationEndHandle(object sender, EventArgs e);
    public event OnConversationEndHandle OnConversationEnd;
	
	// StartConversationActions() is called by the conversation engine when a conversation begins.
	// It is called via BroadcastMessage(), so you can add as many actions as necessary. These are just examples
	public void StartConversationActions()
	{
		//Debug.Log("starting convo actions");
	    mPlayerRef = MainCharacter.Instance.gameObject;
        //PauseMenu pm = mPlayerRef.GetComponent<PauseMenu>();

        //pm.mConvoEnabled = true;
	    mMainCharacterRef = MainCharacter.Instance;
		
		if (mStopMoving)
        {
            iTween.Pause(this.gameObject);
        }

		if(mTurnTowardsPlayer)
		{
            
			Vector3 playerPos = mPlayerRef.transform.position;
			
			this.transform.LookAt(new Vector3(playerPos.x, this.transform.position.y, playerPos.z));
			
			if(mReverseModel)
			{
				this.transform.Rotate(0, 180, 0);
			}
		}
		
		/*if (mStopMoving == true)
		{
			iTween.Pause(this.gameObject);
		}*/
	}
	
	void StopConversationActions()
	{
		//Debug.Log("stopping convo actions");
	    mPlayerRef = MainCharacter.Instance.gameObject;
	    mMainCharacterRef = MainCharacter.Instance;
		
		if(mMainCharacterRef.IsDetected()) { mMainCharacterRef.SetDetected(false); }
		if(mStopMoving) { iTween.Resume(this.gameObject); }
	}

	void StopConversationRestoreDefaults()
	{
        StopConversationActions();
	}

    
	
	
	
	
	
	
	
	
	
	
	// OLD function, used to initialize every character at once except the butler. Will be replaced with function above to be placed on the start of each NonPlayableCharacter script
	
};