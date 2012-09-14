var targetObject:GameObject;

var triggerOnce:boolean;



private var triggered:boolean;

function OnTriggerEnter(collider:Collider)
{
	if (collider.gameObject.tag == "Player" && !triggered)
	{
		if (targetObject == null) audio.Play();
		else 
        {
            targetObject.audio.Play();
            targetObject.guiTexture.active = true;
        }
		if (triggerOnce) triggered = true;
		
		//Debug.Log("Sound Played");
	}
}