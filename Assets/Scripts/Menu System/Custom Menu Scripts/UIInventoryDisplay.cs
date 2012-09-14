using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

// ****************************************************************************
public class UIInventoryDisplay : UICustomScript
{
	// Members ****************************************************************
    GameObject      mLargeItemSlot,
                    mSmallItemSlot;
    Item            mSlotOne,
                    mSlotTwo;
	
	Vector3	mLargeItemSlotTransform,
			mLargeItemSlotScale;
	Vector3 mSmallItemSlotTransform,
			mSmallItemSlotScale;
	Shader 	mShader;
	
	public override void Start ()
	{
        base.Start();       // Make sure to call this first
	
		mShader = Shader.Find("Transparent/Diffuse"); 
		
		mLargeItemSlotTransform = new Vector3(1.782f, .1f, -0.151f);
		mLargeItemSlotScale = new Vector3(0.4f, 1f, 0.6f);
		mSmallItemSlotTransform = new Vector3(-3.35f, .1f, 2.20f);
		mSmallItemSlotScale = new Vector3(0.20f, 1f, 0.35f);

        if (Inventory.Instance == null)
        {
            BuildOk = false;
           //Debug.LogError("Inventory is null from UIInventoryDisplay()");
        }
        else
        {
        	BuildOk = SetupPlanes();
        }
		
		if (BuildOk == false )
		{
			Debug.LogError("UIInventoryDisplay not set up properly.");
		}
	}

    bool SetupPlanes()
    {
		
        bool ret = true;

		if (Owner != null)
		{
        	mLargeItemSlot = CreateTexturePlane(Owner.gameObject);
			mSmallItemSlot = CreateTexturePlane(Owner.gameObject);
			
			if ( (mLargeItemSlot == null) || (mSmallItemSlot == null) )
			{
				ret = false;
			}
			else
			{

				// Set up plane one
				mLargeItemSlot.transform.localPosition = mLargeItemSlotTransform;
				mLargeItemSlot.transform.localScale = mLargeItemSlotScale;
				mLargeItemSlot.renderer.material.SetColor("_Color", Color.white);
				
				// Set up plane two
				mSmallItemSlot.transform.localPosition = mSmallItemSlotTransform;
				mSmallItemSlot.transform.localScale = mSmallItemSlotScale;
				mSmallItemSlot.renderer.material.SetColor("_Color", Color.white);
				
				if (mShader == null)
				{
					Debug.LogWarning("Couldn't find default inventory shader (Transparent/Diffuse)!");
				}
				else
				{
					mLargeItemSlot.renderer.material.shader = mShader;
					mSmallItemSlot.renderer.material.shader = mShader;
				}
			}
		}
		else
		{
			ret = false;
		}

        return (ret);
    }
	
	private void SetMatTexture(GameObject plane, Texture2D tex)
	{
		if ( (plane == null) || tex == null )
		{
			Debug.LogWarning("You dunnit wrong");
		}
		else
		{
			plane.renderer.material.SetTexture("_MainTex", tex);
			plane.renderer.material.shader = mShader;
			
		}
	}
	public override void Update ()
	{
        if (Active)
        {

            //TITO i changed this to get from indexes instead
            Item small = Inventory.Instance.GetInventoryItemFromIndex(1),
                    big = Inventory.Instance.GetInventoryItemFromIndex(0);

            if (small != null)
            {
                mSmallItemSlot.renderer.enabled = true;
                SetMatTexture(mSmallItemSlot, small.mItemIcon);
            }
            else
            {
                mSmallItemSlot.renderer.enabled = false;
            }

            if (big != null)
            {
                mLargeItemSlot.renderer.enabled = true;
                SetMatTexture(mLargeItemSlot, big.mItemIcon);
            }
            else
            {
                mLargeItemSlot.renderer.enabled = false;
            }
            /*
            Item newSlotOne = Inventory.Instance.GetInventoryItemFromIndex(0),
                newSlotTwo = Inventory.Instance.GetInventoryItemFromIndex(1);
            //Inventory.Instance.GetInventoryItemFromIndex(1);
            //Item newSlotOne = Inventory.Instance.EquippedItem,
            //    newSlotTwo = Inventory.Instance.UnEquippedItem;
            
			if ( newSlotOne == null)
			{
				mLargeItemSlot.active = false;
                
			}
			else if ( mSlotOne != newSlotOne )
			{
				mLargeItemSlot.active = true;
				mSlotOne = newSlotOne;

				Texture2D itemOneTex;
				if ( mSlotOne.mItemIcon == null )
				{
					Debug.LogWarning(mSlotOne + " prefab item icon has not been set!");	
				}
				else
				{
					itemOneTex = mSlotOne.mItemIcon;
					SetMatTexture(mLargeItemSlot, itemOneTex);
					//mDisplayPlaneOne.renderer.material.mainTexture = itemOneTex;
				}
				
			}
			if ( newSlotTwo == null)
			{
				mSmallItemSlot.active = false;
			}
			else if ( mSlotTwo != newSlotTwo )
			{
				mSmallItemSlot.active = true;
				mSlotTwo = newSlotTwo;
				Texture2D itemTwoTex;
				if ( mSlotTwo.mItemIcon == null )
				{
					Debug.LogWarning(mSlotTwo + " prefab item icon has not been set!");	
				}
				else
				{
					itemTwoTex = mSlotTwo.mItemIcon;
					SetMatTexture(mSmallItemSlot, itemTwoTex);
					//mDisplayPlaneOne.renderer.material.mainTexture = itemTwoTex;
				}
				
			}*/
        }
	}
}
