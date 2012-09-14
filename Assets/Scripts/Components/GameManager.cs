using System;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class GameManager : MonoBehaviour
{
    /// <summary>
    /// Class Name: GameManager
    /// Author: TC_FALLOWS
    /// Last Edit: TSTEELE 1/27/2012
    /// Purpose: The manager for the actual game. Maintains the current game state, switches between states and starts off the initial one.
    /// </summary>
    							// List of all NPC's in the current level
    private static GameManager instance;
    public static GameManager Instance { get { return instance; } }
    

    private Dictionary<string, Character> characters = new Dictionary<string, Character>();
    public Dictionary<string, Character> Characters { get { return characters; } }

    private Dictionary<string, Item> items = new Dictionary<string, Item>();
    public Dictionary<string, Item> Items { get { return items; } }

    private Dictionary<string, Motive> motives = new Dictionary<string, Motive>();
    public Dictionary<string, Motive> Motives { get { return motives; } }

    public delegate void OnGamePausedHandler(object sender, EventArgs e);
    public delegate void OnGameResumeHandler(object sender, EventArgs e);

    public static event OnGamePausedHandler OnGamePaused;
    public static event OnGameResumeHandler OnGameResume;

    public delegate void OnGameEndingHandler(object sender, EventArgs e);
    public delegate void OnGameCancelEndHandler(object sender, EventArgs e);

    public static event OnGameEndingHandler OnGameEnding;
    public static event OnGameCancelEndHandler OnGameCancelEnd;

    private bool isGamePaused;

    private int mFPS = 0;									// Current FPS
    private float mUpdateInterval = 0.5f;						// Update interval
    private float mAccum = 0.0f; 								// FPS accumulated over the interval
    private float mTimeleft = 0.0f; 							// Left time for current interval+
    public float MAX_GAME_TIME = 15;                         // How long will the game go for before end state is called automatically?
    //public UIScreen screenToOpenOnAccuse;

    public void OpenAccuseMenu()
    {
        ActionBase[] screensToOpen = gameObject.GetComponents<ActionOpenScreen>();
        for (int i = 0; i < screensToOpen.Length; i++)
        {
            screensToOpen[i].ExternalAction();
        }
       // var menuOpener = gameObject.GetComponent<ActionOpenScreen>();
		var disableInput= gameObject.GetComponent<ActionToggleInput>();
		//disableInput.inputActive = false;
		//disableInput.pauseGameTime = true;
       // menuOpener.mScreenToOpen = screenToOpenOnAccuse;
        //menuOpener.ExternalAction();
		disableInput.ExternalAction();
    }
//    public int MAX_STRIKE_AMOUNT = 3;
//    private int mStrikeAmount;                      // How many strikes does the character currently have?
//    public int CurrentNumStrikes
//    {
//        get { return mStrikeAmount; }
//    }
//    private const string mStrikeUp = "StrikeUp";

    private bool mIsAccusing;
    //private ActionOpenScreen accuseScreen;
    private ActionBase[] accuseActions;

	public void DestroySingleton()
	{
		instance = null;
		Destroy(gameObject);
		Destroy(this);
	}
    //******************************************************************
    void Update()
    {
        if (instance == null)
        {
            instance = this;
        }
       
        Inputbase.Instance.Update();
		
		if(Input.GetKeyDown(KeyCode.K))
	    {
			DestroySingleton();
			Application.LoadLevel("Main_Menu_00");
		}
		
        if(Input.GetKeyDown(KeyCode.Space))
        {
            if (!isGamePaused)
            {
                if (OnGamePaused != null)
                    OnGamePaused(this, EventArgs.Empty);
                isGamePaused = true;
            }
            else
            {
                if (OnGameResume != null)
                    OnGameResume(this, EventArgs.Empty);
                isGamePaused = false;
            }
        }
		
    }
    //******************************************************************
    void Start()
    {
        //Debug.Log("Starting Game");
        if (instance == null) instance = this;
        else Destroy(this);
		

        mTimeleft = mUpdateInterval; 
		Time.timeScale = 1;
		
        //Debug.Log("Clearing Nav Nodes");
        NavigationNode.Clear();
		
		//Debug.Log("Clearing Items");
        ItemSpawnPoint.Clear();
		
		//Debug.Log("Clearing ID Tables");
        //HashID.Clear();

        //Debug.Log("Spawning NPCs");
       	NPCSpawner.Initilize();
		
		//Debug.Log("Starting Motive Manager");
        MotiveManager.Instance.Init();
		
		//Debug.Log("Creating Input");
        Inputbase.CreateInput<PCInput>();
		
		//Debug.Log("Creating Inventory");
        Inventory inventory = gameObject.AddComponent<Inventory>();
		//Debug.Log("Initilizing Inventory");
        inventory.Init();
		//Debug.Log("Creating SoundManger");
        gameObject.AddComponent<SoundManager>();

        //Debug.Log("Fetching Accuse Screen");
         accuseActions = gameObject.GetComponents<ActionBase>();
        if (accuseActions.Length >= 0)
        {
            //Debug.LogError("There was no OpenScreenAction");
        }
		// SoundManager.Instance.LoadSounds(Directory:"Diary");

       Conversation.OnConversationStarted += ShowCursor;
       Conversation.OnConversationEnded += HideCursor;
        //HideCursor();
        Screen.showCursor = false;
        Screen.lockCursor = true;
    }

    private void ShowCursor(object sender = null, EventArgs e = null)
    {
        //Debug.Log("Showing Cursor FDSKJFJKSDKLJFJSDKLFKLSDKLFSDKLJKSDFKLSDFFSDFSDFSDFSDf");
        Screen.showCursor = true;
    }

    private void HideCursor(object sender = null, EventArgs e = null)
    {
        //Debug.Log("Hiding Cursor");

        Screen.showCursor = false;
    }

    public void RegisterCharacter(string NPCName, Character charRef)
    {
        if (!characters.ContainsKey(NPCName)) characters.Add(NPCName, charRef);
    }

    public void RemoveCharacter(string name)
    {
        if (characters.ContainsKey(name)) characters.Remove(name);
    }

    public void RegisterMotive(string NPCName, Motive motive)
    {
        if (!motives.ContainsKey(NPCName)) motives.Add(NPCName, motive);
    }

    public void RemoveMotive(string name)
    {
        if (motives.ContainsKey(name)) motives.Remove(name);
    }

    public void RegisterItem(string Itemname, Item item)
    {
        if (!items.ContainsKey(Itemname)) items.Add(Itemname, item);
    }

    public void RemoveItem(string name)
    {
		
        if (items.ContainsKey(name)) 
		{	
			if(name == "MurderKnife")
			{
				//Debug.Log("load level");
			}
			if(name == "Knife")
			{
				//Debug.Log("load level");
			}
			items.Remove(name);
		}
    }

    public void SpawnItem(CharacterItem ItemToSpawn)
    {
        //Get random spawn point
		if(ItemToSpawn != null)
		{
	        //Debug.Log("Spawning Item: " + ItemToSpawn);
	        ItemSpawnPoint point;
			
			if(ItemSpawnPoint.PossibleSpawns.Count > 1)
			{
		        int index = new Random().Next(0, ItemSpawnPoint.PossibleSpawns.Count - 1);
		        point = ItemSpawnPoint.PossibleSpawns[index];
		        point.Initilize(ItemToSpawn);
			}
		}
    }
	
	  public void SpawnItem(GameObject objectToSpawn)
    {
        //Get random spawn point
          //Debug.Log(objectToSpawn.name + " requested respawn");
        ItemSpawnPoint point;
        int index = new Random().Next(0, ItemSpawnPoint.PossibleSpawns.Count - 1);
        point = ItemSpawnPoint.PossibleSpawns[index];
        point.Spawn(objectToSpawn);
    }

    public static void Accuse()
    {
        //if(mIsAccusing)return;
        ////gameObject.AddComponent<Endgame>();
        //mIsAccusing = true;
        ////MainCharacter.Instance.gameObject.GetComponent<MouseLook>().Enabled = false;
        //foreach (UIMenuAction action in accuseActions)
        //{
        //    action.ExternalAction();
        //}
        //if(OnGameEnding != null)
        //{
        //    OnGameEnding(this, EventArgs.Empty);
        //}

        //TODO Make the the game store items, and suspicion and everything it needs here to be pushe 
        //over to the accuse screen.
        MansionResults.Characters = instance.characters;
        instance.DestroySingleton();
        Application.LoadLevel("AccuseScene");
    }

    public void CancelAccuse()
    {
        if(!mIsAccusing)return;
		
		//Debug.Log ("Cancelling Accuse");
        //Destroy(gameObject.GetComponent<Endgame>());
		Time.timeScale = 1;
        mIsAccusing = false;
		GameManager.Instance.gameObject.GetComponent<Reticle>().enabled = true;
		MainCharacter.Instance.gameObject.GetComponent<MouseLook>().Enabled = true;
		
        if(OnGameCancelEnd != null)
        {
            OnGameCancelEnd(this, EventArgs.Empty);
        }
    }


    /*public void ConfirmAccuse(string CharacterAccused, string item1, string item2, Motive motive)
    {
        if (CheckPlayerWin(CharacterAccused, item1, item2, motive))
        {
            Application.LoadLevel("Outro_" + CharacterAccused);
        }
        else
        {
            Application.LoadLevel("Outro_Player");
        }

        Destroy(this);
    }*/

    /*private bool CheckPlayerWin(string CharacterAccused, string item1, string item2, Motive motive)
    {
        NonPlayableCharacter looser = null;
        foreach (NonPlayableCharacter character in Characters.Values)
        {
            if (looser == null) looser = character;
            if (character.CharacterSuspicion > looser.CharacterSuspicion)
                looser = character;
        }
       //Debug.Log(looser.mCharacterName + " looses");
        if (CharacterAccused == looser.mCharacterName.ToString())
        {
            if (item1 == looser.Item1.ToString() || item1 == looser.Item2.ToString())
            {
                if (item2 == looser.Item1.ToString() || item2 == looser.Item2.ToString())
                {
                    if (motive == looser.mCharacterMotive)
                    {
                       //Debug.Log("Player wins");
                        return true;
                    }
                    return false;
                }
                return false;
            }
            return false;
        }
        return false;
    }*/

    /*private void OnGUI()
    {
        
    }*/

    public void AddSuspicionToCharacter(Character name, float amount)
    {
    }


    void OnDestroy()
    {
        DestroySingleton();
    }
}
