using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BisonRandomMovement : MonoBehaviour
{
	bool pauseScript = true;
	[SerializeField] Transform bison;
	Rigidbody2D rigid;
	[SerializeField]
	List<AnimationCurve> animCurves = new List<AnimationCurve>();
	[SerializeField] [Range(1, 6)] float pauseMaxDuration = 5f;
	[SerializeField] float movementSpeed = 3f;
	bool pauseBison = false;

	[SerializeField] Transform startPoint;
	[SerializeField] Transform endPoint;

	float distance;
	[SerializeField]
	float bisonTimeOnCurve;
	bool revert = false;
	int curve;
	int time = 5;
	float tempTime;

	private void Start()
	{

		Random.InitState(1);
		distance = Vector2.Distance(startPoint.position, endPoint.position);
		rigid = bison.GetComponent<Rigidbody2D>();
		tempTime = time;
		pauseBison = true;
	}

	private void Update()
	{
		if (pauseScript)
			return;

		tempTime -= tempTime * Time.deltaTime;


		bisonTimeOnCurve = distance - Vector2.Distance(bison.transform.position, startPoint.position);
		if (bisonTimeOnCurve > distance - 0.5)
		{
			revert = false;
			curve = GetRandomCurve();
		}
		else if (bisonTimeOnCurve < 0.5)
		{
			revert = true;

			curve = GetRandomCurve();
		}

		if (!pauseBison)
		{
			rigid.velocity = new Vector2(GetKeyframe(bisonTimeOnCurve, revert, curve) * movementSpeed, 0f);

			if (tempTime < 0.1)
			{
				tempTime = Pause();
				pauseBison = true;
			}
		}
		else
		{

			if (tempTime < 0.1)
			{
				tempTime = Pause() + 7;
				pauseBison = false;
			}

			rigid.velocity = Vector2.zero;
		}
	}

	float GetKeyframe(float bison, bool revert, int curve)
	{
		float value;
		if (revert)
			value = animCurves[curve].Evaluate((bison / 100f * distance));
		else
			value = -animCurves[curve].Evaluate((bison / 100f * distance));
		return value;
	}
	int GetRandomCurve()
	{

		return Random.Range((int)0, animCurves.Count);

	}
	int Pause()
	{

		return Random.Range(2, (int)pauseMaxDuration);
	}
	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.gameObject.layer == 10)
		{
			pauseScript = false;
		}
	}
	public void ResetScript()
	{
		pauseScript = true;
	}

}
