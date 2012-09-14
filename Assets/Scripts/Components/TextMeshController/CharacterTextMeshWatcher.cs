using UnityEngine;
using System.Collections;

public enum WatchVar
{
    Character,
    Item1,
    Item2,
    Motive,
}
public class CharacterTextMeshWatcher : MonoBehaviour
{
    public WatchVar accusationWatch;
    public string defaultText = " killed Gott in his room with the ", maleString, femaleString;
    TextMesh text;
    bool isMale;
	MeshRenderer renderer;
	UIMenuItem item;
	// Use this for initialization
	void Start () 
    {
		item = this.transform.parent.GetComponent<UIMenuItem>();
		renderer = this.GetComponent<MeshRenderer>();
		//Debug.Log("UI ITEM IS " + item);
        text = GetComponent<TextMesh>();
	}
	
	// Update is called once per frame
	void Update () 

    {
		renderer.enabled = item.ParentScreen.mActive;
		
        switch (AccusationResults.CurrentCharacterAccusing)
        {
            case CharacterName.Deborah:
            case CharacterName.Elizabeth:
            case CharacterName.Dorothy:
                isMale = false;
                break;
            default:
                isMale = true;
                break;
        }

        if(isMale)
        {
            defaultText = maleString;
        }
        else
        {
            defaultText = femaleString;
        } 
        switch(accusationWatch)
        {
            case WatchVar.Character:
                text.text = AccusationResults.CurrentCharacterAccusing + defaultText;
                break;
            case WatchVar.Item1:
                text.text = AccusationResults.CharacterItem1 + defaultText;

                break;
            case WatchVar.Item2:
                text.text = AccusationResults.CharacterItem2 + defaultText;

                break;
            case WatchVar.Motive:
                if(AccusationResults.CharacterMotive != null)
                    text.text = AccusationResults.CharacterMotive.DialogText + defaultText;

                break;
       }
	}
}
