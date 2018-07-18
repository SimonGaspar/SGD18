using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BisonRandomMovement : MonoBehaviour
{

	Transform bison;
	Rigidbody2D rigid;
	[SerializeField]
	List<AnimationCurve> animCurves = new List<AnimationCurve>();
	//one cycle duration
	[SerializeField] float cycleTime = 12f;
	[SerializeField] float movementSpeed = 3f;
	[SerializeField] bool pause = false;

	[SerializeField] Transform startPoint;
	[SerializeField] Transform endPoint;

	float distance;
	[SerializeField]
	float bisonTimeOnCurve;
	public bool revert = false;
	int curve;

	private void Start()
	{
		bison = transform;
		distance = Vector2.Distance(startPoint.position, endPoint.position);
		rigid = GetComponent<Rigidbody2D>();

	}

	private void Update()
	{
		bisonTimeOnCurve = distance - Vector2.Distance(bison.transform.position, startPoint.position);
		if (bisonTimeOnCurve > distance - 0.5)
		{
			revert = true;
			curve = GetRandomCurve();
		}
		else if (bisonTimeOnCurve < 0.5)
		{
			revert = false;

			curve = GetRandomCurve();
		}


		if (!pause)
		{
			rigid.velocity = new Vector2(GetKeyframe(bisonTimeOnCurve, revert, curve) * movementSpeed, 0f);
			StartCoroutine(MakePause());
		}
		else
		{
			rigid.velocity = Vector2.zero;
		}
		//	Debug.Log();
	
		//Debug.Log(string.Format("curr bison mov: {0} distanceof points: {1}", bisonTimeOnCurve, distance));
	}
	
	IEnumerator MakePause()  //do i really need two yields?
	{
		yield return new WaitForSeconds(5f);
		float value = Random.Range(0, 60);
		if (value < 6)
			pause = true;
		yield return new WaitForSeconds(5f);
		pause = false;
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
	bool Pause()
	{

		return (Random.Range(0, 40) < 6) ? true : false;
	}

}
