using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class DisableStone : MonoBehaviour
{

	public TunelRollingStone tunelRollingStone;
	[SerializeField] PhysicsMaterial2D physicsMat;


	private void Start()
	{
		Assert.IsNotNull(tunelRollingStone, "disableStone not assigned to TunnelRollingStone script!");
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.tag == "Player")
			return;
		tunelRollingStone.stone.GetComponent<Deathzone>().enabled = false;
		tunelRollingStone.stone.bodyType = RigidbodyType2D.Static;
		tunelRollingStone.childStone.sharedMaterial = physicsMat;
		return;
	}
}
