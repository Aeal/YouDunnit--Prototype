using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

class InteractiveObject : MonoBehaviour
{
    /// <summary>
    /// Class Name: InteractiveObject
    /// Author: TC_FALLOWS
    /// Last Edit: WFALLOWS 12/18/2011
    /// Purpose: Gives the object the ability to be interacted with and follow along an iTween path
    /// </summary>
    ///
	
	#region Members
	//Public
	public  string						mActivatedSound = "";
    public float animationTime = 2.0f;
    private float runningTime = 0;
    public float openTime = 5.0f;
    private float tempTime = 0.0f;
	//Private
	private GameObject                  mPlayerRef;
	private float                       mDistanceFromPlayer;       // How far is the item currently from the player?
	//private ParticleRenderer			mRenderer;
	private const float                 mMaxDistance = 4.0f; // The max distance the player can be from the door to click on it and still activate
	private Vector3                     mStartingPostition;
	private bool						bIsOpen;
    private bool isMoving;
	public GameObject 	mObjectToMove;
	public Transform placetomoveto;
    
    private Vector3 startVec, endVec;
	#endregion
	
	#region Methods
	//******************************************************************
	void Start()
	{
        isMoving = false;
		
		mStartingPostition = this.gameObject.transform.position;
        startVec = placetomoveto.position;
        endVec = gameObject.transform.position; 

		//mfHashBrown.Add("position", placetomoveto.position);
		
		bIsOpen = false;
		
		Inputbase.Instance.OnActionButtonPressedHandle += OnClicked;
		//mRenderer = this.gameObject.GetComponentInChildren<ParticleRenderer>();
	}
	//*****************************************************************
	void OnClicked(object sender, ClickedEventArgs e)
	{
		if(mDistanceFromPlayer > mMaxDistance)
			return;
		if( e.TargetObject == null)
			return;
		if( e.TargetObject.tag != "SecretPassage")
		{
			Debug.Log(e.TargetObject.tag);
			return;
		}
		//Debug.Log(e.TargetObject.name);
		// play the iTween animation
		// If the object has a dedicated sound, play it. If not, play the default
		if(mActivatedSound != String.Empty)
			SoundManager.Play2DSound(mActivatedSound);
		else
            SoundManager.Play2DSound("hiddenPassage");

        if (!isMoving)
        {
            bIsOpen = !bIsOpen;
            isMoving = true;
            runningTime = 0;
            Vector3 temp;
            temp = startVec;
            startVec = endVec;
            endVec = temp;
        }
        
        //Debug.Log("I WAS CLICKEDEDMF");
	}
	//******************************************************************
    void Update()
    {
        if(mPlayerRef != null)
            mDistanceFromPlayer = Vector3.Distance(mPlayerRef.transform.position, this.gameObject.transform.position);
        if (isMoving)
        {
            runningTime += Time.deltaTime;
            if (runningTime >= animationTime)
            {
                isMoving = false;
                tempTime = 0;
                runningTime = animationTime;
            }

            gameObject.transform.position = Vector3.Lerp(startVec, endVec, runningTime / animationTime);
            //gameObject.transform.position = tempTransform.position;

        }
        else
        {
            if (bIsOpen) // if its not moving and its open then increment time 
            {
                tempTime += Time.deltaTime;
                if (tempTime >= openTime)
                {
                    bIsOpen = false;
                    isMoving = true;
                    runningTime = 0;
                    Vector3 temp;
                    temp = startVec;
                    startVec = endVec;
                    endVec = temp;
                    tempTime = 0.0f;
                }
            }
        }

    }
	#endregion
}
