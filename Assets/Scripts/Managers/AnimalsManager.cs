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
    enum AnimalForm { None, Human, Eagle };

    [SerializeField] private GameObject _humanForm;
    //[SerializeField] private GameObject[] _equippedAnimalsPrefabs = new GameObject[2];
    [SerializeField] private Transform _playerSpawnObject;
    private Vector3 _playerSpawnPosition;

    private GameObject _currentPlayerForm;
    private Vector3 _positionBeforeDestroy;

    private AnimalForm _currentAnimalIdentifier;

    public GameObject Particle;
    GameObject _particle;
    bool _runParticle=false;

    [System.Serializable]
    public class FormPrefabs
    {
        public Color color;
        public Sprite sprite;
        public string title;
        public GameObject _animalsPrefabs;
    }

    public FormPrefabs[] _equippedAnimalsPrefabs;
    
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
            chosenAnimalPrefab = _equippedAnimalsPrefabs[index]._animalsPrefabs;
        if (chosenAnimalPrefab != null)
        {
            AnimalForm chosenAnimalForm = (AnimalForm)Enum.Parse(typeof(AnimalForm), chosenAnimalPrefab.name);
            if (_currentAnimalIdentifier == chosenAnimalForm) return;

            _particle = Instantiate(Particle);
            _particle.transform.position = _currentPlayerForm.transform.position - new Vector3(0, 0.7f, -1f);
            _particle = Instantiate(Particle);
            _particle.transform.position = _currentPlayerForm.transform.position - new Vector3(0, 0.7f, 1f);
 
            DestroyCurrentForm();
            _currentPlayerForm = Instantiate(chosenAnimalPrefab, _positionBeforeDestroy, Quaternion.identity);
            _currentAnimalIdentifier = chosenAnimalForm;
        }
    }

    public Vector3 GetPosition() {
        return _currentPlayerForm.transform.position;
    }

    private void Update()
    {
        if (Input.GetButtonDown("MenuHero"))
        {
            RadialMenuSpawner.Instance.SpawnMenu(_equippedAnimalsPrefabs);
        }

        if (_runParticle) {
            _particle.transform.position = _currentPlayerForm.transform.position - new Vector3(0, 1f, 0);
        }

    }
    // Global functions

}