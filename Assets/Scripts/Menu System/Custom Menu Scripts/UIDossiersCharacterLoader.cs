using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

public class UIDossiersCharacterLoader : UICustomScript
{
	// Members ****************************************************************
	String 			mPortraitPath = "UserInterface/DossiersMenu/CharacterPortraits";

	
	List<GameObject>	mPortraitPlanes;
	Dictionary<string, Texture2D> m_textureList;
	CharacterName[] m_characters;
	public Dictionary<string, Texture2D> GetCharacterPortraits
	{
		get { return m_textureList; }
	}
	#region Accessors / Modifiers
	// Accessors / Modifiers **************************************************
	public String PortraitPath
	{
		get { return mPortraitPath; }
		set { mPortraitPath = value; } 
	}
	
	
	#endregion

	public override void Start ()
	{
        base.Start();       // Make sure to call this first
		mPortraitPlanes = new List<GameObject>();

			//GameObject parentPlane;
		string name;

			//parentPlane = this.gameObject.transform.parent.gameObject;
			foreach ( Transform t in transform.parent )
			{
				name = t.name;
				if ( name.StartsWith("Slot") )
				{
					mPortraitPlanes.Add(t.gameObject);
				
					//Debug.Log(t.gameObject.name);
				
				}
			}
			Object[] temp;
			temp = Resources.LoadAll(mPortraitPath, typeof(Texture2D));
			m_textureList = new Dictionary<string, Texture2D>();
			foreach( Object b in temp)
			{
				m_textureList.Add(b.name, b as Texture2D);
			}
		
			if ( (m_characters = CharacterSetManager.CurrentCharacterSet) == null )
			{
				Debug.Log("Error getting characters");
				BuildOk = false;
			}
			else
			{
				for( int i = 0; i < m_characters.Length; i++)
				{
					string chars = m_characters[i].ToString() + "Dossier";
					//Debug.Log("DOSSEIERS LOADED" + chars);
					mPortraitPlanes[i].renderer.material.SetTexture("_MainTex", m_textureList[chars]);
				
				}
			}
			
			//mDisplayPlaneOne.renderer.material = PlayerFace;
			//mDisplayPlaneTwo.renderer.material = CoveredFace;
		
	}

  
	public void Update ()
	{
		if (Active)
		{
			//float currentSuspicion = characterRef.GetPlayerSuspicion();
			// if currentSuspicion = 0, alpha = 1
			// if " = 100, alpha = 0;
			//float alphaValue = (100 - (currentSuspicion))/100;
			//Debug.Log("alphaValue =" + alphaValue.ToString());
			//mDisplayPlaneTwo.renderer.material.color = new Color(1,1,1,alphaValue);
		}
	
	}
}
