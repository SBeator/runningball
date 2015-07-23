using UnityEngine;
using System.Collections;

public class ControlTurnLeftCorner : ControlTurnCorner 
{
	public InputController input;
	
	void Start()
	{
		var inputControllerObject = GameObject.FindWithTag ("InputController");
		this.input =  inputControllerObject.GetComponent("MobileInput") as MobileInput;
		//this.input =  inputControllerObject.GetComponent<MobileInput>();
	}

	void Update()
	{
		//this.ControlTurn (Input.GetKeyDown (KeyCode.J));
		this.ControlTurn (input.GetContolType(ControlType.TurnLeft));
		
		if (this.TurnEventTriggered) 
		{
			this.CheckIfNeedTurn ();
		}
	}
}
