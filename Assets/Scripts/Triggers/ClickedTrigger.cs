using System;
using UnityEngine;
using System.Collections.Generic;




public class ClickedTrigger : Trigger
{
    
	// Use this for initialization
	void Start ()
	{
	    base.Start();
	    Inputbase.Instance.OnActionButtonPressedHandle += OnActivated;
	}
	
	// Update is called once per frame
    private void OnActivated(object sender, ClickedEventArgs e)
    {
        if (e.TargetObject != gameObject) return;
       //Debug.Log(name + "Is Activated");
        Activated();

    }

    void OnDestroy()
    {
        Inputbase.Instance.OnActionButtonPressedHandle -= OnActivated;
        
    }
}
