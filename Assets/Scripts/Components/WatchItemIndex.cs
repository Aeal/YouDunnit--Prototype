using UnityEngine;
using System.Text;
using System.Collections;
using System.Collections.Generic;
public class WatchItemIndex : MonoBehaviour 
{
    public static List<CharacterItem> ItemsInPlay = null;
    public int IndexToWatch = 0;
    private TextMesh meshToControl;

	// Use this for initialization
	void Start () 
    {
        meshToControl = GetComponent<TextMesh>();
        if (meshToControl == null)
        {
            Debug.LogError("There was an error finding the text mesh");
        }
        var i = 0;
		

        //if (ItemsInPlay == null)
        //{
            //Debug.Log("Initilizing Items in Play");
            ItemsInPlay = new List<CharacterItem>();
            //Populate it and make it static so we only call findSceneObjects of type once.
            foreach (NonPlayableCharacter item in GameObject.FindSceneObjectsOfType(typeof(NonPlayableCharacter)))
            {
                //Debug.Log("Adding character item: " + item.Item1);
                ItemsInPlay.Add(item.Item1);
                //Debug.Log("Adding character item: " + item.Item2);
                ItemsInPlay.Add(item.Item2);
            }

        //}

        meshToControl.text = AddSpacesToItem(ItemsInPlay[IndexToWatch].ToString());  
        
	
	}
	
	string AddSpacesToItem(string text)
	{
        if (string.IsNullOrEmpty(text))
           return "";
        StringBuilder newText = new StringBuilder(text.Length * 2);
        newText.Append(text[0]);
        for (int i = 1; i < text.Length; i++)
        {
            if (char.IsUpper(text[i]) && text[i - 1] != ' ')
                newText.Append(' ');
            newText.Append(text[i]);
        }
        return newText.ToString();
	}
	
	// Update is called once per frame
	void Update () 
    {
        if (meshToControl == null) return;
        
	    
	}
}
