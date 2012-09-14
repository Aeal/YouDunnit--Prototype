using UnityEngine;
using System.Collections;

public class PlayAnimationClip : MonoBehaviour
{
    public string animationName = "";

	// Use this for initialization
	void Start ()
	{
        animation.Play(animationName);
        animation.clip.wrapMode = WrapMode.Loop;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
