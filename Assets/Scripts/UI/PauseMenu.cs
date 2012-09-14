using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class PauseMenu : MonoBehaviour
{
    public bool mMenuEnabled = false,
                mConvoEnabled = false;
	public GUIStyle mStyle;
	
	void Start()
	{	
		mStyle.alignment = TextAnchor.MiddleCenter;
		mStyle.fontStyle = FontStyle.BoldAndItalic;
		mStyle.fontSize = 32;
		mStyle.normal.textColor = Color.white;

		
	}
	void Update ()
	{
        bool doLock = true,
             doShow = false;
        MouseLook[] looks = this.gameObject.GetComponentsInChildren<MouseLook>();
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			if (mMenuEnabled == true)
			{
				mMenuEnabled = false;
			}
			else
			{
				mMenuEnabled = true;
			}
			
		}

        doLock = true;
        doShow = false;

        for (int i = 0; i < looks.Length; i++)
        {
            looks[i].enabled = true;
        }
        //Time.timeScale = 1;

		if (mMenuEnabled )
		{
            doLock = false;
            doShow = true;
           
            for (int i = 0; i < looks.Length; i++)
            {
                looks[i].enabled = false;
            }
			//Time.timeScale = 0;
		}

        if (mConvoEnabled)
        {
            doLock = false;
            doShow = true;

            for (int i = 0; i < looks.Length; i++)
            {
                looks[i].enabled = false;
            }

        }


        //Screen.lockCursor = doLock;
        //Screen.showCursor = doShow;




	}
		
	void OnGUI()
	{

		if (mMenuEnabled)
		{
			GUI.Label(new Rect(Screen.width / 2 , Screen.height / 2, 100, 50), "Pause", mStyle);
		}
	}
}