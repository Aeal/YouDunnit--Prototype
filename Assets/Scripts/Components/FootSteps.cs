using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class FootSteps : MonoBehaviour
{
    int index = 0;
    public List<AudioClip> FootStepSounds  = new List<AudioClip>();
    public CharacterMovement movement;
    private AudioSource source;
	// Use this for initialization
	void Start ()
    {
        source = gameObject.AddComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () 
    {
        if (movement.IsMoving)
        {
            if (!source.isPlaying)
            {
                //Debug.Log("Playing foot step");
                source.clip = FootStepSounds[index];
                source.Play();
                index++;
                if (index >= FootStepSounds.Count)
                {
                    index = 0;
                }
            }
        }
	}
}
