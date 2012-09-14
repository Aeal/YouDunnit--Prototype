using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class RadioInteraction : Interaction
{
    AudioSource mAudio;
    private bool volumeOn = true;
	// Use this for initialization
	void Start () {
        mAudio = this.audio;
       mAudio.Play();
	}


    public override void OnTriggerActivated(object sender, EventArgs e)
    {
        if (volumeOn)
            TurnOffVolume();
        else
            TurnOnVolume();

    }

    private void TurnOffVolume()
    {
        mAudio.volume = 0.0f;
        volumeOn = false;
    }

    private void TurnOnVolume()
    {
        mAudio.volume = 1.0f;
        volumeOn = true;
    }

}
