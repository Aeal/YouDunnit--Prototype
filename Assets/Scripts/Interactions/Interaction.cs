using System;
using UnityEngine;
using System.Collections;

public class Interaction : MonoBehaviour 
{

    public virtual void OnTriggerActivated(object sender, EventArgs e) 
	{
       //Debug.Log(gameObject.name + "'s Trigger activated");
	}
}
