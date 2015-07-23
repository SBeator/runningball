using UnityEngine;
using System.Collections;

public class ControlTurnRightCorner : ControlTurnCorner 
{
	public InputController input;

	void Start()
	{
		var inputControllerObject = GameObject.FindWithTag ("InputController");
		this.input =  inputControllerObject.GetComponent("MobileInput") as MobileInput;
	}

	void Update()
	{
		//this.ControlTurn (Input.GetKeyDown (KeyCode.L));
		this.ControlTurn (input.GetContolType(ControlType.TurnRight));

		if (this.TurnEventTriggered) 
		{
			this.CheckIfNeedTurn ();
		}
	}
}
