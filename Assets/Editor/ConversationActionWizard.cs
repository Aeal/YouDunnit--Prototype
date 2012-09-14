using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ConversationActionWizard : ScriptableWizard
{
    
    public static void CreateWizard(List<DialogItem> DialogItems)
    {
        DisplayWizard<ConversationActionWizard>("Add Default Actions", "Create", "Cancel");
        //convo.Show();
        items = DialogItems;
    }

    private GameObject mainGameObject, lookAtTarget;
    private string animationToPlay = "", animationLeave = "";
    private static List<DialogItem> items;

    void OnGUI()
    {
        mainGameObject = (GameObject)EditorGUILayout.ObjectField("Convo Owner: ", mainGameObject, typeof(GameObject), true);
        lookAtTarget = (GameObject)EditorGUILayout.ObjectField("Look at target: ", lookAtTarget, typeof(GameObject), true);
        animationToPlay = EditorGUILayout.TextField("Start Convo Animation: ", animationToPlay);
        animationLeave = EditorGUILayout.TextField("End Convo Animation: ", animationLeave);
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Apply"))  OnWizardCreate();
        if (GUILayout.Button("Cancel")) OnWizardOtherButton();
        EditorGUILayout.EndHorizontal();
    }

    void OnWizardCreate()
    {
        DialogItem first = items[0];
        DialogItem last = items[items.Count - 1];
        if (mainGameObject != null)
        {
            StopNav nav = new StopNav();
            nav.Initilize(ref first, NodeActionType.OnDialogFocused);
            nav.SetNodeTarget(ref mainGameObject);
            first.onNodeFocusedActions.Add(nav);
        }
        if (mainGameObject != null && lookAtTarget != null)
        {
            LookAtGameObject lookAt = new LookAtGameObject();
            lookAt.Initilize(ref first, NodeActionType.OnDialogFocused);
            lookAt.SetLooker(ref mainGameObject);
            lookAt.SetTarget(ref lookAtTarget);
            first.onNodeFocusedActions.Add(lookAt);
        }
        if (animationToPlay != "")
        {
            PlayAnimation anim = new PlayAnimation();
            anim.Initilize(ref first, NodeActionType.OnDialogFocused);
            anim.SetNodeTarget(ref mainGameObject);
            first.onNodeFocusedActions.Add(anim);

        }
        if (animationLeave != "")
        {
            PlayAnimation anim = new PlayAnimation();
            anim.Initilize(ref last, NodeActionType.OnDialogLeave);
            anim.SetNodeTarget(ref mainGameObject);
            last.onNodeLeaveActions.Add(anim);
        }
        if (mainGameObject != null)
        {
            EndConvoActions end = new EndConvoActions();
            end.Initilize(ref last, NodeActionType.OnDialogLeave);
            last.onNodeLeaveActions.Add(end);
        }
        CleanUp();
    }

    void OnWizardOtherButton()
    {
        CleanUp();
    }
    void CleanUp()
    {
        items = null;
        Close();

    }
}