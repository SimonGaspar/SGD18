using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class plMovement_placeholder : MonoBehaviour
{

	[SerializeField]
	Transform player;
	Rigidbody2D rigid;
	[SerializeField]
	float movementSpeed;


	// Use this for initialization
	void Start()
	{
		rigid = GetComponent<Rigidbody2D>();
		if (rigid == null)
			Debug.Log("No rigid attached to player");
	}

	// Update is called once per frame
	void Update()
	{

		float horizontal = Input.GetAxisRaw("Horizontal");
		float vertical = Input.GetAxisRaw("Vertical");
		Vector2 move = new Vector2(horizontal, vertical);
		move.Normalize();
		Vector2 targetVel = rigid.velocity;



		if (horizontal == 0)
			targetVel.x = (horizontal < 0) ? -1 : 1;
		else
			targetVel.x = 0;


		targetVel.x = movementSpeed * horizontal;

		rigid.velocity = Vector2.Lerp(rigid.velocity, targetVel, 0.8f);


	}
}
