using UnityEngine;
using System.Collections;

public abstract class ControlTurnCorner : TurnCorner 
{
	public float TurnEventLastTime = 1f;
	
	protected bool TurnEventTriggered
	{
		get
		{
			return this.turnEventTriggered;
		}
	}

	bool turnEventTriggered;
	float turnEventTriggeredTime;

	void Start ()
	{
		this.turnEventTriggered = false;
		this.turnEventTriggeredTime = 0f;
	}

	void Update ()
	{
		if (this.turnEventTriggered) 
		{
			this.CheckIfNeedTurn ();
		}
	}

	protected void ControlTurn (bool control)
	{
		if (control) 
		{
			this.turnEventTriggered = true;
			this.turnEventTriggeredTime = 0f;
		}

		if (this.turnEventTriggered) 
		{
			this.turnEventTriggeredTime += Time.deltaTime;
			if (this.turnEventTriggeredTime > this.TurnEventLastTime) 
			{
				this.turnEventTriggered = false;
			}
		}
	}
}
