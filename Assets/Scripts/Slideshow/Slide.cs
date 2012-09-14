using System;
using UnityEngine;
using System.Collections;

[Serializable]
public class Slide
{
    public Texture2D picture;
    public AudioClip sound;
	public string subtitle;
    public float fadeTime;
    public delegate void OnClipCompleteHandle(float fadeTime);
    public static event OnClipCompleteHandle OnClipComplete;

    public void Show()
    {
      //Debug.Log("Showing");
       SlideShow.source =  SoundManager.Play3DSoundWithCallback(Camera.mainCamera.gameObject, sound, new SoundCallBack(OnClipCompleteHandler));
	   SlideShow.mCurrentText = subtitle;
    }

    private void OnClipCompleteHandler()
    {
        if (OnClipComplete != null)
            OnClipComplete(fadeTime);
    }
}
