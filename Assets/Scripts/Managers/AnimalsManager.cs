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

    private void Start()
    {
        Assert.IsNotNull(_player);
        _playerController = _player.GetComponent<PlayerController>();
        _playerController.Animal = _humanForm;
        _playerController.UpdateStats();
    }

    public void SwapToAnimalNumber(int animalIndex)
    {
        Debug.Log("Swapping to " + animalIndex);
        if (animalIndex < _animals.Length && _animals[animalIndex] != null)
        {
            _playerController.Animal = _animals[animalIndex];
            _playerController.UpdateStats();
        }
    }

    public void SwapToHuman()
    {
        _playerController.Animal = _humanForm;
        _playerController.UpdateStats();
    }
    private void Update()
    {

    }
    // Variables
    [SerializeField] private GameObject _player;
    private PlayerController _playerController;
    [SerializeField] private Animal _humanForm;
    [SerializeField] private Animal[] _animals = new Animal[2];
    [SerializeField] private Animal _currentAnimal;
    // Global functions

}