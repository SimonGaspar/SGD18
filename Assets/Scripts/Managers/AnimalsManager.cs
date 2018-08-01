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
        _currentPlayerForm = Instantiate(_humanFormHolder, _positionBeforeDestroy, Quaternion.identity);
        _currentAnimalIdentifier = AnimalForm.Human;
        EventsManager.Instance.formChangeDelegate();
    }

    public void SwapToAnimalNumber(int index)
    {
        if (GameManager.Instance.IsAnimalAvailable(index)) StartCoroutine(Swap(index));
    }

    public IEnumerator Swap(int index)
    {
        if (PreventSwapForm()) yield return null;
        GameObject chosenAnimalPrefab = null;
        if (index < _equippedAnimalsPrefabs.Length)
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

    private void Update()
    {

    }

    // Global functions

}