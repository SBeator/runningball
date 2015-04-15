using UnityEngine;
using System.Collections;

public class TurnCorner : MonoBehaviour 
{
	public TriggerPlayer turnCenter;

	protected bool turned;
	protected bool needTurn;

	public bool NeedTurn 
	{
		get
		{
			return this.needTurn;
		}
	}

	public Transform TurnTransform 
	{
		get
		{
			return this.turnCenter.transform;
		}
	}

	void Start()
	{
		this.turned = false;
		this.needTurn = false;
	}

	void Update()
	{
		CheckIfNeedTurn ();
	}
	
	public void Turn()
	{
		this.turned = true;
		this.needTurn = false;
	}

	protected void CheckIfNeedTurn ()
	{
		if (!this.turned && !this.needTurn && turnCenter.Triggered) {
			this.needTurn = true;
		}
	}
}
