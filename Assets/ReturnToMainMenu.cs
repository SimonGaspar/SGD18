using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReturnToMainMenu : MonoBehaviour
{

	public void ReturnToMainMenuFromCredits()
	{
		GameManager.Instance.LoadLevel(0, false);
	}
	private void Update()
	{
		if (Input.GetKeyUp(KeyCode.Escape))
			GameManager.Instance.LoadLevel(0, false);
	}
}
