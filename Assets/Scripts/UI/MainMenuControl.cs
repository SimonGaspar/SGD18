using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuControl : MonoBehaviour
{

    [SerializeField] private Animator _mainMenuAnimator;

    private void Start()
    {

    }

    public void Play()
    {
        GameManager.Instance.StartGame();
    }

    public void QuitGame()
    {
        GameManager.Instance.QuitGame();
    }

    public void Continue()
    {
        GameManager.Instance.Continue();
    }

    public void NewGame()
    {
        GameManager.Instance.NewGame();
    }
	public void Credits(int index)
	{
		GameManager.Instance.Credits(index);
	}
}
