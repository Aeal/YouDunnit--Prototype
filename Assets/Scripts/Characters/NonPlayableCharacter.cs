using UnityEngine;
using System.Collections.Generic;
public enum CharacterItem
{
    SleuthingGuide,
    LuckyMagnifyingGlass,
    FeatherDuster,
    SkeletonKey,
    LoveLetters,
    LipStick,
    Perfume,
    CigarettePack,
    Medal,
    Bayonet,
    MotorcycleKeys,
    RocknRollRecord,
    AceOfSpades,
    Colt45,
    JewelNecklace,
    Photograph,
    GoldenLighter,
    Monocle,
    Stethoscope,
    PocketWatch,
    CigarBox,
    WineBottle,
    IvoryHorn,
    ShrunkenHead,
	MurderKnife,

}

public class NonPlayableCharacter : Character
{
    /// <summary>
    /// Class Name: MainCharacter
    /// Author: Will Fallows
    /// Last Edit: Will Fallows 12/17/2011
    /// Purpose: Adds properties for NPC's
    /// </summary>

    #region Members
    //Public
    public string               mWalkAnimation = "walk",
                                mIdleAnimation = "idle",
                                mTalkAnimation = "talk";
	MainCharacter characterRef;
    public CharacterItem        Item1, 
                                Item2;

    public TextAsset            mDefaultConvo,
                                mCaughtItem1Convo,
                                mCaughtItem2Convo;

    private List<DialogItem>    mDefault,
                                mCaught1, 
                                mCaught2;

    public float                mVisionRange, 
                                mFieldOfView = 68,
                                mSenseRange = 2;
    //Private
    private float               mDelayAtEnd = 5.0f,
                                mCurDelay = 0.0f;

	private Conversation        mConversationRef;
    private Navigation          mNavController;
    internal Motive             mCharacterMotive;
    private GameObject[]        mDoors;
	private Animation 			mAnimationRef;
	int temp = 0;
	public string 				DossierData;
    #endregion

    #region Methods
    //****************************************************************************
    //****************************************************************************
    void Start()
    {
        //mDoors = GameObject.FindGameObjectsWithTag("Door");
		var body = gameObject.AddComponent<Rigidbody>();
		body.isKinematic = true;
		body.useGravity = false;
		GameObject temp = GameObject.FindGameObjectWithTag("MainCamera");
		if(temp != null)
		{
			characterRef = temp.GetComponentInChildren<MainCharacter>();
		}
		base.Start();
        mCharacterMotive        = MotiveManager.GetMotive(mCharacterName);
        mConversationRef        = gameObject.GetComponent<Conversation>();
        //Debug.Log(gameObject.name + " Conversation status: " + mConversationRef);
        mNavController          = gameObject.GetComponent<Navigation>();

        GameManager.Instance.RegisterMotive(mCharacterName.ToString(),mCharacterMotive);
        GameManager.Instance.SpawnItem(Item1);
        GameManager.Instance.SpawnItem(Item2);
        mCaught1 = Conversation.LoadConvo(mCaughtItem1Convo);
        mCaught2 = Conversation.LoadConvo(mCaughtItem2Convo);
        mDefault = Conversation.LoadConvo(mDefaultConvo);
		DiarySpawner.SpawnDiary(mCharacterName.ToString());
        Inputbase.Instance.OnActionButtonPressedHandle += StartConverstaion;
		mAnimationRef = GetComponentInChildren<Animation>();
		
		if(gameObject.GetComponent<AccusePlayerInteraction>() != null)
		{
			mAnimationRef.Play(mIdleAnimation);
		}
		else
			mAnimationRef.Play(mWalkAnimation);		
		//Debug.Log("Animation ref status: " + mAnimationRef);
    }
	//***************************************************************************

    private void StartConverstaion(object sender, ClickedEventArgs e)
    {
        if (e.TargetObject != gameObject) return;

        //Debug.Log("Main Character can talk: " + !MainCharacter.Instance.mCanTalk); 
        if (!MainCharacter.Instance.mCanTalk) return;

        //Debug.Log("Main Character is detected: " + MainCharacter.Instance.mIsDetected); 
        if (MainCharacter.Instance.mIsDetected) return;

        //Debug.Log("Conversation status: " + Conversation.Talking); 
        if (Conversation.Talking)return;

        mConversationRef = e.TargetObject.GetComponent<Conversation>();
        if (mConversationRef == null)
        {
            //Debug.LogError("No conversation ref found");
            return;
        }
        //Debug.Log("Starting conversation with " + e.TargetObject.name);
        mConversationRef.TalkedTo(mDefault);
    }

    private void Update()
   {	
       //if (!animation.IsPlaying(mWalkAnimation) && !Conversation.Talking)
       //{
       //   //Debug.Log("Playing walk animation on: " + gameObject.name);
          //animation.Play(mWalkAnimation);
       //}

       if (mConversationRef != null)
       {
           if (Conversation.Talking || !MainCharacter.Instance.mCanTalk)
               return;
       }


       if ( !CheckSenseRange())// && !CheckFOVForPlayer())
		{
			return;
		}
		else if(CheckFOVForObject(MainCharacter.Instance.gameObject))
		{
			
			return;
		}
		else if(checkLOS())
		{
			
			return;
		}
		
       //Debug.Log("hit");
       //return;
        //Debug.Log("Checking Item: " + Item1);
        //if (Inventory.Instance == null || GameManager.Instance == null) return;
		
		if(GameManager.Instance.Items != null)	
		{	
			if(GameManager.Instance.Items.ContainsKey(Item1.ToString()))
			{
		        if (Inventory.Instance.InventoryContains(GameManager.Instance.Items[Item1.ToString()]))
		        {
					
		            //Debug.Log("Caught PLayer with item1 ");
		            Item itemToRemove = GameManager.Instance.Items[Item1.ToString()];
					characterRef.ModifySuspicion(itemToRemove.mSuspicionAmount);
		            //Debug.Log(itemToRemove);
		            Inventory.Instance.RemoveItemFromInventory(itemToRemove, true);
		            mConversationRef.TalkedTo(mCaught1);
		            GameManager.Instance.SpawnItem(itemToRemove.gameObject);
		            return;
		        }
			}
			if(GameManager.Instance.Items.ContainsKey(Item2.ToString()))
			{
		        if (Inventory.Instance.InventoryContains(GameManager.Instance.Items[Item2.ToString()]))
		        {
		            //Debug.Log("Caught Player with item2 ");
		            Item itemToRemove = GameManager.Instance.Items[Item2.ToString()];
					characterRef.ModifySuspicion(itemToRemove.mSuspicionAmount);
		            //Debug.Log(itemToRemove);
		            Inventory.Instance.RemoveItemFromInventory(itemToRemove, true);
		            mConversationRef.TalkedTo(mCaught2);
		            GameManager.Instance.SpawnItem(itemToRemove.gameObject);
		            return;
		
		        }
			}
		}
   }

    //****************************************************************************
	
	void OnDestroy()
	{
        Inputbase.Instance.OnActionButtonPressedHandle -= StartConverstaion;
	    
	}


    private bool CheckFOVForObject(GameObject objectToCheck)
    {
		//Debug.Log("in range");
		// get the vector representing the current npc and player
        Vector3 rayDirection = objectToCheck.gameObject.transform.position - transform.position;
		// normalize it for to get the direction
		rayDirection.Normalize();
		// find the vector for where the npc is facing.
		Vector3 npcView = transform.forward;
		// i probally dont need to normalize it, but just incase it isnt normalized
		npcView.Normalize();
		
		// find the angle between teh direction of the player
		// and the and where the character is facing
		float theta = Vector3.Dot(rayDirection,npcView);
		float rad = Mathf.Acos(theta);
		// convert to to degrees
		float degrees = rad * Mathf.Rad2Deg;
		// because the degrees is the total fov, we need to devide it by 2
		if( (degrees/2) < mFieldOfView )
		{
			
			return false;
		}
		//Debug.Log("hit");
		// at this point we have gotten rid of most cases
		// so it should be ok if this is a little
		// slower and more detailed, we want to check
		// and see if a wall is in the way
		/*temp++;
		Debug.Log(temp.ToString());
		RaycastHit hit;
		Vector3 dist = MainCharacter.Instance.gameObject.transform.position - transform.position;
		Vector3 dir = dist;
		dir.Normalize();
		if(Physics.Raycast(transform.position, dir,out hit,dist.magnitude))
		{
			if(hit.collider.tag == "NPC")
			{
				Debug.Log("something is in the way");
			}
		}*/
        return true;
    }

    private bool CheckSenseRange()
    {
        Vector3 rayDirection = MainCharacter.Instance.gameObject.transform.position - transform.position;
		float mag = rayDirection.magnitude;
		//if(mag < 3.0)
		//{
			if(rayDirection.magnitude >  mVisionRange)
			{
				return false;
			}
		//}
		return true;
    }
	
	private bool checkLOS()
	{
		//Debug.Log("in front");
		//Debug.Log(temp.ToString());
		RaycastHit[] hit;
		Vector3 dist = MainCharacter.Instance.gameObject.transform.position - transform.position;
		Vector3 dir = dist;
		dir.Normalize();
		hit = Physics.RaycastAll(transform.position, dir,dist.magnitude);

			//if(hit.collider.tag == "MainCamera")
			//{
				//Debug.Log("something is in the way");
		foreach(RaycastHit h in hit)
		{
			//Debug.Log(h.collider.name.ToString());
			
					
			if(h.collider.name.ToString() == "Wall")
			{
				return true;
			}
			/*if(h.collider.gameObject.layer.ToString() == "InnerWall")
			{
				Debug.Log("through WALL");
				return true;
			}*/
		}
		/*temp++;
		Debug.Log(temp.ToString());
		Debug.Log(hit.Length.ToString());*/
		return false;
	}
    #endregion
}
