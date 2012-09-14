using UnityEngine;
using System.Collections;

public class NoteManager : MonoBehaviour {


    [HideInInspector]
    private string entireNote;
    private string displayNote = "\t\t\t\t\tNotes";
    private int lastInd = 0;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    Rect tots = new Rect(10, 10, 200, 150);

    void OnGUI()
    {
        // Make a background box
       // GUI.Box(tots, displayNote);
        GUI.TextArea(tots, displayNote);

        //// Make the first button. If it is pressed, Application.Loadlevel (1) will be executed
        //if (GUI.Button(Rect(20, 40, 80, 20), "Level 1"))
        //{
        //    Application.LoadLevel(1);
        //}

        //// Make the second button.
        //if (GUI.Button(Rect(20, 70, 80, 20), "Level 2"))
        //{
        //    Application.LoadLevel(2);
        //}
    }

    public void setNote(string passedIn)
    {
        entireNote = passedIn;

        if (passedIn.Length > 135)//70 is arbitrary num for size of box
        {
            //start at the passed in string at 70, then work backwards looking for a   ".", " ", "!"
            int tmpcount = 135;
            bool run = true;
            while (run)
            {
                if (passedIn[tmpcount] == ' ' || passedIn[tmpcount] == '.' || passedIn[tmpcount] == '!' || passedIn[tmpcount] == '?')
                {
                    run = false;
                }

                tmpcount--;
            }
            Debug.Log(tmpcount);
            int anotherTmp = 0;
            displayNote = "\t\t\t\t\tNotes\n\n";
            while (anotherTmp != tmpcount)
            {
                displayNote += passedIn[anotherTmp];
                anotherTmp++;
            }
            displayNote += passedIn[anotherTmp];
        }
        else
        {
            displayNote = "\t\t\t\t\tNotes\n\n";
            displayNote += passedIn;
        }
   
    }
}
