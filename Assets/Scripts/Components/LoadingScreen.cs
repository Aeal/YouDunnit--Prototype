using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class LoadingScreen : MonoBehaviour
{
	public Texture2D loadingTexture;
    //[HideInInspector] //Hide in inspector hides public memebers from the inspector
	public string LevelToLoad = "Mansionv2"; // commenting out so designers do not touch it but keep it coded in case we ever want it anywhere else
    public bool LoadOnStart = true;
	private AsyncOperation levelOperation;
	private float percentageLoaded = 0;
    public float mMinWaitTime = 2.0f;
    public GUIFader fader;
    public AudioClip clipToPlayOnLoadStart;
    private AudioSource source;
	
	void Start()
	{
        source = GetComponent<AudioSource>();
        if (source == null)
        {
            source = gameObject.AddComponent<AudioSource>();
			source.playOnAwake = false;
        }
        if (LoadOnStart)
            StartLoad(LevelToLoad);
		//StartCoroutine(LoadLevelAsync(levelToLoad));
	}
    public void StartLoad( string levelToLoad = "")
    {
        if (levelToLoad != "")
        {
            LevelToLoad = levelToLoad;
        }
        if (source != null && clipToPlayOnLoadStart != null && !source.isPlaying)
            source.PlayOneShot(clipToPlayOnLoadStart);
        Debug.Log("Loading level: " + LevelToLoad);
        StartCoroutine(LoadLevelAsync(LevelToLoad));

    }
	IEnumerator LoadLevelAsync(string level)
	{
		yield return new WaitForSeconds(mMinWaitTime);
		levelOperation = Application.LoadLevelAsync(level);
		
		while(!levelOperation.isDone)
			yield return 0;
	}
	
	void Update()
	{
        if (Application.GetStreamProgressForLevel(LevelToLoad) != 1)
		{
            percentageLoaded = Application.GetStreamProgressForLevel(LevelToLoad) * 100;
		}
        if (levelOperation != null && levelOperation.progress > .8)
        {
            fader.FadeTime = .5f;
            fader.StartFadeOut();
            
        }
	}
	
	void OnGUI()
	{
		GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), loadingTexture, ScaleMode.StretchToFill, true, 0);
		//GUI.TextArea(new Rect(0, 0, Screen.width/2, 100), percentageLoaded + "% complete");
		// add loading bar
	}
}