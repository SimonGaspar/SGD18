using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Assertions;
using UnityEngine.UI;
using UnityEngine;

/*
 * To work correctly, this Script has to be attached to game object in current scene !
 */

public class AnimalsManager : Singleton<AnimalsManager>
{
    // Variables
    enum AnimalForm { None, Human, Eagle, Bison };

    [SerializeField] private GameObject _humanFormHolder;
    [SerializeField] private GameObject[] _equippedAnimalsPrefabs = new GameObject[2];
    private Vector3 _playerSpawnPosition;

    private GameObject _currentPlayerForm;
    private Vector3 _positionBeforeDestroy;

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
            _currentPlayerForm = null;
            _currentAnimalIdentifier = AnimalForm.None;
        }
    }
    public void SpawnHuman()
    {
        if (PreventSwapForm()) return;
        if (_currentAnimalIdentifier == AnimalForm.Human) return;
        DestroyCurrentForm();
        _currentPlayerForm = Instantiate(_humanFormHolder, _positionBeforeDestroy, Quaternion.identity);
        _currentAnimalIdentifier = AnimalForm.Human;
        EventsManager.Instance.formChangeDelegate();
    }

    public void SwapToAnimalNumber(int index)
    {
        if (PreventSwapForm()) return;
        GameObject chosenAnimalPrefab = null;
        if (index < _equippedAnimalsPrefabs.Length)
            chosenAnimalPrefab = _equippedAnimalsPrefabs[index];
        if (chosenAnimalPrefab != null)
        {
            AnimalForm chosenAnimalForm = (AnimalForm)Enum.Parse(typeof(AnimalForm), chosenAnimalPrefab.name);
            if (_currentAnimalIdentifier == chosenAnimalForm) return;
            DestroyCurrentForm();
            _currentPlayerForm = Instantiate(chosenAnimalPrefab, _positionBeforeDestroy, Quaternion.identity);
            _currentAnimalIdentifier = chosenAnimalForm;
            EventsManager.Instance.formChangeDelegate();
        }
    }

    private bool PreventSwapForm()
    {
        return _currentPlayerForm != null && _currentPlayerForm.GetComponent<PlayerController>().IsMoving;
    }

    public Transform GetCurrentAnimalTransformComponent()
    {
        return _currentPlayerForm.GetComponent<Transform>();
    }

    private void Update()
    {

    }
    // Global functions

}