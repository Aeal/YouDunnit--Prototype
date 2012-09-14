using UnityEngine;
using System.Collections;

public class TeleportOnContact : MonoBehaviour 
{
	public GameObject connection;
	public GameObject connectingEntrance;
	private bool isTeleporting;
	private float transitionTime;
	public float transitionLength;
	public float alphaFadeValue;
	public Texture blackTexture;
	// Use this for initialization
	void Start () 
	{
		transitionTime = 0.0f;
		isTeleporting = false;
		transitionLength = 5.0f;
		alphaFadeValue = 0.0f;
	}
	
	// Update is called once per frame
	void Update () 
	{
		if( isTeleporting )
		{
			transitionTime += Time.deltaTime;
			if(transitionTime < 5.0f)
			{
				isTeleporting = false;
				transitionTime = 0.0f;
			}
		}
	
	}
	void OnTriggerEnter(Collider other)
	{
		if(other.tag == "MainCamera")
		{
			Debug.Log("hit");
			//FadeToBlack black = new FadeToBlack();
			//Timer time = new Timer();
			//time.StartTimer();
			///Time.timeScale = 0;
			isTeleporting = true;
			Vector3 playerPos = other.gameObject.transform.position;
			playerPos = connection.gameObject.transform.position;
			other.gameObject.transform.position = playerPos;
			// figure out where the player should face
			Vector3 ExitPos = connection.gameObject.transform.localPosition;
			other.transform.LookAt(playerPos + ExitPos);
		}
	}
	

	
	
}