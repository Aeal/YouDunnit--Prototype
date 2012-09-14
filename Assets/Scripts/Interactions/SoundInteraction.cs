using UnityEngine;
using System.Collections;

public class SoundInteraction : Interaction 
{

    public AudioClip clipToPlay;
    public bool closePopup;
    public override void OnTriggerActivated(object sender, System.EventArgs e)
    {
       //Debug.Log("Playing sound: " + clipToPlay.name);
        var audio = GetComponent<AudioSource>();
        if (audio != null)
        {
            //HACK to remove extra audio sources
            Destroy(audio);
        }
        SoundManager.Play3DSoundWithCallback(gameObject, clipToPlay, new SoundCallBack(ClosePopup));
        base.OnTriggerActivated(sender, e);
    }

    void ClosePopup()
    {
        if (closePopup)
        {
            
            GetComponent<PopupInteraction>().Close();
        }
    }

	
}
