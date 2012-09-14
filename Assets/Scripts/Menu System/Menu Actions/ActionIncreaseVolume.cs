using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
public class ActionIncreaseVolume : ActionBase
{
    protected override void DoActualAction()
    {
        //clsoe

       // item.mParentScreen.OnScreenInactive();
       //Debug.Log("mothafucka");
    }

#if UNITY_EDITOR
    public override bool OnMenuActionGUI(UIMenuItem item)
    {
        bool ret = base.OnMenuActionGUI(item);

        return (ret);
    }
#endif

}