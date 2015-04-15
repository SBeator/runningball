using UnityEngine;
using System.Collections;

public class TriggerPlayer : MonoBehaviour 
{
	bool triggered;

	void Start()
	{
		this.triggered = false;
	}

	public bool Triggered
	{
		get 
		{
			return this.triggered;
		}
	}

	void OnTriggerEnter(Collider other) 
	{
		if (other.gameObject.tag == "Player") 
		{
			this.triggered = true;
		}
	}
}
