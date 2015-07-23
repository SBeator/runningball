using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour 
{
	public float forwardForce = 100f;
	public float maxForwardSpeed = 10f;
	public float forwardResistanceForceFactor = 1f;
	public float sideResistanceForceFactor = 5f;
	public float minSpeed = 1f;
	public float levelUpDistance = 100f;
	public float levelStepFactor = 0.1f;

	public Vector3 forward;
	public Vector3 right;
	public Vector3 rightCenter;
	
	public float jumpSpeed = 22;
	public float gravity = 50;
	public float moveSideForce = 50;
	public float roadWidth = 10;

	public InputController input;

	float levelUpSpeedStep;
	float levelUpForceStep;
	float nextLevelUpdistance;
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
	AudioSource jumpAudio;

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
		this.nextLevelUpdistance = 0;
		this.lastPosition = this.rBody.position;

		this.levelUpForceStep = this.forwardForce * this.levelStepFactor;
		this.levelUpSpeedStep = this.maxForwardSpeed * this.levelStepFactor;

		this.stopTime = 0;
		this.isDead = false;

		this.jumpAudio = this.GetComponent<AudioSource>();
	}

	void FixedUpdate () 
	{
		if (!this.isDead) 
		{
			this.limitForwardSpeed();
			
			this.HandleJump ();
			this.HandleMoveSide();

			this.HandleRotate();

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

		this.nextLevelUpdistance += deltaDistance;
		if (this.nextLevelUpdistance > this.levelUpDistance) 
		{
			this.LevelUp();
			this.nextLevelUpdistance = 0;
		}
	}

	void LevelUp()
	{
		this.forwardForce += this.levelUpForceStep;
		this.maxForwardSpeed += this.levelUpSpeedStep;

	}

	float forwarSpeed()
	{
		return Vector3.Dot(this.forward, this.rBody.velocity);
	}

	float rightSpeed()
	{
		return Vector3.Dot(this.right, this.rBody.velocity);
	}

	void limitForwardSpeed()
	{
		var forceSize = - this.forwardForce / this.maxForwardSpeed * this.forwarSpeed() + this.forwardForce;
		this.rBody.AddForce(forceSize * this.forward);
	}
	
	void HandleMoveSide()
	{
		// var moveHorizontal = Input.GetAxis("Horizontal");
		var moveHorizontal = input.Horizontal;

		var rightOffset = Vector3.Dot((this.transform.position - this.rightCenter), this.right);
		var force = - 2f * this.moveSideForce / this.roadWidth * rightOffset + this.moveSideForce * moveHorizontal;
		this.rBody.AddForce(force * this.right);

		var rightSpeed = this.rightSpeed();
		var sideResistanceForce = - this.right.normalized * this.sideResistanceForceFactor * rightSpeed;
		this.rBody.AddForce(sideResistanceForce);
	}
	
	void HandleJump ()
	{
		if (this.jumpTime > this.jumpInterval && isOnFloor ()) {
			//var jump = Input.GetKeyDown (KeyCode.Space);
			var jump = input.GetContolType(ControlType.Jump);

			if (jump) {
				this.rBody.velocity += new Vector3 (0, this.jumpSpeed, 0);
				this.jumpTime = 0;
				this.jumpAudio.Play();
			}
		}
		this.jumpTime += Time.deltaTime;
	}

	void HandleRotate()
	{
		this.rBody.angularVelocity = this.forwarSpeed() * this.right;
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
