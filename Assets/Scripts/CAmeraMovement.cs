using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CAmeraMovement : MonoBehaviour
{

	Camera cam;
	float defOffsetX = 1.5f;
	float defOffsetY;
	[SerializeField]
	float offsetX;
	[SerializeField]
	float offsetY;

	float horizontal;
	bool isLookingLeft =true;

	[SerializeField]
	Transform followTarget;

	float targetOffsetX;

	void Start()
	{
		offsetX = defOffsetX;
		offsetY = defOffsetY;
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
		

		
	//	isLookingLeft = (Input.GetAxisRaw("Horizontal") ==1)? true  : false);
	}
	
	void LateUpdate()
	{
		
		targetOffsetX = (isLookingLeft)? -offsetX : offsetX;
		Vector3 offset = new Vector3(targetOffsetX, offsetY,-10f); //z not to block everything

		Vector3 currentPos = cam.transform.position;
		Vector3 targetPos = followTarget.position+offset;

		transform.position = Vector3.Lerp(currentPos, targetPos, 0.3f);
	}

	
}
