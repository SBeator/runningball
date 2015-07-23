using UnityEngine;
using System.Collections;

public enum ControlType
{
	None,
	TurnLeft,
	TurnRight,
	Jump,
}

public abstract class InputController : MonoBehaviour
{
	public abstract bool GetContolType(ControlType controlType);

	public abstract float Horizontal {get;}

	public abstract TouchType TouchType {get;}
}
