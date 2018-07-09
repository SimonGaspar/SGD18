using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Assertions;
using UnityEngine.UI;
using UnityEngine;

/*
 * To work correctly, this Script has to be attached to game object in current scene !
 */

public class AnimalsManager : MonoBehaviour
{
    // Singleton stuff
    private static AnimalsManager _instance;

    public static AnimalsManager Instance { get { return _instance; } }

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            _instance = this;
        }
    }
    // Variables
    enum AnimalForm { None, Human, Eagle };

    [SerializeField] private GameObject _humanForm;
    [SerializeField] private GameObject[] _equippedAnimalsPrefabs = new GameObject[2];
    [SerializeField] private Transform _playerSpawnObject;
    private Vector3 _playerSpawnPosition;

    private GameObject _currentPlayerForm;
    private Vector3 _positionBeforeDestroy;

    private AnimalForm _currentAnimalIdentifie = AnimalForm.None;

    private void Start()
    {
        Assert.IsNotNull(_humanForm);
        Assert.IsNotNull(_playerSpawnObject);

        _playerSpawnPosition = _playerSpawnObject.transform.position;
        _positionBeforeDestroy = _playerSpawnPosition;
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
        if (_currentAnimalIdentifier == AnimalForm.Human) return;
        DestroyCurrentForm();
        _currentPlayerForm = Instantiate(_humanForm, _positionBeforeDestroy, Quaternion.identity);
        _currentAnimalIdentifier = AnimalForm.Human;
    }

    public void SwapToAnimalNumber(int index)
    {
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
        }
    }
    private void Update()
    {

    }
    // Global functions

}