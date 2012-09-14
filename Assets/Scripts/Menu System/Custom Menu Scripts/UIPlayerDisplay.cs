using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

public class UIPlayerDisplay : UICustomScript
{
	// Members ****************************************************************
	String 			mItemPath = "UserInterface/Items";
    GameObject      mDisplayPlaneOne,
                    mDisplayPlaneTwo;
	
	public Material PlayerFace;
	public Material CoveredFace;
	MainCharacter characterRef;
	
	#region Accessors / Modifiers
	// Accessors / Modifiers **************************************************
	public String ItemPath
	{
		get { return mItemPath; }
		set { mItemPath = value; } 
	}
	
	
	#endregion

	public override void Start ()
	{
        base.Start();       // Make sure to call this first
       	GameObject temp = GameObject.FindGameObjectWithTag("MainCamera");
		if(temp != null)
		{
			characterRef = temp.GetComponentInChildren<MainCharacter>();
			BuildOk = SetupPlanes();
			mDisplayPlaneOne.renderer.material = PlayerFace;
			mDisplayPlaneTwo.renderer.material = CoveredFace;
			
		}
		

		
	}


    bool SetupPlanes()
    {
        bool ret = true;
       
		if (Owner != null)
		{
        	mDisplayPlaneOne = CreateTexturePlane(Owner.gameObject);
        	mDisplayPlaneTwo = CreateTexturePlane(Owner.gameObject);
			
			Vector3 tempScale = mDisplayPlaneOne.transform.localScale;
			tempScale.x *= 2.0f;
			mDisplayPlaneOne.transform.localScale = tempScale;
			
			Vector3 tempScale1 = mDisplayPlaneTwo.transform.localScale;
			tempScale1.x *= 2.0f;
			mDisplayPlaneTwo.transform.localScale = tempScale1;
			/*Quaternion blah = mDisplayPlaneOne.transform.rotation;
			blah.y = 180;
			mDisplayPlaneOne.transform.rotation = blah;
			
			Transform blah1 = mDisplayPlaneTwo.transform;
			blah1.RotateAround(Vector3.up,180);
			mDisplayPlaneTwo.transform.rotation = blah1.rotation;*/
			

			Vector3 temp = mDisplayPlaneOne.gameObject.transform.position;
			temp.z += 0.1f;
			mDisplayPlaneOne.transform.position = temp;
		}
		else
		{
			ret = false;
		}
        /*if (mDisplayPlaneOne == null || mDisplayPlaneTwo == null)
        {
            ret = false;
        }
        else
        {
            mDisplayPlaneOne.transform.localPosition = Vector3.zero;
            mDisplayPlaneTwo.transform.localPosition = Vector3.zero;
            mDisplayPlaneOne.transform.position = Owner.gameObject.transform.position;
            mDisplayPlaneTwo.transform.position = Owner.gameObject.transform.position;
            mDisplayPlaneOne.transform.parent = Owner.gameObject.transform;
            mDisplayPlaneTwo.transform.parent = Owner.gameObject.transform;
        }*/


        return (ret);
    }
	public void Update ()
	{
		if (Active)
		{
			float currentSuspicion = characterRef.GetPlayerSuspicion();
			// if currentSuspicion = 0, alpha = 1
			// if " = 100, alpha = 0;
			float alphaValue = (100 - (currentSuspicion))/100;
			//Debug.Log("alphaValue =" + alphaValue.ToString());
			mDisplayPlaneTwo.renderer.material.color = new Color(1,1,1,alphaValue);
		}
	
	}
}
