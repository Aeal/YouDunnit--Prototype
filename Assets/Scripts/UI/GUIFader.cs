using UnityEngine;
using System.Collections;

public class GUIFader : MonoBehaviour
{


    public float FadeTime = 5.0f;
    public Texture2D TextureMask;
    public Texture2D RealTexture;
    public bool IsFading;
    public Timer timer;
    public float MaxVolume = .75f;
    public bool FadingIn;
    public bool FadeOnStart;
    public bool FadeMusic, gotToScene = true;
    public AudioSource sourceToFade;

    private bool reset;
    public bool Reset
    {
        get { return reset; }
        set { reset = value; }
    }
    bool ending = false;
	bool starting = false;

    private string GoToSceneOnFinish;
    // Use this for initialization
    void Start()
    {
        timer = new Timer();
        timer.Length = FadeTime;
        if (FadeOnStart)
        {
            StartFadeIn();
            ////print("start");
        }


    }

    // Update is called once per frame
    void Update()
    {
        timer.Update();
        ////print(timer.isRunning);
    }

    void OnGUI()
    {

        LateOnGUI();
        if (ending || starting)
        {
            //print("LOL");
            GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), TextureMask);

        }

    }
    public void LateOnGUI()
    {


        if (IsFading)
        {
            if (FadingIn)
            {
                ////print(timer.precentComplete);
                ////print(timer.ElapsedTime);


                GUI.color = new Color(GUI.color.r, GUI.color.g, GUI.color.b, 1.0f - timer.percentComplete);
                GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), TextureMask);
                if (FadeMusic)
                {
                    sourceToFade.volume = (timer.percentComplete * MaxVolume);
                }
                if (timer.percentComplete >= 1.0f)
                {
                    IsFading = false;
                    FadingIn = false;

                }

            }
            else if (!FadingIn)
            {
                GUI.color = new Color(GUI.color.r, GUI.color.g, GUI.color.b, 1.0f * timer.percentComplete);
                GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), TextureMask);
                
                if (FadeMusic)
                {
                    sourceToFade.volume = (MaxVolume - (timer.percentComplete * MaxVolume));
                }
                //print("FAdingout");               
                if (timer.percentComplete >= 1.0f)
                {
                    IsFading = false;
                    
                    GUI.color = new Color(GUI.color.r, GUI.color.g, GUI.color.b, 1.0f);
                    GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), TextureMask);


                    if (!ending && gotToScene)
                    {
                        Application.LoadLevel(this.GoToSceneOnFinish);
                        ending = true;
                    }




                }



            }


        }
    }

    public void StartFadeIn()
    {
        //timer.ResetTimer();
        timer.StartTimer();
        // timer.isRunning = true;

        ////print("Started");
        FadingIn = true;
        IsFading = true;
    }
    public void StartFadeIn(string GoToScene)
    {
        //timer.ResetTimer();
        timer.StartTimer();
        // timer.isRunning = true;

        ////print("Started");
        FadingIn = true;
        IsFading = true;
        GoToSceneOnFinish = GoToScene;
    }

    public void StartFadeOut()
    {
        timer.StartTimer();
        FadingIn = false;
        IsFading = true;
    }

    public void StartFadeOut(string GoToScene)
    {
       //Debug.Log("Fading out and going to scene: " + GoToScene);
        timer.StartTimer();
        FadingIn = false;
        IsFading = true;
        GoToSceneOnFinish = GoToScene;

    }

}