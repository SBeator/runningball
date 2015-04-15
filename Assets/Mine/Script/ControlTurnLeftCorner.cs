using UnityEngine;
using System.Collections;

public class ControlTurnLeftCorner : ControlTurnCorner 
{
	void Update()
	{
		this.ControlTurn (Input.GetKeyDown (KeyCode.J));
		
		if (this.TurnEventTriggered) 
		{
			this.CheckIfNeedTurn ();
		}
	}
}
