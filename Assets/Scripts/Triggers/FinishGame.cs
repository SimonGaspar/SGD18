using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishGame : MonoBehaviour {

	[SerializeField] private bool _claimed = false;
	public bool Claimed { get { return _claimed; } set { _claimed = value; } }

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (!_claimed && other.gameObject.tag == "Player")
		{
			GameManager.Instance.LoadLevel(3,false);

			GameManager.Instance.DeleteSavedGame();
			_claimed = true;
		}
	}
}
