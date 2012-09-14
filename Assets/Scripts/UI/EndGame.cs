using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class Endgame : MonoBehaviour
{
	public static event GameEndHandle OnGameEnd, OnGameEndCancel;
	public delegate void GameEndHandle();
	
	
    private bool isEnding;
    private int currentCharacter,
                firstItem,
                secondItem,
                motiveSelection;

    private string[] items, characters, motives;

    private State endGameState;
    private enum State
    {
        Default,
        SelectingCharacter,
        SelectingItem1,
        SelectingItem2,
        SelectingMotive,
    }

    private void Start()
    {
        characters = GameManager.Instance.Characters.Keys.ToArray();
        items      = GameManager.Instance.Items.Keys.ToArray();
        motives    = GameManager.Instance.Motives.Values.Select(VARIABLE => VARIABLE.DialogText).ToList().ToArray();
        
    }
    private void OnGUI()
    {
        
        if(characters.Length == 0 )
            characters = GameManager.Instance.Characters.Keys.ToArray();
        if (items.Length == 0)
            items = GameManager.Instance.Items.Keys.ToArray();
        if (motives.Length == 0)
            motives = GameManager.Instance.Motives.Values.Select(VARIABLE => VARIABLE.DialogText).ToList().ToArray();

        GUILayout.BeginArea(new Rect(Screen.width *.25f,Screen.height*.25f,Screen.width *.5f, Screen.height *.5f),"","box");
        switch (endGameState)
        {
            case State.Default:
                DrawDefault();
                break;

            case State.SelectingCharacter:
                currentCharacter = DrawSelectionGrid(characters,currentCharacter);
                break;

            case State.SelectingItem1:
                firstItem = DrawSelectionGrid(items, firstItem);
                break;

            case State.SelectingItem2:
                secondItem = DrawSelectionGrid(items, secondItem);
                break;

            case State.SelectingMotive:
                motiveSelection = DrawSelectionGrid(motives, motiveSelection);
                break;
        }
        GUILayout.EndArea();
    }

    void DrawDefault()
    {
        GUILayout.BeginHorizontal();
        if (GUILayout.Button(characters[currentCharacter]))
        {
            endGameState = State.SelectingCharacter;
        }
        GUILayout.Label("Killed Gott with the ");
        GUILayout.EndHorizontal();


        GUILayout.BeginHorizontal();
        if (GUILayout.Button(items[firstItem]))
        {
            endGameState = State.SelectingItem1;
        }
        GUILayout.Label("And dropped his ");
        GUILayout.EndHorizontal();



        GUILayout.BeginHorizontal();
        if (GUILayout.Button(items[secondItem]))
        {
            endGameState = State.SelectingItem2;
        }
        GUILayout.Label("in Gotts room ");
        GUILayout.EndHorizontal();


        GUILayout.BeginHorizontal();
        
        GUILayout.Label("He got killed because ");
        if (GUILayout.Button(motives[motiveSelection]))
        {
            endGameState = State.SelectingMotive;
        }
        GUILayout.EndHorizontal();
        if(GUILayout.Button("THEY DUNNIT"))
        {
            //run accousal routine
            Motive finalMotive = null;
            foreach (Motive motive in GameManager.Instance.Motives.Values.Where(motive => motive.DialogText == motives[motiveSelection]))
            {
                finalMotive = motive;
            }
            //GameManager.Instance.ConfirmAccuse(characters[currentCharacter], items[firstItem], items[secondItem], finalMotive);
        }
        if(GUILayout.Button("Oh... wait.. nevermind..."))
        {
            GameManager.Instance.CancelAccuse();
        }
        
    }

    int DrawSelectionGrid(string [] objects, int selected)
    {
        int current = selected;
        selected = GUILayout.SelectionGrid(selected, objects, 1);
        if ( current != selected || GUILayout.Button("Return") )
            endGameState = State.Default;
        return selected;
    }


}

