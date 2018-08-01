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
		Rigidbody2D rigid = collision.gameObject.GetComponent<Rigidbody2D>();
		rigid.gravityScale = 12f;
		rigid.velocity = Vector2.zero;
		//rigid.constraints = RigidbodyConstraints2D.FreezePositionX;
		rigid.bodyType = RigidbodyType2D.Dynamic;
		StartCoroutine(RotateObject(collision));
		Destroy(collision.gameObject.GetComponent<PushableObject>());
	}
	IEnumerator RotateObject(Collider2D collision)
	{
		yield return new WaitForSeconds(0.8f);
		//transform.eulerAngles = Vector3.Lerp(transform.rotation.eulerAngles, targetRot, rotationsSpeed);

		//collision.gameObject.transform.parent = transform;
		inTrigger = true;
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
