using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

// ****************************************************************************
// 
// ****************************************************************************
/*public enum UIMenuActionType
{
    CloseCurrentScreen,         // Close the current screen this item is on
    OpenNewScreen,              // Open a new screen
    SwitchScene,                // Switch unity scenes
    IncreaseVolume,             // Increase game volume
    DecreaseVolume,             // Decrease game volume
};*/
// ****************************************************************************
// Menu Item Interaction Type
// ****************************************************************************
public enum UIMenuItemInteractionType
{
    None,
    ExternalTrigger,
    MouseClick,         // Action is triggered with a click on this item    
    KeyboardPress,      // Action is triggered with a keypress of this item's key
    MouseKeyboard,      // Action is triggered under either of previous conditions
};
// ****************************************************************************
// Base Menu Action
// ****************************************************************************
[Serializable]
public class ActionBase : MonoBehaviour
{
    public KeyCode ActionKey;
    public UIMenuItemInteractionType EventType;
    public int ActionIndex = 0;


    public void ExternalAction()
    {
        //DoActualAction();
        DoAction(new UIActionRegisterArgs(UIMenuItemInteractionType.ExternalTrigger, KeyCode.None));
    }
    protected   virtual void DoActualAction(){}

    // Action Base
    public virtual void DoAction(UIActionRegisterArgs args)
    {

        if (args.Key != this.ActionKey && args.InteractionType != UIMenuItemInteractionType.MouseClick && args.InteractionType != UIMenuItemInteractionType.ExternalTrigger)
        {
            return;
        }
        else
        {
            DoActualAction();
        }

    }

    // Editor Base
    #if UNITY_EDITOR
    public virtual bool OnMenuActionGUI(UIMenuItem item)
    {
        EventType = (UIMenuItemInteractionType)EditorGUILayout.EnumPopup("Trigger Type", EventType);
        if (EventType == UIMenuItemInteractionType.KeyboardPress || EventType == UIMenuItemInteractionType.MouseKeyboard)
        {
            ActionKey = (KeyCode)EditorGUILayout.EnumPopup("Key", ActionKey);

        }

        if (GUILayout.Button("Delete Action"))
        {
            DestroyImmediate(this, false);
            return true;
        }
        return false;
    }
    #endif
};
// ****************************************************************************