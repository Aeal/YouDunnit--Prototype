using UnityEngine;
using System.Collections;

public class WatchMotiveIndex : MonoBehaviour
{

    public int IndexToWatch = 0;
    private TextMesh meshToControl;

    // Use this for initialization
    void Start()
    {
        meshToControl = GetComponent<TextMesh>();
        if (meshToControl == null)
        {
           //Debug.LogError("There was an error finding the text mesh");
        }
	
	}
	
	// Update is called once per frame
	void Update () 
    {
        meshToControl.text = MotiveManager.Instance.Motives[AccusationResults.CurrentCharacterAccusing][IndexToWatch].DialogText;
	}
}
