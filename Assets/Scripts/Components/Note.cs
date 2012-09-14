using UnityEngine;
using System.Collections;

public class Note : MonoBehaviour 
{
	private const float mMaxDistance = 4.0f;
	private float mCurDistance = 0.0f;
	private bool bIsDisplayed;
    private bool hasPlayed;
	public string audioToPlay = "";
    public AudioClip Clip;
	private GameObject mPlayerRef;
	
	public bool OneShot = true,
                FadeMusic = true,
                SendFeedBack = true;
    public float SoundVolume = .5f,
                 MusicFadeVolume = .1f,
                 MusicFadeTime = .5f;
	
	public string mNoteText;
    public GUISkin MySkin;
    private AudioSource source;
    private Font defaultFont, diaryFont;
	void Start()
	{
		bIsDisplayed = false;
		Inputbase.Instance.OnActionButtonPressedHandle += OnClicked;
		mPlayerRef = GameObject.FindGameObjectWithTag("MainCamera");
	    source = gameObject.GetComponent<AudioSource>() ?? gameObject.AddComponent<AudioSource>();
        MySkin = Resources.Load("UserInterface/Conversation/ConversationUI") as GUISkin;
        diaryFont = Resources.Load("Fonts/Font-AngelinaSmall") as Font;
        defaultFont = MySkin.font;


	}
	
	void OnClicked(object sender, ClickedEventArgs e)
	{
		if(mCurDistance > mMaxDistance)
			return;
		if( e.TargetObject == null)
			return;
        if(bIsDisplayed)source.Stop();
		bIsDisplayed = !bIsDisplayed;
	    hasPlayed = false;
	}

	void Update()
	{
		mCurDistance = Vector3.Distance(MainCharacter.Instance.transform.position, gameObject.transform.position);
		
		if(bIsDisplayed)
		{
			mPlayerRef.GetComponent<MouseLook>().enabled = false;
			if(audioToPlay != "")
			{
				Debug.Log("Playing: " + audioToPlay);
            	//SoundManager.Play2DSound(audioToPlay, volume:SoundVolume, loop:false, fadeMusic:FadeMusic, fadeTo:MusicFadeVolume, fadeTime: 1.0f);
                if (!hasPlayed )
                {
                    hasPlayed = true;
                    source.clip = Clip;
                    source.Play();
                }
			}
		}
		else
			mPlayerRef.GetComponent<MouseLook>().enabled = true;
	}
	
	void OnGUI()
	{
        GUI.skin = MySkin;
        GUI.skin.font = diaryFont;
		if(bIsDisplayed)
		{
			GUI.skin.box.wordWrap = true;
			GUI.Box(new Rect(Screen.width*.25f, Screen.height*.25f, Screen.width*.5f,Screen.height*.5f), mNoteText);
            
            //if(GUI.Button(new Rect(Screen.width), ))
		}
        GUI.skin.font = defaultFont;
	}
	
	void OnDestroy()
    {
        Inputbase.Instance.OnActionButtonPressedHandle -= OnClicked;
    }
}

