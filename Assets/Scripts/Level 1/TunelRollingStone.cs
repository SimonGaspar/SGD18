using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class TunelRollingStone : MonoBehaviour
{

	[SerializeField] public Rigidbody2D stone;
	[SerializeField] Animator anim;
	[SerializeField] AnimationClip animClip;
	public CircleCollider2D col;
	//public BoxCollider2D preventFromJumpGroundOff;  just incase someone wants to jump back up
	public bool disabled;
	public CircleCollider2D childStone;

	void Start()
	{
		stone.isKinematic = true;
		anim = GetComponent<Animator>();
		stone.velocity = Vector2.zero;
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (disabled)
		{
			stone.GetComponent<Deathzone>().enabled = false;
		}
		if (collision.tag == "Player")

		{
			StartCoroutine(ActivateStone());

		}
	}
	IEnumerator ActivateStone()
	{

		anim.Play(animClip.name);
		//preventFromJumpGroundOff.enabled = false;
		yield return new WaitForSeconds(0.12f);
		stone.isKinematic = false;

		yield return new WaitForSeconds(1f);
		stone.GetComponent<Collider2D>().enabled = true;
	}
	private void OnDrawGizmos()
	{
		Gizmos.DrawIcon(stone.transform.position, "fallingStone", true);
		Gizmos.DrawIcon(transform.position, "fallingBranch", true);

	}
}
