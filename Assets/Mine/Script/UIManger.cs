using UnityEngine;
using System.Collections;

public class UIManger : MonoBehaviour 
{
	public GameControllor gameControllor;

	void AnimatorFinished()
	{
		gameControllor.CanReset();
	}
}
