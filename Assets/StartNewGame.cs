using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartNewGame : MonoBehaviour {




	public void StartGame()
	{
		GameManager.Instance.NewGame();
	}
}
