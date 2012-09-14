using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


public class EasterInterAction : Interaction
{
    private static int numActions = 0;
    private static int numActionsUsed = 0;
    public string GoToScene = "mMansionBkup";
    private bool used = false;
    public void Start()
    {
        numActions++;
    }
    public override void OnTriggerActivated(object sender, EventArgs e)
    {
        //Maybe add a loader here
        if(used) return;
        used = true;
        numActionsUsed++;
       //Debug.Log("Total actions required: " + numActions + "Total actions found: " + numActionsUsed);
        if (numActionsUsed >= numActions)
        {
            Application.LoadLevelAdditive(GoToScene);
        }
    }

  

}

