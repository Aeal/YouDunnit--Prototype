using UnityEngine;

public class Item : MonoBehaviour
{

    /// <summary>
    /// Class Name: Item
    /// Author: TC_FALLOWS
    /// Last Edit: TSTEELE 1/27/2012
    /// Purpose: Holds item properties, including picking up and dropping them
    /// </summary>
    /// 

	#region Public Members
	//Public
	public const string					mAudioDirectory = "Player Calls/"; //The audio directory at which to play files
	
    public Texture2D                    mItemIcon;				   // The icon that displays when the item is picked up
	public CharacterName                mItemOwner;        		   // Who owns this item?
	public string						mPickupEffect;			   // Is there a sound associated with the item?
	public string						mPickupAudio;			   // Is there an audio clip when picked up?
	public string 						mPlacementEffect;		   // Should an effect play when placed?	
	public string						mPlacementAudio;		   // Should an audio track play when placed?
    public float                        mSuspicionAmount;          // How much suspicion does it hold?
    public CharacterItem                mItemName;			       // What's the name of the item?
    public SpawnPointPosition           mAreaSpawn;
	#endregion
	
	#region Private Members
    //Private
    private GameObject                  mPlayerRef;
    private MainCharacter               mMainCharacterRef;
    private Vector3                     mHoverPosition;            // Where is the hover cube located?
	private float                       mDistanceFromPlayer;       // How far is the item currently from the player?
	private bool                        mIsDrawingHover;           // Should I be drawing the hover cube?
    private bool                        mIsEquippable;             // Can it be equipped?
	private bool						mIsEquipped = false;	   // Is it currently equipped?
	private bool                        mDrawInventoryIcon;        // Should I be drawing the inventory icon?
	#endregion

    #region debug strings
    string dItemAdded = "";
    #endregion
	
    #region Accessors / Modifiers
	public string GetAudioDirectory() { return mAudioDirectory; }
	
    public bool IsEquippable
    {
        get { return mIsEquippable; }
        set { mIsEquippable = value; }
    }
	
	 public bool IsEquipped
    {
        get { return mIsEquipped; }
        set { mIsEquipped = value; }
    }

    public bool DrawInventoryIcon
    {
        get { return mDrawInventoryIcon; }
        set { mDrawInventoryIcon = value; }
    }

    public float SuspicionAmount
    {
        get { return mSuspicionAmount; }
        set { mSuspicionAmount = value; }
    }

    #endregion

    #region Methods
    // Use this for initialization
    //******************************************************************
	void Start ()
	{
        //Debug.Log("Starting Item: " + mItemName);
		//rigidbody.isKinematic = true;
	    mPlayerRef = MainCharacter.Instance.gameObject;
        mMainCharacterRef = MainCharacter.Instance;
        mIsEquippable = true;
		mIsDrawingHover = false;
		mDrawInventoryIcon = false;
	    Inputbase.Instance.OnActionButtonPressedHandle += OnActionPressed;
	    gameObject.tag = "InteractiveItem";
        //Debug.Log("Registering: " + mItemName);
        GameManager.Instance.RegisterItem(mItemName.ToString(), this);
		Physics.IgnoreCollision(collider,MainCharacter.Instance.collider);
	}

    void OnDestroy()
    {
        Inputbase.Instance.OnActionButtonPressedHandle -= OnActionPressed;        
    }

    //******************************************************************
	void OnMouseOver()
	{
        if (mIsEquippable)
        {
            //renderer.material.color = Color.red;
        }
	}
    //******************************************************************
    void OnMouseExit()
	{
        if (mIsEquippable)
        {
            renderer.material.color = Color.white;
        }
	}
    //******************************************************************
    void OnActionPressed(object sender, ClickedEventArgs e)
    {
       ////Debug.Log("ACTION PRESSED");
        if(e.TargetObject != gameObject) return;
        //Debug.Log(gameObject.name + "is clicked");
        if (!mIsEquippable) return;
        //Debug.Log("It is equipable");
        mDistanceFromPlayer = Vector3.Distance(mPlayerRef.transform.position, gameObject.transform.position);

        //TODO  Update this to work with Input Manager
        if ( mDistanceFromPlayer <= 4.0 && !Inventory.Instance.InventoryContains(this))
        {
            if (Inventory.Instance.AddItemToInventory(this))
            {
                MoveItemToPlayer();
            }
            else
            {
                SwapItems();
            }
        }
    }
    private void SwapItems()
    {
        //Debug.Log("Swaping Items");
       var item = Inventory.Instance.EquippedItem;
       if (item == null)
       {
           //Debug.Log("No equipped item defaulting to slot 0");
           item = Inventory.Instance.PlayerInventory[0];
       }
        if (item == null)
       {
           //Debug.Log("Nothing to swap with");
           return;
       }

        SwapAndActivate(item.gameObject);
        Inventory.Instance.RemoveItemFromInventory(item);
        if (Inventory.Instance.AddItemToInventory(this))
        {
            MoveItemToPlayer();
        }

    }

    public void SwapAndActivate(GameObject item)
    {
        item.transform.position = transform.position;
        item.transform.rotation = transform.rotation;
        item.transform.parent   = null;
        item.renderer.enabled   = true;
    }

    //******************************************************************
    public void RenderItem()
    {
        transform.collider.isTrigger = false;
        transform.renderer.enabled = true;
		transform.parent = null;
    }
    //******************************************************************
    public void MoveItemToPlayer()
    {
		
		//mDrawInventoryIcon = true;
        //transform.collider.isTrigger = true;
        transform.renderer.enabled = false;
        transform.parent = MainCharacter.Instance.transform;
        transform.localPosition = Vector3.zero;
		
		
    }
    //******************************************************************
    public void DropItem()
    {
        //Debug.Log("Dropping item: " + this.name);
		mDrawInventoryIcon = false;
        this.gameObject.AddComponent("Rigidbody");
        RenderItem();
    }
    #endregion
}