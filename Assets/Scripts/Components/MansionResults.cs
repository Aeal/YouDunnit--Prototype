using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

	public class MansionResults
	{
        private static Dictionary<string, float> characters = new Dictionary<string, float>();
	    private static Dictionary<string, Character> allCharacters = new Dictionary<string, Character>();
		private static Dictionary<Character, float> charSuspicion = new Dictionary<Character, float>();
        public static Dictionary<string, Character> Characters { get { return allCharacters; } set { allCharacters = value; } }
		private static MainCharacter mPlayerRef;
		private static Character accusedCharacter = null;
	
		public static void Clear()
		{
	         characters = new Dictionary<string, float>();
		     allCharacters = new Dictionary<string, Character>();
			 charSuspicion = new Dictionary<Character, float>();
			 accusedCharacter = null;
		
		}
		private const int item1Modifier = 5;
		private const int item2Modifier = 5;
		private const int motiveModifier = 10;
		private const int maxModifier = 20;
	
		public void Start()
		{
			mPlayerRef = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<MainCharacter>();
		}

        public static void ConfirmAccuse(string CharacterAccused, string item1, string item2, Motive motive)
        {
			Debug.Log("Confirming Accuse");
            if (CheckPlayerWin(CharacterAccused, item1, item2, motive))
            {
                Application.LoadLevel("Framed" + CharacterAccused);
            }
            else
            {
                Application.LoadLevel("FramedPlayer");
            }

            
        }

        private static bool CheckPlayerWin(string CharacterAccused, string item1, string item2, Motive motive)
        {
			bool accusedIsInList = false;
		
			if(mPlayerRef == null)
			{
				mPlayerRef = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<MainCharacter>();
			}
		
			foreach(NonPlayableCharacter character in Characters.Values)
			{
				if(character.CharacterSuspicion >= mPlayerRef.CharacterSuspicion - maxModifier && character.mCharacterName.ToString() == CharacterAccused)
				{
					Debug.Log(character.mCharacterName + " has more suspicion than the player");
					if(!charSuspicion.ContainsKey(character))
						charSuspicion.Add(character, character.CharacterSuspicion);
					if(!characters.ContainsKey(character.mCharacterName.ToString()))
						characters.Add(character.mCharacterName.ToString(), character.CharacterSuspicion);
				}
			}
		
			foreach(var scopedCharacters in charSuspicion)
			{
				if(scopedCharacters.Key.mCharacterName.ToString() == CharacterAccused)
				{
					accusedIsInList = true;
					Debug.Log(scopedCharacters.Key.mCharacterName + " is the person being framed");
					accusedCharacter = scopedCharacters.Key;
				}
			}
		
			if(!accusedIsInList)
				return false;
		
			var charComponent = GameManager.Instance.Characters[CharacterAccused];
		
			var npcComponent = charComponent as NonPlayableCharacter;
		
			if(npcComponent == null)
				return false;
		
			if(item1 == npcComponent.Item1.ToString() || item1 == npcComponent.Item2.ToString())
			{
				Debug.Log(accusedCharacter.mCharacterName + ": matched the first item, 5 points!");
				accusedCharacter.ModifySuspicion(item1Modifier);
			}
			else
			{
				mPlayerRef.ModifySuspicion(item1Modifier);
			}
			
		
			if(item2 == npcComponent.Item1.ToString() || item2 == npcComponent.Item2.ToString())
			{
				Debug.Log(accusedCharacter.mCharacterName + ": matched the second item, 5 points!");
				accusedCharacter.ModifySuspicion(item2Modifier);
			}
			else
			{
				mPlayerRef.ModifySuspicion(item2Modifier);
			}
		
			if(motive == npcComponent.mCharacterMotive)
			{
				Debug.Log(accusedCharacter.mCharacterName + ": matched the motive, 10 points!");
				accusedCharacter.ModifySuspicion(motiveModifier);
			}
			else
			{
				mPlayerRef.ModifySuspicion(motiveModifier);
			}
		
			if(mPlayerRef.CharacterSuspicion < accusedCharacter.CharacterSuspicion)
			{
				return true;
			}
			else
			{
				return false;
			}
        }
	}

