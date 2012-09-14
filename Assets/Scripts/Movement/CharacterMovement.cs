using System;
using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CharacterController))]
public class CharacterMovement : MonoBehaviour
{

    public float MoveSpeed = .5f,
                 Gravity = .97f;

    private bool isMovingForward,
                 isMovingLeft,
                 isMovingRight,
                 isMovingBack,
				 isEnabled = true;
	public bool Enabled
	{
		get { return isEnabled; }
		set { isEnabled = value; }
	}

    public bool IsMoving
    {
        get { if (isMovingForward || isMovingLeft || isMovingRight || isMovingBack) return true;
              else return false; }
    }
    private CharacterController controller;

    public void Reset()
    {
        isMovingForward = false;
        isMovingLeft = false;
        isMovingRight = false;
        isMovingBack = false;
    }
    // Use this for initialization
	void Start ()
	{
        controller = GetComponent<CharacterController>();
	    Inputbase.Instance.OnMoveDownPressedHandle   += StartBack;
        Inputbase.Instance.OnMoveDownReleasedHandle  += StopBack;

        Inputbase.Instance.OnMoveLeftPressedHandle   += StartLeft;
        Inputbase.Instance.OnMoveLeftReleasedHandle  += StopLeft;

        Inputbase.Instance.OnMoveRightPressedHandle  += StartRight;
        Inputbase.Instance.OnMoveRightReleasedHandle += StopRight;

        Inputbase.Instance.OnMoveUpPressedHandle     += StartForward;
        Inputbase.Instance.OnMoveUpReleasedHandle    += StopForward;
        Inputbase.Instance.OnInputReset  += Reset;
		Conversation.OnConversationStarted += Disable;
	    Conversation.OnConversationEnded += Enable;
        isEnabled = true;


	}
    void OnApplicationFocus()
    {
        isMovingForward = false;
        isMovingLeft    = false;
        isMovingRight   = false;
        isMovingBack    = false;
    }
	
	void Disable(object sender, EventArgs e)
	{
		isEnabled = false;
	}
	
	void Enable(object sender, EventArgs e)
	{
		isEnabled = true;
	}

    void StopBack(object sender, EventArgs e)
    {
        isMovingBack = false;
    }

    void StartBack(object sender, EventArgs e)
    {
        isMovingBack = true;
    }

    void StartLeft(object sender, EventArgs e)
    {
        isMovingLeft = true;
    }
    void StopLeft(object sender, EventArgs e)
    {
        isMovingLeft = false;
    }

    void StartRight(object sender, EventArgs e)
    {
        isMovingRight= true;
    }
    void StopRight(object sender, EventArgs e)
    {
        isMovingRight = false;
    }

    void StartForward(object sender, EventArgs e)
    {
        isMovingForward = true;
    }
    void StopForward(object sender, EventArgs e)
    {
        isMovingForward = false;
    }

    void OnDestroy()
    {
       //Debug.Log("Dead");
        Inputbase.Instance.OnMoveDownPressedHandle   -= StartBack;
        Inputbase.Instance.OnMoveDownReleasedHandle  -= StopBack;

        Inputbase.Instance.OnMoveLeftPressedHandle   -= StartLeft;
        Inputbase.Instance.OnMoveLeftReleasedHandle  -= StopLeft;

        Inputbase.Instance.OnMoveRightPressedHandle  -= StartRight;
        Inputbase.Instance.OnMoveRightReleasedHandle -= StopRight;

        Inputbase.Instance.OnMoveUpPressedHandle     -= StartForward;
        Inputbase.Instance.OnMoveUpReleasedHandle    -= StopForward;

        Inputbase.Instance.OnInputReset    -= Reset;
        Conversation.OnConversationStarted -= Disable;
        Conversation.OnConversationEnded   -= Enable;
    }
	
	// Update is called once per frame
	void Update ()
	{
		if(!isEnabled) return;
	    Vector3 moveAmount = new Vector3();
        if (isMovingBack)
            moveAmount += Vector3.back;
        if (isMovingForward)
            moveAmount += Vector3.forward;
        if (isMovingLeft)
            moveAmount+= Vector3.left;
        if (isMovingRight)
            moveAmount += Vector3.right;

        
	    moveAmount *= Time.smoothDeltaTime;
	    moveAmount *= MoveSpeed;

	    CollisionFlags flag =  controller.Move(transform.TransformDirection(moveAmount));

        if((flag & CollisionFlags.CollidedBelow) == 0)
        {
            controller.Move(new Vector3(0, -Gravity, 0));
        }


	}
}
