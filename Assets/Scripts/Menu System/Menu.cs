using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;


//*****************************************************************************
public class Menu : MonoBehaviour
{
	public bool mInTransition;
    public List<UIScreen> mScreenList;
    private UIScreen mActiveScreen;
    public List<string> mScreenListNames;
	public bool mGamePaused;
	private bool mISGAME = true;
	
	Camera 		m_refMenuCamera;
	GameObject m_refPlayer,
				m_refLight;
	CharacterMovement m_refCharacterMovement;
	MouseLook m_refMouseLook;
	Light m_menuLight;
	public GameObject Player
	{
		get { 
				
			if ( m_refPlayer == null )
			{
				m_refPlayer = GameObject.Find("PlayerCharacter");
				if ( m_refPlayer == null )
					mISGAME = false;
			}
			return m_refPlayer;
		}
	}
	public CharacterMovement Movement
	{
		get {
			if ( m_refCharacterMovement == null )
			{
				m_refCharacterMovement = Player.GetComponent<CharacterMovement>();
			}
			return m_refCharacterMovement;
		}
		
	}
	public MouseLook Cursor
	{
		get {
			if ( m_refMouseLook == null )
			{
				m_refMouseLook = Player.GetComponent<MouseLook>();
			}
			return m_refMouseLook;
		}
		
	}
    //******************************************************************
    void Start()
    {
		mInTransition = false;
        // Warn if there are no screens
        if (mScreenList.Count < 1)
        {
           //Debug.LogWarning("No screens in menu.", this.gameObject);
        }
		
		if ( ( m_refLight = this.gameObject.transform.FindChild("MouseLight").gameObject) == null )
		{
		}
		else if ( ( m_menuLight = (Light)m_refLight.GetComponent<Light>()) == null )
		{
			
		}
		else if ( ( m_refMenuCamera = this.gameObject.GetComponent<Camera>()) == null )
		{
			
		}

		
		if ( Player == null)
		{
			// shit
		}
		else if ( (m_refCharacterMovement = m_refPlayer.GetComponent<CharacterMovement>()) == null )
		{
			// also shit
		}
		else if ( (m_refMouseLook = m_refPlayer.GetComponent<MouseLook>()) == null )
		{
			
		}
		else
		{

			// finally ok
				//m_refCharacterMovement.Enabled = false; 
				//m_refMouseLook.enabled = false;
				//Screen.showCursor = true;
		}
		
        BasicMenuInput.CreateInput();
        ActivateFirstScreen();
        RegisterHandles();
        UpdateScreenRegistry();
    }

    public void UpdateScreenRegistry()
    {

        if (mScreenListNames == null)
        {
            mScreenListNames = new List<string>();
        }
        mScreenListNames.Clear();

        foreach (UIScreen screen in mScreenList)
        {
            mScreenListNames.Add(screen.name);
        }

    }
	
	/*if ( mPausesGame == true )
		{
			GameObject player;
			CharacterMovement movement;
			MouseLook look;
			if ( (player = GameObject.Find("PlayerCharacter")) != null)
			{

				if ( (movement = player.GetComponent<CharacterMovement>()) != null )
				{

					movement.Enabled = false; 
				}
				if ( (look = player.GetComponent<MouseLook>()) != null )
				{
					look.enabled = false;
					Screen.showCursor = true;
				}
			}
		}*/

    /// <summary>
    /// ActivateFirstScreen - Finds the first screen in the menu and sets it as active.
    /// </summary>
    private void ActivateFirstScreen()
    {
        SetActiveScreen(mScreenList[0]);

    }

    /*public void SetPauseState(bool isPaused, UIScreen active = null)
    {
        if (isPaused)
        {
            mGamePaused = true;

            if ( active != null)
            if (active.mModifyTime == true)
            {
                Time.timeScale = 0;
            }
            SwitchInput(false);

            Screen.showCursor = true;
        }
        else
        {
            mGamePaused = false;

            if (active != null)
            if (active.mModifyTime == true)
            {
                Time.timeScale = 1;

            }
            SwitchInput(true);

            Screen.showCursor = false;

        }

    }*/
    /// <summary>
    /// SetActiveScreen - Set a new active screen
    /// </summary>
    /// <param name="active">The new active screen.</param>
    /// <param name="setInactive">Marks whether the current screen should be marked as inactive.</param>
    public void SetActiveScreen(UIScreen active)
    {
        if (active != null)
        {
            if (mActiveScreen != null)
            {
                mActiveScreen.Active = false;
            }
            mActiveScreen = active;
			mActiveScreen.Active = true;
			mActiveScreen.OnScreenActive();
			
        }
		else
		{
			Debug.LogError("setting an active screen which doesn't exist");
		}
		
		//PCInput.Instance.Disabled = mGamePaused;

    }
	
	
	/*public void SwitchInput(bool on)
	{
		if ( mISGAME == true )
		{
			if ( on ) 
			{
               //Debug.Log("ENABELING");
				Movement.Enabled = true;
				Cursor.Enabled = true;
                Screen.lockCursor = true;
                //Movement.Reset();

			}
			else
			{
               //Debug.Log("DISABLING");
				Movement.Enabled = false;
				Cursor.Enabled = false;
                Screen.lockCursor = false;
                //Movement.Reset();

			}
		}
	}*/
	public void SetInactiveScreen(UIScreen inactive)
    {
        if (inactive != null)
        {
            inactive.Active = false;
            inactive.OnScreenInactive();
        }
		else
		{
			Debug.LogError("ERROR Menu.cs: Setting an active screen that is (null).");
		}
    }

    void RegisterHandles()
    {
        BasicMenuInput.Instance.OnMenuClickPressHandle += HandleClickPressed;
        BasicMenuInput.Instance.OnMenuClickReleaseHandle += HandleClickReleased;
        BasicMenuInput.Instance.OnMenuEscapePressHandle += HandleEscapePressed;
    }

    void HandleClickPressed()
    {

    }

    void HandleClickReleased()
    {

    }

    void HandleEscapePressed()
    {

    }

    //******************************************************************
    void Update()
    {
		Vector3 mouse = Input.mousePosition;
		
        BasicMenuInput.Instance.Update();
		 
		
		mouse.z = -5;
		m_menuLight.transform.position = (m_refMenuCamera.ScreenToWorldPoint(mouse));
		mouse.z = 10;
		m_menuLight.transform.LookAt(m_refMenuCamera.ScreenToWorldPoint(mouse));
		
			mInTransition = false;
			foreach(UIScreen screen in mScreenList)
			{
				foreach(UIMenuItem item in screen.mMenuItems )
				{
					
					if (item.GetTransitionFinished() == false)
					{
					
						mInTransition = true;
						return;
					}

				}

			}

    }

}
//**********************************************************************
