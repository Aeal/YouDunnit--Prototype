using UnityEngine;
using System.Collections;

public class PCInput : Inputbase
{
    public KeyCode MoveUp    = KeyCode.W,
                   MoveDown  = KeyCode.S,
                   MoveLeft  = KeyCode.A,
                   MoveRight = KeyCode.D;

   
    public override void Update()
    {
		if ( Disabled )
		{
			return;
		}

        if(Input.GetMouseButtonDown(0))     OnActionButtonPressed();


        if (Input.GetKeyDown(MoveUp))       OnMoveUpPressed();
        else if (Input.GetKeyUp(MoveUp))    OnMoveUpReleased();

        if (Input.GetKeyDown(MoveDown))     OnMoveDownPressed();
        else if (Input.GetKeyUp(MoveDown))  OnMoveDownReleased();

        if (Input.GetKeyDown(MoveLeft))     OnMoveLeftPressed();
        else if (Input.GetKeyUp(MoveLeft))  OnMoveLeftReleased();

        if (Input.GetKeyDown(MoveRight))    OnMoveRightPressed();
        else if (Input.GetKeyUp(MoveRight)) OnMoveRightReleased();
        
    }
}
