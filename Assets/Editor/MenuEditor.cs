
//*****************************************************************************
//*****************************************************************************
// MansionMenuEditor.cs
// Author: Tyler Ortiz
// Date: 1/20/2012
// Purpose: Custom editor interface for Mansion Menu Scripts
//*****************************************************************************
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

//*****************************************************************************

[ExecuteInEditMode]
[CustomEditor(typeof(Menu))]
public class MenuEditor : Editor
{
    public static string mFilename = "";
    private Menu mMenuBeingEdited;
    private Transform mScreensFolder;
    public void Awake()
    {
        mMenuBeingEdited = (Menu)target;
        if (mMenuBeingEdited.mScreenList == null)
        {
            mMenuBeingEdited.mScreenList = new List<UIScreen>();
        }
        mScreensFolder = mMenuBeingEdited.transform.FindChild("Screens");
        
    }

    public override void OnInspectorGUI()
    {
        EditorGUILayout.LabelField("Parent Menu", mMenuBeingEdited.mScreenList.Count.ToString() + " screen(s) in this menu system.");
        mFilename = EditorGUILayout.TextField("Filename:", mFilename);
        if (GUILayout.Button("Save Menu System"))
        {
            SaveMenuSystem(mFilename);
            
        }

        //Debug.Log(mFilename);

        if (GUILayout.Button("Add Screen"))
        {
            AddScreen();
        }

        ShowScreenList();


    }

    static bool SaveMenuSystem(string filename)
    {
        bool ret = false;



        return (ret);
    }
    private void AddScreen()
    {
        if (mMenuBeingEdited != null)
        {
            
            if (mMenuBeingEdited.mScreenList == null)
            {
                mMenuBeingEdited.mScreenList = new List<UIScreen>();
            }

            GameObject newObject = (GameObject)Instantiate(Resources.Load("Prefabs/Menu System Prefabs/ScreenPrefab", typeof(GameObject)), Vector3.zero, Quaternion.identity);
            newObject.name = "New Screen";
            newObject.transform.parent = mScreensFolder;
            newObject.transform.localPosition = Vector3.zero;
            newObject.transform.localScale = Vector3.one;
            newObject.transform.localRotation = Quaternion.identity;// *Quaternion.Euler(new Vector3(270, 0, 0));

            UIScreen newScreen = newObject.GetComponent<UIScreen>();
            mMenuBeingEdited.mScreenList.Add(newScreen);
            newScreen.ScreenMenu = mMenuBeingEdited;

            GameObject newScreenDirectory = new GameObject();
            newScreenDirectory.name = "Menu Items";
            newScreenDirectory.transform.parent = newObject.transform;
            newScreenDirectory.transform.localPosition = Vector3.zero;
            newScreenDirectory.transform.localScale = Vector3.one;
            newScreenDirectory.transform.localRotation = Quaternion.identity;
            EditorUtility.SetDirty(mMenuBeingEdited);

            mMenuBeingEdited.UpdateScreenRegistry();
        }
    }

    private void ShowScreenList()
    {
        // List out all the menu items we have
        if (mMenuBeingEdited.mScreenList != null)
        {
            
			for( int i = 0; i < mMenuBeingEdited.mScreenList.Count; i++)
			{
				UIScreen screen = mMenuBeingEdited.mScreenList[i];
                
                // Are we inspecting this specific UI Item
				screen.mIsViewing = EditorGUILayout.InspectorTitlebar(screen.mIsViewing, screen.gameObject);
				ScreenEditor.UpdateScreen(screen);
                // If we are viewing all the item's properties then display them all
                if (screen.mIsViewing)
                {

                    // Begin UI Item Edit Region
                    EditorGUILayout.BeginVertical(new GUIStyle("box"));
                    EditorGUILayout.BeginHorizontal();

                    string newName = screen.gameObject.name = EditorGUILayout.TextField("Screen Name: ", screen.gameObject.name);
					if (i > 0)
					{
						if ( GUILayout.Button("^", GUILayout.Width(30)) )
						{
							
							UIScreen above = mMenuBeingEdited.mScreenList[i-1];
							UIScreen current = screen;
							mMenuBeingEdited.mScreenList[i] = above;
							mMenuBeingEdited.mScreenList[i - 1] = current;
							Debug.Log("SWAPPED UP");
						}
					}
					if (i < mMenuBeingEdited.mScreenList.Count)
					{
						if ( GUILayout.Button("v", GUILayout.Width(30)) )
						{
							
							UIScreen below = mMenuBeingEdited.mScreenList[i+1];
							UIScreen current = screen;
							mMenuBeingEdited.mScreenList[i] = below;
							mMenuBeingEdited.mScreenList[i+1] = current;
							Debug.Log("SWAPPED DOWN");
						}
					}
                    if (GUILayout.Button("Delete", GUILayout.Width(75)))
                    {
                        mMenuBeingEdited.mScreenList.Remove(screen);
                        mMenuBeingEdited.UpdateScreenRegistry();
                        DestroyImmediate(screen.gameObject);
                        EditorUtility.SetDirty(mMenuBeingEdited);
                        break;
                    }
                    EditorGUILayout.EndHorizontal();
                    EditorGUILayout.BeginHorizontal();
                    float oldLayer = screen.ScreenLayer;
                    int layerMax = (int)mMenuBeingEdited.gameObject.transform.FindChild("MenuBackplane").transform.localPosition.z;
                    
                    screen.ScreenLayer = EditorGUILayout.IntSlider("Layer: ", screen.ScreenLayer, 1, layerMax);
                    if (oldLayer != screen.ScreenLayer)
                    {
                        screen.gameObject.transform.localPosition = new Vector3(0, -screen.ScreenLayer, 0);
                    }
                    EditorGUILayout.EndHorizontal();

                    ScreenEditor.EditScreen(screen);
                    EditorGUILayout.EndVertical();
                  
                    
                } // end item view 
				
            } // end item loop
			
            mMenuBeingEdited.UpdateScreenRegistry();

        }

    }
}
//*****************************************************************************

