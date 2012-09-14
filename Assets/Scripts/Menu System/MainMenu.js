var showMoreGui = false;

function OnGUI()
 {

    if (GUI.Button (Rect (120,20,100,40), "Play"))
        showMoreGui = true;
    
    if (showMoreGui)
        MoreGUI ();
      
	if (GUI.Button (Rect (420,20,100,40), "Quit")) {
		Application.Quit();
	}          
}

function MoreGUI() 
{
    if (GUI.Button (Rect (70, 80,200,40), "I've never played before."))
    	Application.LoadLevel("SkippedScene");
    if (GUI.Button (Rect (70, 140,200,40), "I've played before."))
    	Application.LoadLevel("Mansionv2");
    if (GUI.Button (Rect (120, 200,100,20), "Back"))
        showMoreGui = false;
}

   



