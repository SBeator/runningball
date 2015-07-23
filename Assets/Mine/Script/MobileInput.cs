using UnityEngine;
using System.Collections;

public enum TouchType
{
	None,
	SweepLeft,
	SweepRight,
	Click,
	Reset
}

public class MobileInput : InputController
{
	public float minMovement = 1f;
	public float controlEventDelay = 0.1f;
	
	TouchType touchType;
	ControlType controlType;
	float horizontal;
	Vector2 startPos;
	Vector2 direction;
	bool directionChosen;
	float controlEventTime;

	public override TouchType TouchType 
	{
		get
		{
			return this.touchType;
		}
	}
	
	public override float Horizontal 
	{
		get
		{
			return this.horizontal;
		}
	}

	public override bool GetContolType(ControlType controlType)
	{
		if (controlType == this.controlType) 
		{
			this.controlType = ControlType.None;
			return true;
		}

		return false;
	}

	void Start()
	{
		directionChosen = false;
	}

	// Update is called once per frame
	void Update () 
	{
		this.SetTouchType ();
		this.SetHorizontal ();
		this.SetControlType ();
	}

	void SetTouchType ()
	{
		touchType = TouchType.None;
		// Track a single touch as a direction control.
		if (Input.touchCount > 0) 
		{
			var touch = Input.GetTouch (0);
			// Handle finger movements based on touch phase.
			switch (touch.phase) 
			{
			// Record initial touch position.
			case TouchPhase.Began:
				this.startPos = touch.position;
				this.direction = Vector3.zero;
				directionChosen = false;
				break;
			// Determine direction by comparing the current touch
			// position with the initial one.
			case TouchPhase.Moved:
				this.direction = touch.position - this.startPos;
				break;
			// Report that a direction has been chosen when the finger is lifted.
			case TouchPhase.Ended:
				directionChosen = true;
				break;
			}
		}
		if (directionChosen) 
		{
			directionChosen = false;
			if (direction.magnitude > this.minMovement) 
			{
				if (Mathf.Abs (direction.x) > Mathf.Abs (direction.y)) 
				{
					if (direction.x > 0) 
					{
						touchType = TouchType.SweepRight;
					}
					else 
					{
						touchType = TouchType.SweepLeft;
					}
				}
			}
			else 
			{
				touchType = TouchType.Click;
			}
		}
	}

	void SetHorizontal()
	{
		var horizontal = Input.acceleration.x * 1.5f;
		if (horizontal > 1) 
		{
			horizontal = 1;
		}
		else if (horizontal < -1)
		{
			horizontal = -1;
		}

		this.horizontal = horizontal;
	}

	void SetControlType()
	{
		ControlType controlType = this.controlType;

		switch(this.touchType)
		{
			case TouchType.Click:
				controlType = ControlType.Jump;
				this.controlEventTime = 0;
				break;
			case TouchType.SweepLeft:
				controlType = ControlType.TurnLeft;
				this.controlEventTime = 0;
				break;
			case TouchType.SweepRight:
				controlType = ControlType.TurnRight;
				this.controlEventTime = 0;
				break;
			default:
				if (this.controlEventTime > this.controlEventDelay) 
				{
					controlType = ControlType.None;
				}
				this.controlEventTime += Time.deltaTime;
				break;
		}

		this.controlType = controlType;
	}
}
