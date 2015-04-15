using UnityEngine;
using System.Collections;

public class Road : MonoBehaviour 
{
	public TriggerPlayer startTrigger;
	public TriggerPlayer[] endTriggers;
	public TurnCorner[] turnCorners;

	public Road[] NextRoads;
	public Road LastRoad;
	
	float minAngle = 0.01f;

//	public void SetAlongLastRoad(Road lastRoad)
//	{
//		foreach (var endTrigger in lastRoad.endTriggers) 
//		{
//			var startTransform = lastRoad.endTrigger.transform;
//			
//			var positionOffset = startTransform.position - startTrigger.transform.position;
//			
//			this.transform.position += positionOffset;
//			
//			var rotateAngle = Quaternion.Angle(startTrigger.transform.rotation, startTransform.rotation);
//			if (rotateAngle > this.minAngle)
//			{
//				if (rotateAngle < 180f - this.minAngle) 
//				{
//					this.transform.RotateAround(
//						this.transform.position, 
//						Vector3.Cross (startTrigger.transform.forward, startTransform.forward), 
//						Quaternion.Angle(startTrigger.transform.rotation, startTransform.rotation));	
//				}
//				else
//				{
//					this.transform.RotateAround(
//						this.transform.position, 
//						startTrigger.transform.up, 
//						180f);	
//				}
//			}
//		}
//	}

	public void SetAlongNewRoads(Road[] newRoads)
	{
		this.NextRoads = new Road[newRoads.Length];

		for (int i = 0; i < newRoads.Length; i++) 
		{
			var newRoad = newRoads[i];

			var startTransform = endTriggers[i].transform;
			
			var positionOffset = startTransform.position - newRoad.startTrigger.transform.position;
			
			newRoad.transform.position += positionOffset;
			
			var rotateAngle = Quaternion.Angle(newRoad.startTrigger.transform.rotation, startTransform.rotation);
			if (rotateAngle > this.minAngle)
			{
				if (rotateAngle < 180f - this.minAngle) 
				{
					newRoad.transform.RotateAround(
						newRoad.transform.position, 
						Vector3.Cross (newRoad.startTrigger.transform.forward, startTransform.forward), 
						Quaternion.Angle(newRoad.startTrigger.transform.rotation, startTransform.rotation));	
				}
				else
				{
					newRoad.transform.RotateAround(
						newRoad.transform.position, 
						newRoad.startTrigger.transform.up, 
						180f);	
				}
			}

			this.NextRoads[i] = newRoad;
			newRoad.LastRoad = this;
		}
	}

	public bool PlayerIn()
	{
		return this.startTrigger.Triggered;
	}
	
	public bool PlayerOut()
	{
		var playerOut = false;

		foreach (var endTrigger in this.endTriggers) 
		{
			if (endTrigger.Triggered) 
			{
				playerOut = true;
				break;
			}
		}

		return playerOut;
	}

	public Transform NeedTurn()
	{
		Transform turnTransform = null;

		foreach (var turn in this.turnCorners) 
		{
			if (turn.NeedTurn) 
			{
				turn.Turn();
				turnTransform = turn.TurnTransform;
				break;
			}
		}

		return turnTransform;
	}
}
