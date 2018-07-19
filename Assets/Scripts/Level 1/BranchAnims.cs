using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BranchAnims : MonoBehaviour
{

	[SerializeField] public float animDelay = 1f;
	Animator anim;

	[SerializeField] AnimationClip animClip;
	// Use this for initialization
	void Start()
	{
		anim = GetComponent<Animator>();
	}
	
	IEnumerator PlayAnim(float value)
	{
		yield return new WaitForSeconds(value);

		anim.Play(animClip.name);
	}
	private void OnDrawGizmos()
	{
		Gizmos.DrawIcon(transform.transform.position, "fallingBranch", true);
	}
	public void ResetScript()
	{
		anim.Play("Empty");
	}
	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.gameObject.layer == 10)
		{
			StartCoroutine(PlayAnim(animDelay));
		}
	}
	//only for debug purposes; safe to delete
	private void OnCollisionStay2D(Collision2D collision)
	{
		//StartCoroutine(Wait()); 
	}
	private void OnTriggerStay2D(Collider2D collision)
	{
		//StartCoroutine(Wait());
		
	}
	
	/*IEnumerator Wait()
	{
		if (Input.GetKeyDown(KeyCode.R))
		{

			yield return new WaitForSeconds(2f);
			ResetScript();
		}
	}*/
}
