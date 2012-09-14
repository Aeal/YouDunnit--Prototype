using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class VolumeTrigger : Trigger
{
    BoxCollider volume;
	public bool IgnoreTag = false;
	public string TagToIgnore = "MainCharacter";
    void Start()
    {
        base.Start();
       //Debug.Log("Starting Volume Trigger");
        gameObject.GetComponent<BoxCollider>().isTrigger = true; ;
    }

    void OnTriggerEnter(Collider other)
    {
		if(IgnoreTag && TagToIgnore == other.tag) return;
        Activated();
    }

}

