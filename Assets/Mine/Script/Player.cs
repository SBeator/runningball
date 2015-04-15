using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour 
{
	public float forwardForce = 100;
	public float maxForwardSpeed = 10;
	public float minSpeed = 1;

	public Vector3 forward;
	public Vector3 right;
	public Vector3 rightCenter;
	
	public float jumpSpeed = 22;
	public float gravity = 50;
	public float moveSideForce = 50;
	public float roadWidth = 10;

	float distance;
	Vector3 lastPosition;
	
	Player movement;
	
	float jumpTime = 0f;
	float jumpInterval = 0.2f;

	float maxStopTime = 0.5f;
	float stopTime;
	
	float floorHeightTolerances = 0.1f;
	float underFloorHeight = -5f;
	float floorHeight;
	Rigidbody rBody;

	bool isDead;

	public bool IsDead 
	{
		get 
		{
			return this.isDead;
		}
	}

	public float Distance
	{
		get
		{
			return this.distance;
		}
	}

	void Awake()
	{
		this.forward = Vector3.forward;
		this.right = Vector3.right;
		this.rightCenter = Vector3.zero;
	}

	void Start()
	{
		Physics.gravity = new Vector3(0, -gravity, 0);
		this.rBody = this.GetComponent<Rigidbody> ();
		this.floorHeight = this.rBody.position.y;

		this.distance = 0;
		this.lastPosition = this.rBody.position;

		this.stopTime = 0;
		this.isDead = false;
	}

	void FixedUpdate () 
	{
		if (!this.isDead) 
		{
			this.rBody.AddForce(this.forwardForce * this.forward);
			
			if (this.forwarSpeed() > this.maxForwardSpeed) 
			{
				this.limitForwardSpeed();
			}
			
			this.HandleJump ();
			this.HandleMoveSide();

			this.RecordDistance();
		}
	}
	
	public void Turn(Transform turnTransform)
	{
		this.forward = turnTransform.forward.normalized;
		this.right = turnTransform.right.normalized;
		this.rightCenter = turnTransform.position;
	}

	public bool IsOver()
	{
		var over = false;

		if( Vector3.Dot(this.rBody.velocity, this.forward.normalized) < minSpeed)
		{
			this.stopTime += Time.deltaTime;
		} 
		else
		{
			this.stopTime = 0;
		}

		if (this.stopTime > this.maxStopTime) 
		{
			over = true;
		}

		over = this.stopTime > this.maxStopTime;
		over |= this.isUnderFloor();

		return over;
	}

	public void Die()
	{
		this.isDead = true;
	}

	void RecordDistance()
	{
		var position = this.rBody.position;
		var deltaDistance = Vector3.Dot((position - this.lastPosition), this.forward.normalized);

		this.distance += deltaDistance;
		this.lastPosition = position;
	}

	float forwarSpeed()
	{
		return Vector3.Dot(this.forward, this.rBody.velocity);
	}

	void limitForwardSpeed()
	{
		this.rBody.velocity = this.rBody.velocity - (this.forwarSpeed() - this.maxForwardSpeed) * this.forward;
	}
	
	void HandleMoveSide()
	{
		var moveHorizontal = Input.GetAxis("Horizontal");
		
		var rightOffset = Vector3.Dot((this.transform.position - this.rightCenter), this.right);
		var force = - 2f * this.moveSideForce / this.roadWidth * rightOffset + this.moveSideForce * moveHorizontal;
		this.rBody.AddForce(force * this.right);
	}
	
	void HandleJump ()
	{
		if (this.jumpTime > this.jumpInterval && isOnFloor ()) {
			var jump = Input.GetKeyDown (KeyCode.Space);
			if (jump) {
				this.rBody.velocity += new Vector3 (0, this.jumpSpeed, 0);
				this.jumpTime = 0;
			}
		}
		this.jumpTime += Time.deltaTime;
	}
	
	bool isOnFloor()
	{
		return this.rBody.position.y < this.floorHeight + this.floorHeightTolerances;
	}

	bool isUnderFloor()
	{
		return this.rBody.position.y < this.floorHeight + this.underFloorHeight;
	}
}
