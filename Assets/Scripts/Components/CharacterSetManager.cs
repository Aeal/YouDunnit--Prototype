using UnityEngine;
using System.Collections;
using Random = System.Random;

public class CharacterSetManager 
{

    private const int NUM_SETS = 5;
    private static int setID;
    public static int SetID
    {
        get { return setID; }
    }
    public static CharacterName[] CurrentCharacterSet
    {
        get { return CharacterSet[setID]; }
    }
    private static CharacterName[][] CharacterSet = new CharacterName[NUM_SETS][]
    {
        new CharacterName[] { CharacterName.Malory, CharacterName.Butler, CharacterName.Kurtz,      CharacterName.Deborah,   CharacterName.Sinclair,   CharacterName.Wellsworth },
        new CharacterName[] { CharacterName.Malory, CharacterName.Butler,CharacterName.Bouchez,    CharacterName.Elizabeth, CharacterName.Bennington, CharacterName.Cunningham },
        new CharacterName[] { CharacterName.Malory, CharacterName.Butler,CharacterName.Elizabeth,  CharacterName.Dorothy,   CharacterName.Sinclair,   CharacterName.Quinn      },
        new CharacterName[] { CharacterName.Malory, CharacterName.Butler,CharacterName.Kurtz,      CharacterName.Deborah,   CharacterName.Bouchez,    CharacterName.Bennington },
        new CharacterName[] { CharacterName.Malory, CharacterName.Butler,CharacterName.Cunningham, CharacterName.Dorothy,   CharacterName.Wellsworth, CharacterName.Quinn      }
    };

    private static CharacterSetManager managerInstance = null;
	
	

    public static void GenerateSceneario()
    {
        setID = new Random().Next(0, 5);
        //Debug.Log("Character Set ID: " + setID);
        /*foreach (var characterName in CurrentCharacterSet)
        {
           //Debug.Log("SET CHARACTER: " + characterName);
        }*/
    }
	
}
