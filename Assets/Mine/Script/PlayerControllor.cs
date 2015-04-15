//using UnityEngine;
//using System.Collections;
//
//public class PlayerControllor : MonoBehaviour 
//{
//	public float jumpSpeed = 22;
//	public float gravity = 50;
//	public float moveSideForce = 10;
//	public float backCenterForce = 10;
//	public float roadWidth = 10;
//
//	PlayerMovement movement;
//	
//	float jumpTime = 0f;
//	float jumpInterval = 0.2f;
//
//	float floorHeightTolerances = 0.1f;
//	float floorHeight;
//
//	// float centerPositionTolerances = 0.1f;
//
//	Rigidbody rBody;
//	
//	void Start()
//	{
//		Physics.gravity = new Vector3(0, -gravity, 0);
//		this.rBody = this.GetComponent<Rigidbody> ();
//		this.floorHeight = this.rBody.position.y;
//		this.movement = this.GetComponent<PlayerMovement>();
//	}
//	
//	void FixedUpdate ()
//	{
//		HandleJump ();
//		HandleMoveSide();
//	}
//
//	void HandleMoveSide()
//	{
//		var moveHorizontal = Input.GetAxis("Horizontal");
//
//		var rightOffset = Vector3.Dot((this.transform.position - this.movement.rightCenter), this.movement.right);
//		var force = - 2f * this.moveSideForce / this.roadWidth * rightOffset + this.moveSideForce * moveHorizontal;
//		this.rBody.AddForce(force * this.movement.right);
//	}
//
//	void HandleJump ()
//	{
//		if (this.jumpTime > this.jumpInterval && isOnFloor ()) {
//			var jump = Input.GetKeyDown (KeyCode.Space);
//			if (jump) {
//				this.rBody.velocity += new Vector3 (0, this.jumpSpeed, 0);
//				this.jumpTime = 0;
//			}
//		}
//		this.jumpTime += Time.deltaTime;
//	}
//
//	bool isOnFloor()
//	{
//		return this.rBody.position.y < this.floorHeight + this.floorHeightTolerances;
//	}
//}
