using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DebugCamScript : MonoBehaviour
{
	CameraMovement cameraMovement;
	[SerializeField]
	Image upperBar;
	[SerializeField]
	Image lowerBar;
	[SerializeField]
	Image leftBar;
	[SerializeField]
	Image rightBar;
	[SerializeField]
	Texture testTexture;


	[HideInInspector]
	public Transform target;

	public Camera cam;
	Bounds camDeadzone;


	[SerializeField]
	[Range(0, 1)]
	float relativeXdeadZone;

	[SerializeField]
	[Range(0, 1)]
	float DeadzoneWidth;

	[SerializeField]
	bool centerX = false;

	[SerializeField]
	[Range(0, 1)]
	float relativeYdeadZone;

	[SerializeField]
	[Range(0, 1)]
	float DeadzoneHeight;


	[SerializeField]
	bool centerY = false;



	Vector2 camSize;
	Vector2 relativeValues;
	Vector2 position;
	Vector3 screenTarget;
	Vector2 distanceFromCenter;

	Vector2 maxDif;

	public static DebugCamScript singleton;

	private void Awake()
	{
		singleton = this;

	}

	void Start()
	{
		cameraMovement = GetComponent<CameraMovement>();

		camDeadzone.center = cam.transform.position;
		camSize = new Vector2(cam.pixelWidth, cam.pixelHeight);
	}


	void Update()
	{

		screenTarget = cam.WorldToScreenPoint(target.transform.position);
		if (centerX)
		{
			Center(true);
		}
		if (centerY)
		{
			Center(false);
		}
		camSize = new Vector2(cam.pixelWidth, cam.pixelHeight);
		Vector2 camCenter = new Vector2(cam.pixelWidth / 2, cam.pixelHeight / 2);

		relativeValues = CalculateRelativePos(camSize, new Vector2(relativeXdeadZone, relativeYdeadZone));


		lowerBar.rectTransform.position = new Vector2(camSize.y, relativeValues.y);
		upperBar.rectTransform.position = new Vector2(camSize.y, relativeValues.y + DeadzoneHeight * camSize.y);
		leftBar.rectTransform.position = new Vector2(relativeValues.x, 0f);
		rightBar.rectTransform.position = new Vector2(relativeValues.x + DeadzoneWidth * camSize.x, 0f);

		camDeadzone.size = new Vector3(rightBar.rectTransform.position.x - leftBar.rectTransform.position.x,
										upperBar.rectTransform.position.y - lowerBar.rectTransform.position.y, 0f);


		maxDif.x = Mathf.Abs(camDeadzone.extents.x);
		maxDif.y = Mathf.Abs(camDeadzone.extents.y);
		//maxDif = cam.WorldToScreenPoint(maxDif);

		//	Debug.DrawRay(new Vector2(camSize.x / 2 - screenTarget.x, camSize.y / 2 - screenTarget.y),Vector2.up);
		Debug.DrawRay(camDeadzone.max, Vector2.up*100);
		
		distanceFromCenter = new Vector2(camDeadzone.max.x - screenTarget.x, camDeadzone.max.y - screenTarget.y);
		//Debug.Log(camDeadzone.center.x);
		Debug.DrawRay(distanceFromCenter, Vector3.up);
		if (Mathf.Abs(distanceFromCenter.x) > maxDif.x) //bez abs potom elsef ci s abs aby som vedel z ktorej strany ide
		{
			cameraMovement.outOfBoundsX = true;
			
		}
		else
			cameraMovement.outOfBoundsX = false;

		if (Mathf.Abs(distanceFromCenter.y) > maxDif.y)
			cameraMovement.outOfBoundsY = true;
		else
			cameraMovement.outOfBoundsY = false;


		//Debug.Log(target.transform.position);
		//Debug.DrawLine(screenTarget, camDeadzone.max);

		//if (Input.GetKeyUp(KeyCode.T)) 
			//AdjustYrelativeOffset();

	}




	Vector2 CalculateRelativePos(Vector2 camSize, Vector2 deadZone)
	{
		Vector2 relativeDeadZone = Vector2.zero;


		relativeDeadZone.x = deadZone.x * camSize.x;

		relativeDeadZone.y = deadZone.y * camSize.y;

		return relativeDeadZone;
	}
	private void OnDrawGizmos()
	{

		camDeadzone.center = new Vector3(relativeValues.x, relativeValues.y, 0f);
		Rect rect = new Rect(camDeadzone.center, camDeadzone.size);
		Gizmos.DrawGUITexture(rect, testTexture);
		//Debug.Log();
	}

	void Center(bool isX)
	{
		if (isX)
		{
			DeadzoneWidth = relativeXdeadZone;
		}
		else
		{
			DeadzoneHeight = relativeYdeadZone;
		}
	}
	public void AdjustYrelativeOffset(float target)
	{
		relativeYdeadZone = target / camSize.y;		//screenTarget.y / camSize.y; 
	}

}
