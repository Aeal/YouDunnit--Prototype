/*using System.Collections;
using System;
using System.Text;
using System.IO;

bool triggered;

void OnTriggerEnter( Collider other)
{
	if (other.gameObject.tag == "MainCamera")
       {
       triggered = true;
       }
}
void OnGui()
{
    if (triggered) //The dialogue starts.
    {
  	  GUI.Label(new Rect(300,300,500,200), String);
    }
}
void OnTriggerExit( Collider other)
{
     if (other.gameObject.tag == "Player")
     {
     //The dialogue ends.
     triggered = false;
     }
}*/