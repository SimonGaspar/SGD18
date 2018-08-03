
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Assertions;
using UnityEngine.UI;
using UnityEngine;

/*
 * To work correctly, this Script has to be attached to game object in current scene !
 */
public enum AnimalForm { None, Human, Bison, Eagle };

public class AnimalsManager : Singleton<AnimalsManager>
{

	[SerializeField] private GameObject _humanFormHolder;
	[SerializeField] private GameObject[] _equippedAnimalsPrefabs = new GameObject[3];
	[SerializeField] private Transform _playerSpawnObject;
	private Vector3 _playerSpawnPosition;

	private GameObject _currentPlayerForm;
	private Vector3 _positionBeforeDestroy;
	//used to maintain scale between spawns
	private Vector3 _currentScale = new Vector3(0.21f, 0.2f, 1f);
	private AnimalForm _currentAnimalIdentifier;

	private void Start()
	{
		Assert.IsNotNull(_humanFormHolder);
	}
	public void ResetSpawn()
	{
		DestroyCurrentForm();
		_positionBeforeDestroy = GameManager.Instance.PlayerSpawn;
		_currentAnimalIdentifier = AnimalForm.None;
		SpawnHuman();
	}

	public void DestroyCurrentForm()
	{
		if (_currentPlayerForm != null)
		{
			_positionBeforeDestroy = _currentPlayerForm.transform.position;
			Destroy(_currentPlayerForm);
		}
	}
	public void SpawnHuman()
	{
		if (PreventSwapForm()) return;
		if (_currentAnimalIdentifier == AnimalForm.Human) return;
		DestroyCurrentForm();

		//used to maintain scale between spawns
		if (_currentPlayerForm != null)
			_currentScale = _currentPlayerForm.GetComponent<PlayerController>().transform.localScale;


		_currentPlayerForm = Instantiate(_humanFormHolder, _positionBeforeDestroy, Quaternion.identity);

		//used to maintain scale between spawns
		if (_currentPlayerForm != null)
			_currentPlayerForm.GetComponent<PlayerController>().transform.localScale = _currentScale;
		_currentAnimalIdentifier = AnimalForm.Human;



		EventsManager.Instance.formChangeDelegate();
	}

	public void SwapToAnimalNumber(int index)
	{
		StartCoroutine(Swap(index));
		//used to maintain scale between spawns
		_currentScale = _currentPlayerForm.GetComponent<PlayerController>().transform.localScale;
	}

	public IEnumerator Swap(int index)
	{

		if (index != 0)
		{
			if (PreventSwapForm() || !GameManager.Instance.IsAnimalAvailable(index + 1)) yield break;
		}
		else
		{
			Debug.Log("else");
			if (PreventSwapForm())  yield break;
		}
		GameObject chosenAnimalPrefab = null;
		if (index < _equippedAnimalsPrefabs.Length)
			Debug.Log(_equippedAnimalsPrefabs[index]);
			chosenAnimalPrefab = _equippedAnimalsPrefabs[index];
		if (chosenAnimalPrefab != null)
		{
			AnimalForm chosenAnimalForm = (AnimalForm)Enum.Parse(typeof(AnimalForm), chosenAnimalPrefab.name);
			if (_currentAnimalIdentifier == chosenAnimalForm) yield return null;

			if (chosenAnimalForm == AnimalForm.Bison)
				_currentPlayerForm.GetComponent<PlayerController>().SetTransformBison(0);
			else
				if (_currentPlayerForm.name.Contains(AnimalForm.Bison.ToString()))
				_currentPlayerForm.GetComponent<PlayerController>().SetTransformBison(1);
			else
				_currentPlayerForm.GetComponent<PlayerController>().SetTransformBison(10);
			_currentPlayerForm.GetComponent<Animator>().SetTrigger("FadeOut");


			yield return new WaitForSeconds(2.5f / 4f);
			var x = Instantiate(chosenAnimalPrefab, _currentPlayerForm.transform.position, Quaternion.identity);


			DestroyCurrentForm();
			_currentPlayerForm = x;

			//used to maintain scale between spawns
			_currentPlayerForm.GetComponent<PlayerController>().transform.localScale = _currentScale;
			EventsManager.Instance.formChangeDelegate();
			yield return new WaitForSeconds(1 / 4f);

			_currentAnimalIdentifier = chosenAnimalForm;
		}
		yield return null;
	}

	private bool PreventSwapForm()
	{
		return _currentPlayerForm != null && _currentPlayerForm.GetComponent<PlayerController>().IsMoving;
	}

	public Transform GetCurrentAnimalTransformComponent()
	{
		return _currentPlayerForm.GetComponent<Transform>();
	}
	public PlayerController GetCurrentPlayerController()
	{
		return _currentPlayerForm.GetComponent<PlayerController>();
	}
	private void Update()
	{
		GetInput();
	}
	void GetInput()
	{
		if (Input.GetKeyUp(KeyCode.I))//human
			SwapToAnimalNumber(0);
		if (Input.GetKeyUp(KeyCode.O))//Bison
			SwapToAnimalNumber(1);
		if (Input.GetKeyUp(KeyCode.P))//Eagle
			SwapToAnimalNumber(2);
	}
	// Global functions

}