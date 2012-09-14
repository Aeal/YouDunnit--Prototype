using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class ActionSelectDossier : ActionBase
{
	Texture2D currentTex;
    public int characterIndex;
	private int MaxCharacterWidth = 45;
    public int Selected = 0,
				OtherSelected = 1;
    public  UIScreen dossiersDetailScreen,
			dossierMainScreen;
	CharacterName[] m_characters;
	string characterPrefab,
			dossiersDetailData;
    protected override void DoActualAction()
    {
		
		if ( (m_characters = CharacterSetManager.CurrentCharacterSet) == null )
		{
			Debug.LogError("SELECT DOSSIER: Error getting characters");
		}
		string currentCharacter = m_characters[characterIndex].ToString();
		string dossierTextureName = currentCharacter + "Dossier"; // Lol look how lazy i am  - Tito
		characterPrefab = currentCharacter + "(Clone)";
		Debug.Log("ACTIVATING " + dossierTextureName);
		if ( dossierMainScreen != null)
		{
			foreach(UIMenuItem item in dossierMainScreen.mMenuItems)
			{
				if ( item.gameObject.name == "FolderBackground")
				{
					Debug.LogWarning("DOSSIERS LOOKING FOR THIS THING: " + dossierTextureName);
					currentTex = item.gameObject.GetComponent<UIDossiersCharacterLoader>().GetCharacterPortraits[dossierTextureName];
					
					Debug.Log("I FOUND FOLDER BACKGROUND AND got texture " + currentTex);
					break;
				}
			}	
		}
		GameObject charPrefab = GameObject.Find (characterPrefab);
		if ( charPrefab == null)
		{
			if ( (charPrefab = GameObject.Find (currentCharacter)) == null)
			{
				// shit gone wrong	
			}
			else
			{
				dossiersDetailData =  charPrefab.GetComponent<NonPlayableCharacter>().DossierData;
			}
		}
		else
		{
			dossiersDetailData =  charPrefab.GetComponent<NonPlayableCharacter>().DossierData;
		}
		
		
        if (dossiersDetailScreen != null)
        {

			foreach(UIMenuItem item in dossiersDetailScreen.mMenuItems)
			{

				if (item.gameObject.name == "Title")
				{
					TextMesh tt = item.gameObject.transform.Find("TitleMesh").GetComponentInChildren<TextMesh>();

					tt.text = currentCharacter;
					//tt.text = 
					//GameObject titleMesh = item.gameObject.transform.Find("TitleMesh");
					//titleMesh.GetComponent<TextMesh>
				}
				else if ( item.gameObject.name == "Details" )
				{
					TextMesh tt = item.gameObject.transform.Find("TextMesh").GetComponentInChildren<TextMesh>();
					if (MaxCharacterWidth < 10)
			        {
			            MaxCharacterWidth = 10;
			           //Debug.Log("Dont make the max width so small dude.");
			        }
			
			        string[] myTempString = dossiersDetailData.Split(' ');
			        string tempMessage = "";
			        int tempLength = 0;
			        foreach (string str in myTempString)
			        {
			            if ((tempLength + str.Length +1) > MaxCharacterWidth)
			            {
			                tempMessage = tempMessage +  "\n" + str + " " ;
			                tempLength = str.Length + 1;
			            }
			            else
			            {
			                tempLength += str.Length + 1;
			                tempMessage = tempMessage + str + " ";
			                //Debug.Log(tempLength);
			            }
			        }
			        dossiersDetailData = tempMessage;
					tt.text = dossiersDetailData;
					//tt.text = "LOLOLOLOL";
				}
				else if ( item.gameObject.name == "Character Portrait")
				{
					if ( currentTex != null )
					{
						item.gameObject.renderer.material.SetTexture("_MainTex", currentTex);
					}
				}
			}
        }

    }
	
#if UNITY_EDITOR
    // Editor
    public override bool OnMenuActionGUI(UIMenuItem item)
    {
        characterIndex = EditorGUILayout.IntField("Character index: ", characterIndex);

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.BeginVertical();
        GUILayout.Label("Select Dossier Action");

        int oldSelected = Selected;
		int old2Selected = OtherSelected;
		
        Selected = EditorGUILayout.Popup("Selected Dossier", Selected, item.ParentScreen.ScreenMenu.mScreenListNames.ToArray());
		
		OtherSelected = EditorGUILayout.Popup("Main Dossier Screen", OtherSelected, item.ParentScreen.ScreenMenu.mScreenListNames.ToArray());
        if (oldSelected != Selected || dossiersDetailScreen == null)
        {
            dossiersDetailScreen = item.ParentScreen.ScreenMenu.mScreenList[Selected];
        }
		if (old2Selected != OtherSelected || dossierMainScreen == null)
        {
            dossierMainScreen = item.ParentScreen.ScreenMenu.mScreenList[OtherSelected];
        }
        EditorGUILayout.EndVertical();
        EditorGUILayout.EndHorizontal();
       
        return (base.OnMenuActionGUI(item));
    }
#endif
}

