using UnityEngine;

public class MatchedArea : MonoBehaviour
{
    /// <summary>
    /// Class Name: MatchedArea
    /// Author: TC_FALLOWS
    /// Last Edit: Tyler Steele
    /// Purpose: Performs what needs to be done when an item is being checked to be matched in a placeable area
    /// </summary>

	#region Members
	// Public
	public const float                  mPickupDistance = 4.0f;
    public CharacterName                mOwner;
	private UINPCDisplay 			    mDisplay;
	// Private
	private float						mDistance = 0.0f;					// Distance from player
	
    private Item                        mCurrentItem;
	#endregion
	
	#region Methods
    void Start()
    {
		GameObject[] tempList = GameObject.FindGameObjectsWithTag("UIGUI");
		if(tempList == null)
		{
			Debug.Log("UI elemnts are not tagged UIGUI");
		}
		foreach( GameObject obj in tempList)
		{
			if(obj.name == "NPC Suspicion")
			{
				mDisplay = obj.GetComponentInChildren<UINPCDisplay>();
			}
			
		}
		if(mDisplay == null)
		{
			Debug.Log("UINPCDisplay not attached to any ui element");
		}
        Inputbase.Instance.OnActionButtonPressedHandle += OnClicked;
    }
	//******************************************************************
    void OnMouseOver()
    {
        renderer.material.color = Color.yellow;
    }
	//******************************************************************
    void OnMouseExit()
    {
        renderer.material.color = Color.white;
    }
	//******************************************************************
    void ReplaceMatchedAreaWithEquippedItem()
    {
        Debug.Log("Replacing items: " + mCurrentItem);
        Item itemToAdd = null; //The Item to add back into the inventory
		if(mCurrentItem !=null && Inventory.Instance.EquippedItem != null)return;
        if (mCurrentItem != null) //and a max inventory  + 1 check here
        {
			Debug.Log("Saving current item: " + mCurrentItem.name);
            itemToAdd = mCurrentItem;
            
			
			
        }

        //Item itemToMatch = Inventory.Instance.EquippedItem;
        //if (Inventory.Instance.EquippedItem == null)
        //{
        //    for (int i = 0; i < Inventory.Instance.PlayerInventory.Length; i++)
        //    {
        //        if (Inventory.Instance.PlayerInventory[i] == null) continue;
        //        itemToMatch = Inventory.Instance.PlayerInventory[i];
        //       break;
        //   }
        //}
		Item itemToMatch = Inventory.Instance.EquippedItem;
        if (itemToMatch == null)  
		{
            Debug.Log("item to match is null");
            renderer.enabled = true;
            collider.enabled = true;
			if(itemToAdd != null)
			{
				if(Inventory.Instance.AddItemToInventory(itemToAdd))
				{
					mCurrentItem.transform.parent = null;
					mCurrentItem.MoveItemToPlayer();
					mCurrentItem=null;
		
				}
			}
			return;
			
        }
       
        Debug.Log("Matching Item: " + itemToMatch);
        if (mCurrentItem != null)
        {
            mCurrentItem.transform.parent = null;
            mCurrentItem = null;
        }
        
        mCurrentItem = itemToMatch;
        mCurrentItem.transform.position = transform.position;
        mCurrentItem.gameObject.transform.parent = transform;
        Inventory.Instance.RemoveItemFromInventory(mCurrentItem);

        mCurrentItem.renderer.enabled = true;
        mCurrentItem.collider.enabled = false;
        renderer.enabled = false;
        
        if (itemToMatch.name != "Knife")
        {
            //Modify the owner of this area
            //GameManager.Instance.Characters[mOwner != CharacterName.Anyone ? mOwner.ToString() : mCurrentItem.mItemOwner.ToString()].ModifySuspicion(mCurrentItem.SuspicionAmount);
            //Modify the owner of the item
            GameManager.Instance.Characters[itemToMatch.mItemOwner.ToString()].ModifySuspicion(itemToMatch.SuspicionAmount);
            // signal to the ui to display the character.
            
            Debug.Log("display " + itemToMatch.mItemOwner.ToString());
            mDisplay.displayNPC(itemToMatch.mItemOwner);
            
            if (itemToAdd != null)
            {
                if (Inventory.Instance.AddItemToInventory(itemToAdd))
                {
                    itemToAdd.transform.parent = null;
                    itemToAdd.MoveItemToPlayer();
                    itemToAdd = null;

                }
            }

        }
        else
        {
            //if(mCurrentItem == GameManager.Instance.Items["MurderKnife"])

            Application.LoadLevel("LoadParlor");

        } 
        //Debug.Log("owner is " +mCurrentItem.mItemOwner.ToString());
        //Modify the owner of this area
        
        
      

    //Debug.Log("item is " + itemName );
        

//        if (itemToAdd != null)
//        {
//			Debug.Log("Adding item back");
//            Inventory.Instance.AddItemToInventory(itemToAdd);
//            itemToAdd.MoveItemToPlayer();
//            GameManager.Instance.Characters[itemToAdd.mItemOwner.ToString()].ModifySuspicion(-itemToAdd.SuspicionAmount);
//        }

    }




    //******************************************************************
    void Update()
    {	
		mDistance = Vector3.Distance(MainCharacter.Instance.transform.position, gameObject.transform.position);
        if (mCurrentItem == null) renderer.enabled = true;
    }

    void OnClicked(object sender, ClickedEventArgs e)
    {
        if (e.TargetObject != gameObject) return;
        //if (mDistance > mPickupDistance ) return;
        Debug.Log("Replacing stuff");
        ReplaceMatchedAreaWithEquippedItem();
      
    }


    void OnDestroy()
    {
        Inputbase.Instance.OnActionButtonPressedHandle -= OnClicked;
        
    }
    //******************************************************************
	#endregion
}
