
var footstepDelay:int = 50;

var currentDelay:int = 0;

var audioSources:AudioSource[];

var rugAudio:AudioClip[];

static var onWood:boolean = false;

function Start()
{
	onWood = true;
}

function Update ()
{
	if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.D))
	{
		currentDelay = 0;
	}
	
	if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
	{
		// Play footstep sound
		if (currentDelay == 0)
		{
			if (onWood) audioSources[Random.Range(0, audioSources.length)].Play();
			else audioSources[0].PlayOneShot(rugAudio[Random.Range(0, rugAudio.length)]);
		}
		if (currentDelay < footstepDelay)
		{
			currentDelay ++;
		}
		if (currentDelay == footstepDelay)
		{
			currentDelay = 0;
		}
	}
	
	
}