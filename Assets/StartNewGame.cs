using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartNewGame : MonoBehaviour
{
	Animator anim;
	int counter = 0;

	private void Start()
	{
		anim = GetComponent<Animator>();
		counter = 0;
	}
	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Escape))
			SkipInto();

	}


	public void StartGame()
	{
		GameManager.Instance.LoadLevel(2, true);
	}

	public void SkipInto()
	{
		counter++;
		anim.SetTrigger("Skip");
		if (counter >= 3)
			StartGame();
	}
}
