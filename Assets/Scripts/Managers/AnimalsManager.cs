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

    public ParticleSystem Particle;
    ParticleSystem _particle;

    SpriteRenderer rend;
    bool IsDestroyed=false;
    GameObject chosenAnimalPrefab;

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
        _particle = Instantiate<ParticleSystem>(Particle);


    }

    public void DestroyCurrentForm()
    {
        if (_currentPlayerForm != null)
        {
            IsDestroyed = false;
            rend = _currentPlayerForm.GetComponent<SpriteRenderer>();
            StartCoroutine("Destroying");
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
        chosenAnimalPrefab = null;
        if (index < _equippedAnimalsPrefabs.Length)
            chosenAnimalPrefab = _equippedAnimalsPrefabs[index]._animalsPrefabs;
        if (chosenAnimalPrefab != null)
        {
            AnimalForm chosenAnimalForm = (AnimalForm)Enum.Parse(typeof(AnimalForm), chosenAnimalPrefab.name);
            if (_currentAnimalIdentifier == chosenAnimalForm) return;
            _currentAnimalIdentifier = chosenAnimalForm;

            _particle.Simulate(0.0f, true, true);
            _particle.Play();
            
            DestroyCurrentForm();
        }
    }

    public Vector3 GetPosition() {
        return _currentPlayerForm.transform.position;
    }

    private IEnumerator Destroying()
    {
        for (float f = 1f; f >= 0.05; f -= 0.05f)
        {
            Color c = rend.material.color;
            c.a = f;
            rend.material.color = c;
            yield return new WaitForSeconds(0.05f);
        }

        if (IsDestroyed == false)
        {
            _positionBeforeDestroy = _currentPlayerForm.transform.position;
            Destroy(_currentPlayerForm);
            _currentPlayerForm = Instantiate(chosenAnimalPrefab, _positionBeforeDestroy, Quaternion.identity);
            
            rend = _currentPlayerForm.GetComponent<SpriteRenderer>();
            Color c = rend.material.color;
            c.a = 0f;
            rend.material.color = c;
            IsDestroyed = true;
        }
        
        for (float f = 0.05f; f <= 1; f += 0.05f)
        {
            Color c = rend.material.color;
            c.a = f;
            rend.material.color = c;
            yield return new WaitForSeconds(0.05f);
        }
    }
    

    private void Update()
    {
        if (Input.GetButtonDown("MenuHero"))
        {
            RadialMenuSpawner.Instance.SpawnMenu(_equippedAnimalsPrefabs);
        }

        _particle.transform.position = _currentPlayerForm.transform.position - new Vector3(0,0,0.1f);
    }
    // Global functions

}