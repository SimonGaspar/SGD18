using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//simpple camera shake
public class CameraShake : MonoBehaviour
{

	Camera cam;

	[SerializeField]
	[Range(0, 1)]
	float trauma;
	float cameraShake;

	float angle;

	float offsetX;
	float offsetY;

	[SerializeField]
	float maxAngle;
	[SerializeField]
	float maxOffset;


	void Start()
	{
		cam = this.GetComponent<Camera>();
		float cameraShake = trauma * trauma;
	}
	
	void Update()
	{

		if (Input.GetKeyDown(KeyCode.I))
		{
			trauma += 0.2f;
		}	

		cameraShake = trauma * trauma;

		CalcShake(cameraShake);
		if (trauma > 0)
		{
			trauma -= 0.5f * Time.deltaTime;
		}
		trauma = Mathf.Clamp01(trauma);
		ApplyState();

	}

	void CalcShake(float shake)
	{
		angle = maxAngle * shake * Random.Range(-1f,1f);
		offsetX = maxOffset * shake * Random.Range(-1f, 1f);
		offsetY = maxOffset * shake * Random.Range(-1f, 1f);
	}
	void ApplyState()
	{
		cam.transform.position = cam.transform.position + new Vector3(offsetX, offsetY, 0f);
		cam.transform.eulerAngles = new Vector3(0f, 0f, angle);
	}
}
