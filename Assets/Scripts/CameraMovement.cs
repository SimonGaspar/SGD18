using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{

	public Camera cam;
	float defOffsetX = 1.5f;
	float defOffsetY;

	DebugCamScript debugCamScript;

	[SerializeField]
	bool enableOffset = true;
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
	public bool outOfBoundsX = false;
	public bool outOfBoundsY = false;

	[SerializeField]
	Transform followTarget;

	float targetOffsetX;



	float targetPositionX;

	float targetPositionY;
	Vector3 offset;

	void Start()
	{

		targetPositionX = followTarget.position.x + offset.x;
		targetPositionY = followTarget.position.y + offset.y;
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


		debugCamScript = DebugCamScript.singleton;

		debugCamScript.cam = cam;
	}



	private void Update()
	{

		debugCamScript.target = followTarget;


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

	void LateUpdate()
	{
		float newOffsetX;
		if (enableOffset)
		{
			newOffsetX = (isLookingLeft) ? -offsetX : offsetX;
		}
		else
		{
			newOffsetX = 0.2f;
		}

		targetOffsetX = Mathf.Lerp(targetOffsetX, newOffsetX, smoothOffsetAmmount);



		offset = new Vector3(targetOffsetX, offsetY, -10f); //z not to block everything

		Vector3 currentPos = cam.transform.position;
		//Vector3 targetPos = followTarget.position + offset;

		//float positionX = cam.transform.position.x;
		//float targetX = followTarget.position.x + offset.x;
		//float positionY = cam.transform.position.y;
	 //+ offset.y;

		if (outOfBoundsX)
		{
			targetPositionX = Mathf.Lerp(cam.transform.position.x, followTarget.position.x, 0.3f);
		}
		else
		{
			float targetPositionX = cam.transform.position.x;
		}
		if (outOfBoundsY)
		{
			targetPositionY = Mathf.Lerp(cam.transform.position.y, followTarget.position.y, 0.8f);
			Debug.Log(true);
		}
		else
		{
			float targetPositionY = cam.transform.position.y;
		}
		Vector3 targetPos = new Vector3(targetPositionX, targetPositionY, cam.transform.position.z);

		transform.position = Vector3.Lerp(currentPos, targetPos, smoothAmmount);

	}


}
