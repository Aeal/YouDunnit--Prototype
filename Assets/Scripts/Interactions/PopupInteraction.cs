using UnityEngine;
using System.Collections;

public class PopupInteraction : Interaction
{
    public string Message;
    bool isShowing = false;

    public override void OnTriggerActivated(object sender, System.EventArgs e)
    {
        isShowing = true; ;
        base.OnTriggerActivated(sender, e);
    }

    void OnGUI()
    {
        if (!isShowing) return;

        GUILayout.BeginArea(new Rect(Screen.width * .4f, Screen.height * .4f, Screen.width * .2f, Screen.height * .2f),"box");
        GUILayout.TextArea(Message);
        isShowing = !GUILayout.Button("Close");
        GUILayout.EndArea();
    }

    public void Close()
    {
        isShowing = false;
    }
}
