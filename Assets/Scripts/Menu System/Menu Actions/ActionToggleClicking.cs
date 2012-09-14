using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
//Toggles the players ability to click on in game items based on UI commands;
	class ActionToggleClicking : ActionBase
	{
        public bool Enable;
        protected override void DoActualAction()
        {
            Inputbase.Instance.Disabled = !Enable;
            base.DoActualAction();
        }

#if UNITY_EDITOR
        public override bool OnMenuActionGUI(UIMenuItem item)
        {
            GUILayout.Label("Toggle Clicking action");
            Enable = EditorGUILayout.Toggle("Enable clicking: ", Enable); 
            return base.OnMenuActionGUI(item);
        }
#endif
	}

