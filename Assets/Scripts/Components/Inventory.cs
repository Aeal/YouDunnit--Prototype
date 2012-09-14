using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    /// <summary>
    /// Class Name: Inventory
    /// Author: TC_FALLOWS
    /// Last Edit: WFALLOWS 12/18/2011
    /// Purpose: Collection class for items
    /// </summary>

	#region Members

	private Item[] mInventory = new Item[MAX_ITEMS];
    public Item[] PlayerInventory
    {
        get { return mInventory; }
    }
	
    private Item mEquippedItem = null;						// Item currently equipped
    public Item EquippedItem
    {
        get { return mEquippedItem; }
    }
    private Item mUnEquippedItem = null;
    public Item UnEquippedItem
    {
        get { return mUnEquippedItem; }
    }

    private const string mEquipItem = "EquipItem"; //Now use a number to denote the slot.
    private static Inventory instance;
    public static  Inventory Instance
    {
        get { return instance; }
    }
    const int MAX_ITEMS = 2;

    #endregion
    
	#region Methods
    public void Start()
    {
        /*if (instance == null) instance = this;
        else
        {
            Destroy(this);
        }*/
    }

    public void Init()
    {
        // DurrHurr
        if (instance == null) instance = this;
        else
        {
            Destroy(this);
        }
    }
    void LogItems()
    {
        for (int index = 0; index < mInventory.Length; index++)
        {
            Item item = mInventory[index];
            Debug.Log("Item " + GetInventoryItemFromIndex(index) + " is at index " + index  + " in the inventory");
        }
    }

    //******************************************************************
    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            LogItems();
        }
        for (int i = 0; i < MAX_ITEMS; i++)
        {
            if (Input.GetButtonDown(mEquipItem + i))
            {
                if(mEquipItem != null)
                {
                    mUnEquippedItem = mUnEquippedItem;
                }
                    mEquippedItem = mInventory[i];
                    Debug.Log("Equiping item in slot " + i + "  " + mEquippedItem);
            }

        }
    }
	//*****************************************************************************************
	// If there's space in the inventory, add it to the current list and play audio associated. 
	// If not, return false
	//*****************************************************************************************
    public bool AddItemToInventory(Item addedItem)
    {
        bool added = false;
        Debug.Log("Adding Item: " + addedItem.mItemName);
        for (int i = 0; i < MAX_ITEMS; i++ )
        {
            if (mInventory[i] != null) continue;
            mInventory[i] = addedItem;
            //if (mEquippedItem == null)
            //{
            //    mEquippedItem = addedItem;
            //}
            //else if (mUnEquippedItem == null)
            //{
            //    mUnEquippedItem = addedItem;
            //}
			Debug.Log("Added item to index: " +i);
            added = true;
            if (addedItem.mPickupEffect != String.Empty)
                SoundManager.Play2DSound(addedItem.mPickupEffect);
            else
                Debug.Log("No pickup effect! (AddItemToInventory)");

            if (addedItem.mPlacementAudio != String.Empty)
                SoundManager.PlayMusic(addedItem.mPickupAudio);
            else
                Debug.Log("No placement audio! (AddItemToInvetory)");
            break;
        }
        return added;
    }
	//******************************************************************
	public void ClearInventory()
	{
		// If there are items in the inventory, clear them out.
		if(mInventory.Length > 0)
		{
			foreach(var item in mInventory)
			{
				item.DrawInventoryIcon = false;
			}
			Array.Clear(mInventory,0,mInventory.Length);
		}
	}
	//******************************************************************
    public Item GetInventoryItemFromIndex(int index)
    {
		Item ret = null;
        try
        {
            ret = mInventory.ElementAt(index);
        }
        catch (Exception)
        {
            Debug.Log("There was an error getting index: " + index + " in the inventory");
            throw;
        }
		
            
		
		
        return ret;
    }
	//*******************************************************************
	public bool InventoryContains(Item pItem)
	{
//        Debug.Log("Checking for item: " + pItem.mItemName);
	    for (int index = 0; index < mInventory.Length; index++)
	    {
	        var item = mInventory[index];
            if(item == null) continue;
	        if (item.mItemName == pItem.mItemName)
	        {
	            Debug.Log("Player Has Item");
	            return true;
	        }
	    }
	    return false;
	}
	//*******************************************************************
    public void RemoveItemFromInventory(Item pItem, bool taken = false )
    {
        Debug.Log("Removing " + pItem.mItemName );
        //pItem.DrawInventoryIcon = false;
        
        if (mEquippedItem == pItem)
        {
            mEquippedItem = null;
            // if we have an item in our off hand equipt that
            if (mUnEquippedItem != null)
            {
                mEquippedItem = mUnEquippedItem;
                mUnEquippedItem = null;
            }

        }
        if (mUnEquippedItem == pItem)
        {
            mUnEquippedItem = null;
        }
		for(int i =0; i < mInventory.Length; i++)
		{
			Item item = mInventory[i];
            if(item == null)continue;
			if(item.mItemName == pItem.mItemName)
			{
			    mInventory[i] = null;
				break;
			}
		}
        
		if(!taken)
		{
			SoundManager.Play2DSound(pItem.mPlacementEffect);
		}
		else
		
		if(pItem.mPlacementAudio != String.Empty)
		{ 
			SoundManager.PlayMusic(pItem.GetAudioDirectory() + pItem.mPlacementAudio);
		}
		else 
		{
			Debug.Log("Cannot play item placement audio!");
		}
		

    }
	//******************************************************************
    public void RemoveItemFromInventoryFromIndex(int index)
    {
//        this.GetComponent<Messaging>().AddMessage(mInventory[index].mItemName + " has been taken!");
        if(index >= 0 && index < mInventory.Length -1)
        {
            mInventory[index] = null;
        }
    }
	//******************************************************************
    private void OnDestroy()
    {
        Debug.Log("Removing inventory");
    }
	//******************************************************************
	
	#endregion
}
