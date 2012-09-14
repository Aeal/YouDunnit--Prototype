using UnityEngine;
using System.Collections.Generic;
using System;

public class Trigger : MonoBehaviour
{
    public delegate void OnTriggerActivatedHandler(object sender, EventArgs e);
    public event OnTriggerActivatedHandler OnTriggerActivated;
    private Interaction[] Interactions;

    protected void Start()
    {
       //Debug.Log("Creating Trigger");
        Interactions = gameObject.GetComponents<Interaction>();

        foreach (Interaction action in Interactions)
        {
           //Debug.Log("Adding interaction: " + action.GetType());
            OnTriggerActivated += action.OnTriggerActivated;
        }
    }

    protected void Activated()
    {
       //Debug.Log(gameObject.name + "'s Activated");
        if (OnTriggerActivated != null) OnTriggerActivated(this, EventArgs.Empty);
    }

   

}
