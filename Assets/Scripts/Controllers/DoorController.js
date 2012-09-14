var locked:boolean = false;

var slamSound:AudioSource;
var lockedSound:AudioSource;

var freakOut:boolean = false;

function Start()
{
	if (locked) LockDoor();
}
function Update ()
{
	if (freakOut) FreakOut();
}

function FreakOut()
{
	/*
	if (locked)
	{
		UnlockDoor();
		hingeJoint.limits.min = (Random.Range(-60, 60));
		hingeJoint.limits.max = (Random.Range(-60, 60));
	}
	yield WaitForSeconds (Random.Range(3, 6));
	if (!locked) LockDoor();
	*/
	
	if (locked)
	{
		UnlockDoor();
		hingeJoint.limits.min = (Random.Range(-60, 60));
		hingeJoint.limits.max = (Random.Range(-60, 60));
	}
	if (slamSound.time > (.2 + Random.Range(.1, .5)) || !slamSound.isPlaying) LockDoor();
}

function LockDoor()
{
	locked = true;
	hingeJoint.limits.min = 0;
	hingeJoint.limits.max = 0;
	
	if (slamSound.time > (.2 + Random.Range(.1, .5)) || !slamSound.isPlaying) slamSound.Play();
}

function UnlockDoor()
{
	locked = false;
	hingeJoint.limits.min = -100;
	hingeJoint.limits.max = 100;
}

function OnCollisionEnter(collision:Collision)
{
	if (collision.gameObject.tag == "Player")
	{
		Debug.Log ("collision with door");
		if(lockedSound != null)
		lockedSound.Play();
	}
}