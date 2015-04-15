using UnityEngine;
using System.Collections;

public class FollowCamera : MonoBehaviour 
{
	public Player player;
	public float moveSmooth = 0.3f;
	public float timeAfterPlayerDead = 0.2f;

	Vector3 offset;
	Vector3 offsetUp;
	float backFactor;
	Vector3 rotateDown;

	float stopTime;

	void Start () 
	{
		var playerForward = this.player.forward;
		var thisForward = this.transform.forward;

		this.rotateDown = new Vector3(
			0, 
			(playerForward.x + playerForward.z) / (thisForward.x + thisForward.z) * thisForward.y - playerForward.y,
			0);

		this.offset = this.transform.position - this.player.transform.position;
		this.offsetUp = Vector3.Dot(this.offset, this.rotateDown) * this.rotateDown;
		this.backFactor = Vector3.Dot(this.offset, playerForward.normalized);

		this.stopTime = 0;
	}
	
	void Update () 
	{
		var newPosition = this.GetBallCenterPosition() + this.offsetUp + this.transform.forward.normalized * this.backFactor;

		var newRotation = Quaternion.LookRotation(this.player.forward + this.rotateDown);
		//newRotation = Quaternion.AngleAxis(this.rotateAngle, this.player.right);
		//var newRotation = Quaternion.FromToRotation(aaa, this.player.forward);


		if (this.player.IsDead) 
		{
			this.stopTime += Time.deltaTime;
		}

		if (this.stopTime < this.timeAfterPlayerDead) 
		{
			this.transform.position = Vector3.Lerp(this.transform.position, newPosition, moveSmooth * Time.deltaTime);
			this.transform.rotation = Quaternion.Lerp(this.transform.rotation, newRotation, moveSmooth * Time.deltaTime);
		}
	}

	Vector3 GetBallCenterPosition()
	{
		var playerPosition = this.player.transform.position;
		var xp = playerPosition.x;
		var yp = playerPosition.y;
		var zp = playerPosition.z;
		var crosss = Vector3.Cross(Vector3.up, this.player.forward);
		var a = crosss.x;
		var b = crosss.y;
		var c = crosss.z;
		var d = -(a * this.player.rightCenter.x + b * this.player.rightCenter.y + c * this.player.rightCenter.z);
		var k = -(a * xp + b * yp + c * zp + d) / (a * a + b * b + c * c);
		return new Vector3(
			xp + k * a,
			yp + k * b,
			zp + k * c);
	}
}
