using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class openMenu : MonoBehaviour
{


	[SerializeField] [Tooltip("Name of the object that controls InGame menu")] private string _ingameMenuControl = "InGameMenuControl";
	[SerializeField] private AnimalForm _collectibleMenuFor;
	private bool _claimed = false;
	InGameMenuControl inGameMenuControl;

	private void Awake()
	{
		_claimed = false;
	}
	private void Start()
	{


		GameObject obj = GameObject.Find(_ingameMenuControl);
		inGameMenuControl = obj.GetComponent<InGameMenuControl>();
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (_claimed)
			return;
		if (collision.tag == "Player")
		{

			UIManager.Instance.OpenInGameMenu();

			//open at that colletible menu for
			//because there is a none animal we newed to pass minus one 

			inGameMenuControl.HandleLeftButtonClick((int)_collectibleMenuFor - 1);
			_claimed = true;
		}
	}
}


