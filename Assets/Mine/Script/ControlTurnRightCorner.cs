using UnityEngine;
using System.Collections;

public class ControlTurnRightCorner : ControlTurnCorner 
{
	void Update()
	{
		this.ControlTurn (Input.GetKeyDown (KeyCode.L));
		
		if (this.TurnEventTriggered) 
		{
			this.CheckIfNeedTurn ();
		}
	}
}
