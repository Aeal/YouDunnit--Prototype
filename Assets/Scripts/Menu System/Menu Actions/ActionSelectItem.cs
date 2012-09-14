using UnityEngine;
using System.Collections;
#if UNITY_EDITOR
using UnityEditor;
#endif
public enum AccuseItemSlot
{
    MurderItem,
    DroppedItem,
    Motive,
}
public class ActionSelectItem : ActionBase
{
    public int selectItemIndex = 0;
    public AccuseItemSlot slot = AccuseItemSlot.MurderItem;
    protected override void DoActualAction()
    {
        switch(slot)
        {
            case AccuseItemSlot.MurderItem:
                AccusationResults.CharacterItem1 = WatchItemIndex.ItemsInPlay[selectItemIndex];
                break;
            case AccuseItemSlot.DroppedItem:
                AccusationResults.CharacterItem2 = WatchItemIndex.ItemsInPlay[selectItemIndex];
                break;
            case AccuseItemSlot.Motive:
                AccusationResults.CharacterMotive = MotiveManager.Instance.Motives[AccusationResults.CurrentCharacterAccusing][selectItemIndex];
                break;
        }
        base.DoActualAction();
    }

    #if UNITY_EDITOR

    public override bool OnMenuActionGUI(UIMenuItem item)
    {
        GUILayout.Label("Select Item Action");
        return base.OnMenuActionGUI(item);
    }
    #endif
}
