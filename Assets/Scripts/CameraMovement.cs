using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{

	Camera cam;
	float defOffsetX = 1.5f;
	float defOffsetY;
	[SerializeField]
	float offsetX;
	[SerializeField]
	float offsetY;

	[SerializeField]
	[Range(0, 1)]
	float smoothAmmount;
	[SerializeField]
	[Range(0, 1)]
	float smoothOffsetAmmount;



	float horizontal;
	bool isLookingLeft = true;

	[SerializeField]
	Transform followTarget;

	float targetOffsetX;

	void Start()
	{
		defOffsetX = offsetX;
		defOffsetY = offsetY;
		cam = GetComponent<Camera>();
		if (cam == null)
		{
			Debug.Log("No camera reference!");
		}
		if (followTarget == null)
		{
			Debug.Log("CameraScript: no player to follow, missing reference!");
		}
	}



	private void Update()
	{
		horizontal = Input.GetAxisRaw("Horizontal");
		if (horizontal == 1)
		{

			isLookingLeft = false;
		}
		else if (horizontal == -1)
		{

			isLookingLeft = true;
		}

		
	}

	void FixedUpdate()
	{
		float newOffsetX = (isLookingLeft) ? -offsetX : offsetX;
	


		targetOffsetX = Mathf.Lerp(targetOffsetX, newOffsetX, smoothOffsetAmmount);

		

		Vector3 offset = new Vector3(targetOffsetX, offsetY, -10f); //z not to block everything

		Vector3 currentPos = cam.transform.position;
		Vector3 targetPos = followTarget.position + offset;
		transform.position = Vector3.Lerp(currentPos, targetPos, smoothAmmount);
	}


}
