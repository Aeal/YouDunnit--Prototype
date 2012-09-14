//*****************************************************************************
// ScreenEditor - Menu item which belongs to a particular UIScreen
// Author: Tyler Ortiz
// Date: 1/25/12
//*****************************************************************************
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.Reflection;
//*****************************************************************************
[ExecuteInEditMode]
[CustomEditor(typeof(UIScreen))]
//*****************************************************************************
public class ScreenEditor : Editor
{
	//static bool onScreen = true;
    static List<Type> derivedList;
    static List<ActionBase> convertedList;
    private static string[] mPotentialActions;
    static int currentAction = 0;
    private static GUIStyle mBoxStyle = new GUIStyle("box");
    //*************************************************************************
    #region Menu Item Editor Interfaces
    //*************************************************************************
    private static void AddMenuItem(UIScreen screen)
    {
        if (GUILayout.Button("Add Menu Item"))
        {

            if (screen.mMenuItems == null)
            {
                screen.mMenuItems = new List<UIMenuItem>();
            }

            GameObject newObject = (GameObject)Instantiate(Resources.Load("Prefabs/Menu System Prefabs/UIItemPrefab", typeof(GameObject)));
            newObject.name = "New UI Item (untitled)";
            newObject.transform.parent = screen.gameObject.transform.FindChild("Menu Items");
            newObject.transform.localPosition = new Vector3(0, 0, screen.ScreenLayer);
            newObject.transform.localScale = Vector3.one;
            newObject.transform.localRotation = Quaternion.identity;

            UIMenuItem newItem = newObject.GetComponent<UIMenuItem>();
            newItem.ParentScreen = screen;
            screen.mMenuItems.Add(newItem);
            EditorUtility.SetDirty(screen);
        }
    }
    //*************************************************************************
    private static bool DisplayMenuItem(int index, UIMenuItem item, UIScreen screen)
    {
        bool ret = true; // Are we breaking the calling loop

        EditorGUILayout.Separator();
        item.EditorView = EditorGUILayout.InspectorTitlebar(item.EditorView, item.gameObject);

        // If we are viewing all the item's properties then display them all
        if (item.EditorView == true)
        {
            // ****************************************************************
            // Item Standard Properties
            // ****************************************************************
            GUILayout.Label("Menu Item Properties");
            EditorGUILayout.BeginVertical(mBoxStyle);
            EditorGUILayout.BeginHorizontal();

            item.name = EditorGUILayout.TextField("Name:", item.name);

            if (GUILayout.Button("Delete", GUILayout.Width(75)))
            {
                // Immediately destroy the item, which will break the list
                //  in the screen, so we need to break out of parent loop.
                DestroyImmediate(item.gameObject);
                screen.mMenuItems.Remove(item);
                ret = false;
                return (ret);
            }

            EditorGUILayout.EndHorizontal();
            EditorGUILayout.EndVertical();
			
			EditorGUILayout.BeginVertical(mBoxStyle);
			EditorGUILayout.BeginHorizontal();
			item.mTime = EditorGUILayout.FloatField("Time IN:", item.mTime);
			item.mTime2 = EditorGUILayout.FloatField("Time OUT:", item.mTime2);
			EditorGUILayout.EndHorizontal();
            EditorGUILayout.EndVertical();
            // ****************************************************************
            // Behavior Editor Section
            // ****************************************************************
            GUILayout.Label("Behavior(s) - specify what this item can do.");

            EditorGUILayout.BeginVertical(mBoxStyle);
            EditorGUILayout.BeginHorizontal();

            // Action Editor
            UpdateMenuActions(screen, item);

            EditorGUILayout.EndHorizontal();
            EditorGUILayout.EndVertical();
            /*
                if (item.ItemAction != UIMenuAction.None)
                {
                    EditorGUILayout.BeginVertical(mBoxStyle);
                    EditorGUILayout.BeginHorizontal();
                    switch (item.ItemAction)
                    {
                        case UIMenuAction.None:
                            break;
                        case UIMenuAction.CloseCurrentScreen:
                            break;
                        case UIMenuAction.OpenNewScreen:
                            int indexOld = item.mSelectedScreenIndex;
                            string[] mArray = screen.mMenuRef.mScreenListNames.ToArray();
                            item.mSelectedScreenIndex = EditorGUILayout.Popup("Screen to open", item.mSelectedScreenIndex, mArray);
                            break;
                    }

                    EditorGUILayout.EndHorizontal();
                    EditorGUILayout.EndVertical();
                }*/

            // ****************************************************************
            // Visual Editor Section
            // ****************************************************************
            GUILayout.Label("Visual - texture and positioning.");
            EditorGUILayout.BeginVertical(mBoxStyle);

            item.ItemTexture = (Material)EditorGUILayout.ObjectField("Texture", item.ItemTexture, typeof(Material), true);

            // Position Editor

            UpdateMenuItemPosition(item);



            EditorGUILayout.EndVertical();

        } // end item view 

        return (ret);
    }
    public static List<Type> GetClasses( Type baseType)
    {
        List<Type> ret;
        ret = new List<Type>();
        Type[] list = Assembly.GetAssembly(baseType).GetTypes();
        foreach (Type t in list)
        {
            if (t.IsSubclassOf(baseType))
            {
                ret.Add(t);
            }
        }
        
        return ret;
    }
    //*************************************************************************
    private static void UpdateMenuItemPosition(UIMenuItem item)
    {
        if (item.ItemPositionState == UIMenuItemPositionState.OnScreen)
        {
            EditorGUILayout.PrefixLabel("Active (Editor)");
        }
        else
        {
            EditorGUILayout.PrefixLabel("Inactive");
        }

        item.ItemScreenPositionOn = EditorGUILayout.Vector3Field("Position - ON", item.ItemScreenPositionOn);

        if (item.ItemPositionState == UIMenuItemPositionState.OffScreen)
        {
            EditorGUILayout.PrefixLabel("Active (Editor)");
        }
        else
        {
            EditorGUILayout.PrefixLabel("Inactive");
        }

        item.ItemScreenPositionOff = EditorGUILayout.Vector3Field("Position - OFF", item.ItemScreenPositionOff);
    }
    //*************************************************************************
    private static void UpdateMenuActions(UIScreen screen, UIMenuItem item)
    {
        

        /* EditorGUILayout.BeginHorizontal();*/
        EditorGUILayout.BeginVertical();
        
        if (mPotentialActions == null)
        {
            derivedList = GetClasses(typeof(ActionBase));
            mPotentialActions = new string[derivedList.Count];
            
            for(int i = 0; i < derivedList.Count; i++)
            {
                mPotentialActions[i] = derivedList[i].Name;
            } 
			item.mItemActions = item.gameObject.GetComponents<ActionBase>(); 
        }

		// Select current action type to add
        int oldAction = currentAction;
        currentAction = EditorGUILayout.Popup("Action To Add", currentAction, mPotentialActions);

        // Update list when selection changes
        if (oldAction != currentAction)
        {
            //convertedList.Clear();
            derivedList = GetClasses(typeof(ActionBase));
            mPotentialActions = new string[derivedList.Count];
            
            for (int i = 0; i < derivedList.Count; i++)
            {
                mPotentialActions[i] = derivedList[i].Name;
            }
        }


		if (GUILayout.Button("Add Action" + mPotentialActions[currentAction]))
        {
            //object actionToAdd = derivedList[currentAction].GetConstructor(Type.EmptyTypes).Invoke(null);// System.Activator.CreateInstance(derivedList[currentAction]);
            //UIMenuAction action = (UIMenuAction)actionToAdd;
			Type toAdd = derivedList[currentAction];
            item.gameObject.AddComponent(derivedList[currentAction]);
            item.mItemActions = item.gameObject.GetComponents<ActionBase>(); 
            
        }
        
      

        BoxCollider bc;
        if (item.mItemActions != null)
        {
            if (item.mItemActions.Length == 0)
            {
                GUILayout.Label("No actions");
            }
            else
            {
                ActionBase action;

                for (int i = 0; i < item.mItemActions.Length; i++)
                {

                    action = (ActionBase)item.mItemActions[i] ;

                    

                    if (action.OnMenuActionGUI(item))
                    {
                        DestroyImmediate(item.mItemActions[i]);
                        item.mItemActions[i] = null;
                        item.mItemActions = item.gameObject.GetComponents<ActionBase>(); 
                        EditorUtility.SetDirty(screen);
                        break;
                    }

                }
                
            }


        }
        else
        {
            item.mItemActions = item.gameObject.GetComponents<ActionBase>();
        }
        /*EditorGUILayout.EndHorizontal();*/
        EditorGUILayout.EndVertical();

        /*UIMenuAction oldAction = item.ItemAction;

        item.ItemAction = (UIMenuAction)EditorGUILayout.EnumPopup("Menu Action", item.ItemAction);

        if (oldAction != item.ItemAction)
        {
            BoxCollider bc;
            if (item.ItemAction != UIMenuAction.None)
            {
                if ((bc = item.gameObject.GetComponent<BoxCollider>()) == null)
                {
                    bc = item.gameObject.AddComponent<BoxCollider>();
                }
            }
            else
            {
                if ((bc = item.gameObject.GetComponent<BoxCollider>()) != null)
                {
                    DestroyImmediate(bc);
                }
            }

            EditorUtility.SetDirty(screen);
        }*/

    }
    //*************************************************************************
    #endregion
    //*************************************************************************
    // EditScreen(UIScreen screen) - called from MenuEditor
    //*************************************************************************
	
	public static void UpdateScreen(UIScreen screen)
	{
		if ( screen.mIsViewing )
		{
			for(int i = 0; i < screen.mMenuItems.Count; i++)
			{
				UIMenuItem item = screen.mMenuItems[i];
                if (item != null)
                {
                    item.ItemPositionState = UIMenuItemPositionState.OnScreen;
                    item.UpdatePosition();
                }
                else
                {
                    screen.mMenuItems.RemoveAt(i);
                    
                }

			}
		}
		else
		{
			for(int i = 0; i < screen.mMenuItems.Count; i++)
			{
				UIMenuItem item = screen.mMenuItems[i];
                if (item != null)
                {
                    item.ItemPositionState = UIMenuItemPositionState.OffScreen;
                    item.UpdatePosition();
                }
                else
                {
                    screen.mMenuItems.RemoveAt(i);
                }
			}
		}
	}
    public static void EditScreen(UIScreen screen)
    {
        EditorGUILayout.LabelField("Menu List", screen.mMenuItems.Count.ToString() + " menu item(s) on this screen.");
		
		
        AddMenuItem(screen);

        if (screen.mMenuItems != null)
        {
			for ( int i = 0; i < screen.mMenuItems.Count; i++)
			{
				if (DisplayMenuItem(i,screen.mMenuItems[i], screen) == false)
                {
                    EditorUtility.SetDirty(screen);
                    break;
                }
			}
        }
    }
    //*************************************************************************
}
//*****************************************************************************
