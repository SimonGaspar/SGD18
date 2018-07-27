using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Animator))]
public class TunelRollingStone : MonoBehaviour {

	[SerializeField] Rigidbody2D stone;
	[SerializeField]Animator anim;
	[SerializeField] AnimationClip animClip;
	public CircleCollider2D col;
	public bool disabled;
	public CircleCollider2D childStone;
	void Start () {
		stone.isKinematic = true;
		anim = GetComponent<Animator>();
	}
	
	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.tag == "Player")

		{
			StartCoroutine(ActivateStone());
		
		}
	}
	IEnumerator ActivateStone()
	{

		anim.Play(animClip.name);
		yield return new WaitForSeconds(0.1f);
		stone.isKinematic = false;
	}
	private void OnDrawGizmos()
	{
		Gizmos.DrawIcon(stone.transform.position, "fallingStone", true);
		Gizmos.DrawIcon(transform.position, "fallingBranch", true);

	}
	public void RestartScript()
	{
		stone.velocity = Vector2.zero;
		stone.isKinematic = true;
		stone.transform.position = defStonePos;
	}
}
