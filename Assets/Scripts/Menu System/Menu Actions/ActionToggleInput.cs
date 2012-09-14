using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class ActionToggleInput: ActionBase
{
    Menu m_MenuRef;
    public bool inputActive = true,
                showCursor = false,
                lockCursor = true,
                pauseGameTime = false,
				reticle = false;
    // Action
    protected override void DoActualAction()
    {
        m_MenuRef = GameObject.Find("Editor").GetComponent<Menu>();
        if (m_MenuRef != null)
        {
            m_MenuRef.Movement.Enabled = inputActive;
			if (!inputActive)m_MenuRef.Movement.Reset();
            m_MenuRef.Cursor.Enabled = !showCursor;
            Screen.lockCursor = lockCursor;
            Screen.showCursor = showCursor;
			GameManager.Instance.GetComponent<Reticle>().enabled = reticle;
            Time.timeScale = (pauseGameTime) ? 0 : 1;
        }
        
        PCInput.Instance.Disabled = !inputActive;
    }
#if UNITY_EDITOR
    // Editor
    public override bool OnMenuActionGUI(UIMenuItem item)
    {
		GUILayout.Label("Toggle Input Action");
        inputActive = GUILayout.Toggle(inputActive, "Enable Input");
        showCursor = GUILayout.Toggle(showCursor, "Show Cursor");
        lockCursor = GUILayout.Toggle(lockCursor, "Lock Cursor");
        pauseGameTime = GUILayout.Toggle(pauseGameTime, "Pause Game (timeScale = 0)");
		reticle = GUILayout.Toggle(reticle, "Show reticle");
    	return (base.OnMenuActionGUI(item));
    }
#endif
}
