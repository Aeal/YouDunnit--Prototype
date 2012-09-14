using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

public class UINPCDisplay : UICustomScript
{
	// Members ****************************************************************
	String 			mItemPath = "UserInterface/Items";
    //GameObject      mDisplayPlaneOne;
 	private GameTimer mTimer;
	bool startTimer;
	float delay = 3.0f;
	float currentTime;
	public Material[] matArray;
	//public Material CoveredFace;
	//public Material PlayerFace;
	private bool displayFace;

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
		displayFace = false;
		gameObject.renderer.enabled = false;
		BuildOk = SetupPlanes();
		// give it a random texture with alpha
		//mDisplayPlaneOne.renderer.material = matArray[0];
		//mDisplayPlaneOne.renderer.material.color = new Color(1,1,1,0);
		mTimer = new GameTimer();
		startTimer = false;
		currentTime = 0.0f;
	}
	
    void OnDestroy()
    {
       // mDisplayPlaneOne
    }
	
	

    bool SetupPlanes()
    {
        bool ret = true;
        // Get the UIOverlay texture and load it into a plane
		if (Owner != null)
		{
        	//mDisplayPlaneOne = CreateTexturePlane(Owner.gameObject);

			//Vector3 tempScale = mDisplayPlaneOne.transform.localScale;
			//tempScale.z *= 0.7f;
			//mDisplayPlaneOne.transform.localScale = tempScale;
		}
		else
		{
			ret = false;
		}


        return (ret);
    }
	// So you would think that there woudl be an easier way to do this,
	// aparently not but this works
	public void Update ()
	{
		if (Active)
		{
			if(startTimer)
			{
				currentTime = Time.time;
				startTimer = false;
				
			}
			if(displayFace)
			{

				if(currentTime + delay < Time.time)
				{
					//mDisplayPlaneOne.renderer.material.color = new Color(1,1,1,0);
					gameObject.renderer.enabled = false;
					displayFace = false;
				}
				
			}
			
		}
	
	}
	
	public void displayNPC(CharacterName name)
	{
		//Debug.Log( (int)name );
		if( matArray[(int)name] != null)
		{
			
			startTimer = true;
			gameObject.renderer.enabled = true;
			gameObject.renderer.material = matArray[(int)name];
			//mDisplayPlaneOne.renderer.material = matArray[(int)name];
			displayFace = true;
			
			//mDisplayPlaneOne.renderer.material.color = new Color(1,1,1,1);
		}
	}
}
