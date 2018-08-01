using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraLook : MonoBehaviour
{

	private Camera cam;
	private Transform followTarget;
	[SerializeField] Transform lookAtTarget;
	private bool claimed =false;
	// Use this for initialization
	void Start()
	{
		cam = Camera.main;
		followTarget = cam.GetComponent<CameraMovement>().followTarget;
		claimed = false;
	}

	// Update is called once per frame
	void Update()
	{

	}
	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (claimed)
			return;
		if (collision.tag == "Player")
		{
			followTarget = lookAtTarget;
			StartCoroutine(ResetCam());
			claimed = true;
		}
	}

	IEnumerator ResetCam()
	{
		yield return new WaitForSeconds(5f);
		followTarget = AnimalsManager.Instance.GetCurrentAnimalTransformComponent();
	}
}
