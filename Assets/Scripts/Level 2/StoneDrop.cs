using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoneDrop : MonoBehaviour
{

	[SerializeField] Transform endPoint;
	[SerializeField] Transform stone;
	Vector2 startPos;
	[Range(0, 10)] [SerializeField] float dropSpeed;
	bool triggered = false;
	bool startTimer = false;
	public float timer = 0.002f;
	float slowDis;

	void Start()
	{
		dropSpeed /= 1000;
		slowDis = Vector3.Distance(stone.position, endPoint.position) * 0.15f;
	}


	private void OnTriggerEnter2D(Collider2D collision)
	{

		if (collision.tag == "Player")
			return;
		startPos = stone.transform.position;
		startTimer = true;

	}
	void Update()
	{
		if (startTimer)
		{
			stone.position = Vector2.Lerp(stone.transform.position, endPoint.position, timer);
			timer += (0.01f * Time.deltaTime);
			if (Vector3.Distance(stone.position, endPoint.position) < slowDis)
				timer = 0.03f;
		}
	}
}

