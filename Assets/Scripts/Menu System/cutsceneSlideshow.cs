using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Linq;

public class cutsceneSlideshow : MonoBehaviour
{
	#region Members
	//Public
	public string cutsceneXMLSource;
	public int levelToLoad;
	
	//Private
	private const string ImageDirectory = "Images/";
	private const string XMLDirectory = "Xml/";
	private int mCurrentSlide;
	private bool mNextEnabled;
	private bool mPrevEnabled;
	private bool mContinueEnabled;
	
	Dictionary<Texture2D, string> cutsceneDictionary = new Dictionary<Texture2D, string>();
	#endregion
	
	// Use this for initialization
	void Start ()
	{
		SoundManager.Instance.Init(true,true,2,6, "Audio");
		mNextEnabled = true;
		mPrevEnabled = false;
		mContinueEnabled = false;
		
		if(!Screen.showCursor)
				Screen.showCursor = true;
		
		if(Screen.lockCursor) 
			Screen.lockCursor = false;
		
		if(cutsceneXMLSource != null)
		{
			string element = "";
			mCurrentSlide = 0;
			List<string> imageStrings = new List<string>();
			List<string> audioStrings = new List<string>();
			
			TextAsset textAsset = Resources.Load(XMLDirectory + cutsceneXMLSource) as TextAsset;
			XmlTextReader reader = new XmlTextReader(new StringReader(textAsset.text));
			
			while(reader.Read())
			{
				if(reader.NodeType == XmlNodeType.Element)
				{
					element = reader.Name;
				}
				else if(reader.NodeType == XmlNodeType.Text)
				{
					switch(element)
					{
					case "image":
						imageStrings.Add(reader.Value);	
						break;
						
					case "audio":
						audioStrings.Add(reader.Value);
						break;
					}
				}
			}
			
			for(int i = 0; i < imageStrings.Count; i++)
			{
				//Debug.Log(imageStrings.Count);
				//Debug.Log(ImageDirectory + cutsceneXMLSource + "/" + imageStrings[i]);
				cutsceneDictionary.Add(Resources.Load(ImageDirectory + cutsceneXMLSource + "/" + imageStrings[i]) as Texture2D, audioStrings[i]);
			}
			
			this.renderer.material.mainTexture = cutsceneDictionary.ElementAt(mCurrentSlide).Key;
			SoundManager.PlayMusic(cutsceneDictionary.ElementAt(mCurrentSlide).Value);
		}
	}
	
	private void Next()
	{
		mCurrentSlide++;
		this.renderer.material.mainTexture = cutsceneDictionary.ElementAt(mCurrentSlide).Key;
        SoundManager.PlayMusic(cutsceneDictionary.ElementAt(mCurrentSlide).Value);
	}
	
	private void Prev()
	{
		mCurrentSlide--;
		this.renderer.material.mainTexture = cutsceneDictionary.ElementAt(mCurrentSlide).Key;
        SoundManager.PlayMusic(cutsceneDictionary.ElementAt(mCurrentSlide).Value);
	}

	// Update is called once per frame
	void Update ()
	{
		if(cutsceneDictionary.ElementAt(mCurrentSlide).Key == cutsceneDictionary.Keys.Last())
		{
			mNextEnabled = false;
			mPrevEnabled = true;
			mContinueEnabled = true;
		}
		else if(cutsceneDictionary.ElementAt(mCurrentSlide).Key == cutsceneDictionary.Keys.First())
		{
			mNextEnabled = true;
			mPrevEnabled = false;
		}
		else
		{
			mNextEnabled = true;
			mPrevEnabled = true;
			mContinueEnabled = false;
		}
	}
	
	void OnGUI() 
	{
	
		if(mNextEnabled)
		{
			if (GUI.Button (new Rect (20,40,80,20), "Next")) 
			{
				Next();
			}
		}
	
		if(mPrevEnabled)
		{
			if (GUI.Button (new Rect (20,70,80,20), "Previous") && mPrevEnabled) 
			{
				Prev();
			}
		}
		
		if(mContinueEnabled)
		{
			if (GUI.Button (new Rect (20,100,80,20), "Continue!") && mContinueEnabled) 
			{
				//Camera.mainCamera.enabled = false;
				//AudioListener listener = Camera.mainCamera.GetComponent<AudioListener>();
				//listener.enabled = false;
				
				Application.LoadLevel(levelToLoad);
			}
		}
	}
}