using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class plMovement_placeholder : MonoBehaviour
{
	//Camera cam;
	[SerializeField]
	Transform player;
	[SerializeField]
	Rigidbody2D rigid;
	[SerializeField]
	float movementSpeed=5f;
	[SerializeField]
	float jumpForce = 8;

	[SerializeField]
	CameraMovement cameraMovement;

	bool onGround;
	bool prevOnGround;
	RaycastHit2D hit;






	void Start()
	{

		rigid = GetComponent<Rigidbody2D>();
		if (rigid == null)
			Debug.Log("No rigid attached to player");
	}

	// Update is called once per frame
	void FixedUpdate()
	{
		onGround = OnGround();


		float horizontal = Input.GetAxis("Horizontal");
		float vertical = Input.GetAxis("Vertical");
		


		Vector2 move = new Vector2(horizontal, vertical);
		//move.Normalize();
		Vector2 targetVel = move;

		//if(onGround)
		//{
		if (Input.GetButtonDown("Jump"))
			targetVel.y = jumpForce;
		//}
		
		targetVel.x = movementSpeed * horizontal;
		Debug.Log(targetVel);
		rigid.velocity = Vector2.Lerp(rigid.velocity,targetVel,0.6f);

		if (prevOnGround != onGround)
		{
			if (onGround)
			{
				cameraMovement.AdjustYrelativeOffset(-1.2f);
			}
		}
		Debug.DrawLine(rigid.position, Vector3.up * 100);
		prevOnGround = onGround;
	}

	bool OnGround()
	{
		Vector2 origin = this.transform.position;
		origin = new Vector2(origin.x, origin.y - 0.8f);
		Vector2 dir = Vector2.down;

		float dis = 0.2f;
		hit = Physics2D.Raycast(origin, dir, dis);

		//Debug.DrawLine(origin, hit.point);

		return hit;

	}

	
}
