using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class ActionQuitGameFull : ActionBase
{
    // Action
    protected override void DoActualAction()
    {
		Application.Quit();
    }
#if UNITY_EDITOR
    // Editor
    public override bool OnMenuActionGUI(UIMenuItem item)
    {
		GUILayout.Label("Quit Game (Fully) Action");
    	return (base.OnMenuActionGUI(item));
    }
#endif
}
