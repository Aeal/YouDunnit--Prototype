using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


public class DoorInteraction : Interaction
{
    
    private const float maxDistance = 4.0f; // The max distance the player can be from the door to click on it and still activate
    private bool isOpen;
    [SerializeField]
    private bool IsOpenAtStart;
    [SerializeField]
    private bool Inverted;

    private float rotAmount = 90,
                  minPos;
    public string OpenSound, CloseSound;

    private void Start()
    {

        minPos = transform.localEulerAngles.y;
        
        if(IsOpenAtStart)
            Open();
    }

    public override void OnTriggerActivated(object sender, EventArgs e)
    {
        if (!isOpen)
        {
             Open();
        }
        else
        {
            Close();
        }
    }

    private void Open()
    {   
        //SoundManager.Play3DSound(gameObject, OpenSound);
        if (Inverted) rotAmount = 90;
        else rotAmount = -90;
        iTween.RotateTo(gameObject, iTween.Hash("y", minPos + rotAmount, "islocal", true, "time", .5));
        isOpen = true;
    }

    private void Close()
    {
        //SoundManager.Play3DSound(gameObject, CloseSound);
        
        iTween.RotateTo(gameObject, iTween.Hash("y", minPos, "islocal", true, "time", .5));
        isOpen = false;
    }


}

