using System;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;

[Serializable]
public class SlideShow : MonoBehaviour
{

    public bool StartOnStart;

    public  string      SceneToLoad = "";
	public  List<Slide> Slides = new List<Slide>();
    public  bool        allowSkip,
                        pauseForUser;
    public ScaleMode    ScaleMode;
    private int         Index = 0;
	public bool bSubtitlesOn;

    private float FadeTime = 2.5f, 
                  ElapsedTime= 0,
                  Opacity = 1;

    private bool isFading,
                 isFinished;

    public static AudioSource source;
	public static string mCurrentText;
    public GUISkin MySkin;

    public  void Start()
    {
		if(GameObject.Find("GameManager").GetComponent("Reticle"))
			Destroy(GameObject.Find("GameManager").GetComponent(typeof (Reticle)));
		
		bSubtitlesOn = true;
        MySkin = Resources.Load("UserInterface/Conversation/ConversationUI") as GUISkin;
		
		if(!Screen.showCursor)
			Screen.showCursor = true;
		
		if(Screen.lockCursor)
			Screen.lockCursor = false;
		
		//Debug.Log("Cursor on: " + Screen.showCursor);
		//Debug.Log("Cursor locked: " + Screen.lockCursor);
		
        Slide.OnClipComplete += IncrementSlide;
        if (StartOnStart)
            BeginShow();
    }

    public void BeginShow()
    {
        Slides[0].Show();
        
    }

    public void IncrementSlide(float fadeTime)
    {
        FadeTime = fadeTime;
        BeginFade();
    }

    private void BeginFade()
    {
        isFading = true;
    }

    private  void Update()
    {
        if(isFinished)return;
        if(isFading)
        {
            if(ElapsedTime < FadeTime)
            {
                ElapsedTime += Time.smoothDeltaTime;
                Opacity = 1.0f - (ElapsedTime/FadeTime);
            }
            else
            {
                isFading = false;
                ElapsedTime = 0;
                Index++;
                if (Index >= Slides.Count)
                {
                    isFinished = true;
                    if (!pauseForUser)
                        OnFinished();
                }
            }
        }
        if(Opacity != 1 && !isFading)
        {
            if(ElapsedTime < FadeTime)
            {
                ElapsedTime += Time.smoothDeltaTime;
                Opacity = ElapsedTime/FadeTime;
            }
            else
            {
                ElapsedTime = 0;
                Slides[Index].Show();
                Opacity = 1;
            }
        }
        
        
    }

    public void OnGUI()
    {
        GUI.skin = MySkin;

        if (!isFinished)
        {

            GUI.color = new Color(GUI.color.r, GUI.color.g, GUI.color.b, Opacity);
            GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), Slides[Index].picture, ScaleMode);
        }
        else
        {
            if (pauseForUser)
            {
                if (GUI.Button(new Rect(Screen.width * .45f, Screen.height * .45f, Screen.width * .1f, Screen.height * .1f), "Finished"))
                {
                    OnFinished();
                }
            }
        }

        if (allowSkip)
        {
            if (GUI.Button(new Rect(0, 0, Screen.width * .1f, Screen.height * .1f), "Skip"))
            {
                isFinished = true;
                source.Stop();
                OnFinished();
            }

        }
		
		if(bSubtitlesOn)
		{
			if(mCurrentText != null)
			{
				GUI.skin.box.wordWrap = true;
				//GUIStyle style = GUI.skin.FindStyle("box");
				//style.padding.top =-9;
					GUI.Box(new Rect(50, Screen.height - 100, Screen.width - 100, 50), mCurrentText);
			}
		}
        
    }

    private void OnFinished()
    {
		//Debug.Log("Scene to load is: " + SceneToLoad);
		if(SceneToLoad != "00_MainMenu")
		{
			Screen.showCursor = false;
			Screen.lockCursor = true;
		}
		else
		{
			Debug.Log ("Should show cursor");
			Screen.showCursor = true;
			Screen.lockCursor = false;
		}
		
        Application.LoadLevel(SceneToLoad);
        Destroy(gameObject);
    }
}
