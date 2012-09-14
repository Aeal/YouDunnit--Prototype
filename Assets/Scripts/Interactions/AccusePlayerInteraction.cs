using UnityEngine;
using System.Collections;

public class AccusePlayerInteraction : Interaction 
{
    
    public override void OnTriggerActivated(object sender, System.EventArgs e)
    {
        var npcTrigger = sender as Trigger;
        var npcScript = npcTrigger.gameObject.GetComponent<NonPlayableCharacter>();
		
		if(GameManager.Instance.GetComponent<Reticle>().enabled = true)
			GameManager.Instance.GetComponent<Reticle>().enabled = false;
		Inputbase.Instance.Disabled = true;
		GameObject.FindGameObjectWithTag("MainCamera").GetComponent<MouseLook>().Enabled = false;
		GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CharacterMovement>().Enabled = false;
       //Debug.Log("Opening accusing menu for: " + npcScript.mCharacterName);
        AccusationResults.CurrentCharacterAccusing = npcScript.mCharacterName;
        AccusationResults.CharacterMotive = npcScript.mCharacterMotive;
       //Debug.Log("Current suspect: " + AccusationResults.CurrentCharacterAccusing);
        //OpenPrefab
        GameManager.Instance.OpenAccuseMenu();
        base.OnTriggerActivated(sender, e);
    }
}
