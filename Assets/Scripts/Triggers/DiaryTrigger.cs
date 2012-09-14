using UnityEngine;


public class DiaryTrigger : MonoBehaviour
{

	public string 	MenuPrefabName = "Editor",
	                NotificationPrefabName = "Diary",
                    Title = "Diary Title here",
                    Message = "Note to display";
    public int      MaxCharacterWidth = 55;
    public bool TriggerOnce = false,
                Interactive = false;
                    

    public AudioClip SoundToPlay;
    public bool     FadeMusic = true;
    public float    SoundVolume = .5f,
                 	MusicFadeVolume = .1f,
                 	MusicFadeTime = .5f;
    
    GameObject  m_targetUIObject,		// The target object in the menu system for tutorials
                m_menuSystemObject,		// Object the menu system component is on
                m_textObject,           // Object the text mesh component is on
                m_titleObject,
                m_interactiveObject;
	TextMesh	m_textMeshRef,
                m_titleMeshRef;			// Text mesh component
	Menu  		m_menuSystemRef;		// Menu system 
	UIMenuItem 	m_menuItemRef;			// Menu item reference
    AudioSource 	m_AudioSource;

	bool	m_hasTriggered = false,		// Has this object ever triggered once during playthrough?
			m_initalized = false,		// Has this object been properly initialized?
			m_isTriggered = false;		// Is this currently triggered?

	// Call this to verify and attach assets needed for notication trigger
	bool FindAttachAssets()
	{
        bool ret = false;

		if ( (m_targetUIObject == null) ||
		     (m_menuSystemObject == null) )
		{
			Debug.LogWarning("Diary Trigger object not set - looking it up manually.");
			
			if ( (m_menuSystemObject = GameObject.Find(MenuPrefabName)) == null)
			{
				Debug.LogError("Couldn't find a menu object named("+ MenuPrefabName +")");
			}
			else if ( (m_targetUIObject = GameObject.Find(NotificationPrefabName)) == null)
			{
				Debug.LogError("Couldn't find notification object("+ NotificationPrefabName +")");
			}
			else if ( ( m_textObject = m_targetUIObject.transform.FindChild("TextMesh").gameObject) == null)
			{
				Debug.LogError("Couldn't find a text object on ("+ NotificationPrefabName +")");
			}
            else if ((m_titleObject = m_targetUIObject.transform.FindChild("TitleMesh").gameObject) == null)
            {
               //Debug.LogError("Couldn't find a text object on (" + NotificationPrefabName + ")");
            }
			else if ( (m_menuSystemRef = m_menuSystemObject.GetComponent<Menu>()) == null )
			{
				Debug.LogError("Couldn't find a menu system named("+ MenuPrefabName + ") on " + NotificationPrefabName);
			}
			else if ( ( m_textMeshRef = m_textObject.GetComponent<TextMesh>()) == null )
			{
				Debug.LogError("Couldn't find a TextMesh on("+ NotificationPrefabName +")");
			}
            else if ((m_titleMeshRef = m_titleObject.GetComponent<TextMesh>()) == null)
            {
               //Debug.LogError("Couldn't find a TextMesh on(" + NotificationPrefabName + ")");
            }
			else // Got a valid menu system object and valid notification object
			{

					// Find the notification element
                foreach (UIScreen screen in m_menuSystemRef.mScreenList)
                {
                    for (int i = 0; i < screen.mMenuItems.Count; i++)
                    {
                        if (screen.mMenuItems[i].name == NotificationPrefabName)
                        {
                            m_menuItemRef = screen.mMenuItems[i];
                            break;
                        }
                    }
                }
					
					if ( m_menuItemRef == null )
					{
						Debug.LogError("Couldn't find custom notication object " + NotificationPrefabName);
					}
					else
                    {
                        ret = true;
					}
			}
		}

        return (ret);
	}
	
    // Use this for initialization
    void Start()
    {
        if ( (m_initalized = FindAttachAssets()) == false )
		{
           //Debug.LogError("Diary trigger " + gameObject.name + " was not initialized.");
			//collider.isTrigger = false;
			//collider.enabled = false;
		}
		else
		{
			//collider.isTrigger = true;
            
            FormatString();
			if (Interactive)
			{
                Inputbase.Instance.OnActionButtonPressedHandle += DoClick;
            	m_interactiveObject = this.gameObject;
			}
			
			this.GetComponent<ActionOpenScreen>().mScreenToOpen = m_menuItemRef.ParentScreen;
		}

		if ( SoundToPlay != null )
		{
			m_AudioSource = gameObject.AddComponent<AudioSource>();
		}

	}

    void FormatString()
    {
        if (MaxCharacterWidth < 10)
        {
            MaxCharacterWidth = 10;
           //Debug.Log("Dont make the max width so small dude.");
        }

        string[] myTempString = Message.Split(' ');
        string tempMessage = "";
        int tempLength = 0;
        foreach (string str in myTempString)
        {
            if ((tempLength + str.Length +1) > MaxCharacterWidth)
            {
                tempMessage = tempMessage + "\n"+ str + " ";
                tempLength = str.Length + 1;
            }
            else
            {
                tempLength += str.Length + 1;
                tempMessage = tempMessage + str + " ";
                //Debug.Log(tempLength);
            }
        }
        Message = tempMessage;
        /*for (int i = 1; i < Message.Length; i++)
        {
            if ((currentCount % MaxCharacterWidth) == 0)
            {
                // now rewind to space
                //i = Message.LastIndexOf(' ', lastEndIndex,35);
                for (int l = i; l > 0; l--)
                {
                    if (Message[l] == ' ')
                    {
                        Message = Message.Insert(i, "\n");
                        i = l + 1;
                        
                    }
                }
                currentCount = 0;
                
            }
            else
            {
                currentCount++;
            }
            

        }*/


      // //Debug.Log(tempMessage);
    }
    
	/*void OnClicked(object sender, ClickedEventArgs e)
	{
		isTriggered = !isTriggered;
		//mPlayerRef.GetComponent<CharacterController>().enabled = true;
		//mPlayerRef.GetComponent<MouseLook>().enabled = true;
		
		//GameObject.Destroy(gameObject.GetComponent("TutorialTrigger"));
	}*/
    // ************************************************************************
    public void DoAllActions()
    {
        // Execute all the attached UIMenuActions
        ActionBase[] actions = this.gameObject.GetComponents<ActionBase>();
        for (int i = 0; i < actions.Length; i++)
        {
           //Debug.Log("Executing Action: " + actions[i].name);
            actions[i].ExternalAction();
        }
    }
    void DoClick(object sender, ClickedEventArgs e)
    {

        if (e.TargetObject != m_interactiveObject || m_interactiveObject == null || m_isTriggered)
        {
            return;
        }
        else
        {
            DoTutorialAction(sender, e);
        }
    }
    // ************************************************************************
    void DoTutorialAction(object sender, ClickedEventArgs e)
    {

        if (m_isTriggered == true)
        {


        }
        else
        {
            if (SoundToPlay != null)
            {
				m_AudioSource.Stop();
                m_AudioSource.PlayOneShot(SoundToPlay);
            }
        }

		

		if ( Interactive )
		{
        	if ( e.TargetObject == null )
            {

            }
            else if (e.TargetObject != m_interactiveObject)
            {

            }
            else if (e.TargetObject == m_interactiveObject)
            {
                if (m_menuItemRef.IsTransitionFinished == true)//@@@ TITO MAKE SURE THESE ARE RIGHT ~ love tito
                {
                    DoAllActions();
                    m_textMeshRef.text = Message;
                    m_titleMeshRef.text = Title;

                    m_isTriggered = true;	// This is now in triggered state
                    m_hasTriggered = true;	// This has been triggered at least once
                }
            }


		}
    }
    // ************************************************************************
    void OnTriggerEnter(Collider collider)
    {
		if (m_initalized == true )
		{
			// Only detect the player
	        if (collider.gameObject.tag == "MainCamera")
	        {
                // This hasn't been triggered yet
				if (!m_isTriggered)
				{

                    // "This is only supposed to trigger once", and "this trigger already happened"
					if ( TriggerOnce && m_hasTriggered)
					{
						return;
					}

                    // Talking to "this tutorial trigger", sending "this interactive object"
                   //Debug.Log(m_interactiveObject);
                    DoTutorialAction(this, new ClickedEventArgs(m_interactiveObject));
				}
	        }
		}
    }
    // ************************************************************************
    void Update()
    {
        if (m_initalized)
        {
           if ( Interactive )
           {
                
               if (m_menuItemRef.IsTransitionFinished == true)
               {
                   m_isTriggered = false;
               }
           }
           
        }
		/*if(!isTriggered)
			return;
		else
		{
			mPlayerRef.GetComponent<CharacterController>().enabled = false;
		}*/
    }

    /*void OnGUI()
    {
        if (isTriggered && gameObject.guiTexture != null)
        {
            GUI.depth = 0;
            GUI.DrawTexture(new Rect(Screen.width / 4, Screen.height / 4, gameObject.guiTexture.texture.width,
                         gameObject.guiTexture.texture.height), gameObject.guiTexture.texture);
        }
    }*/
	
	/*void OnDestroy()
    {
        Inputbase.Instance.OnActionButtonPressedHandle -= OnClicked;
    }*/
}
