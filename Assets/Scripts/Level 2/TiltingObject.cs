using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TiltingObject : MonoBehaviour
{


	[SerializeField] float rotationAmmount = 5f;

	[Range(0, 0.08f)] [SerializeField] float rotationsSpeed = 0.008f;
	[SerializeField] bool rotateClockwise = true;
	bool inTrigger = false;
	Vector3 targetRot;
	Vector3 defaultRotation;

	//returnToNormal serves as revert to original state ; currently cant rotate both ways,its ssetp to go only from initial state to rotated and back
	[SerializeField]bool returnToNormal = false;

	void Start()
	{
		defaultRotation = transform.rotation.eulerAngles;

		float targetRotation = ((rotateClockwise) ? +rotationAmmount : -rotationAmmount);
		targetRot = new Vector3(0f, 0f, targetRotation) + defaultRotation;
	}

	//beware of scale if its negative it might rotate incorrecctly 
	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.tag == "Player")
			return;
		inTrigger = true;
	}
	IEnumerator RotateObject(Vector3 targetRot)
	{
		yield return new WaitForSeconds(0.5f);
		transform.eulerAngles = Vector3.Lerp(transform.rotation.eulerAngles, targetRot, rotationsSpeed);
	}
	private void Update()
	{
		if(returnToNormal)
		{
			transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(defaultRotation), rotationsSpeed);
		}

		else if (inTrigger)
		{
			transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(targetRot), rotationsSpeed);
		}

	}
}
