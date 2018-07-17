using System.Collections;
using System.Collections.Generic;
using UnityEngine.Assertions;
using UnityEngine.UI;
using UnityEngine;

public class UIPanelScript : MonoBehaviour
{
    [Space]
    [Header("Content panel settings")]
    [SerializeField] private GameObject _contentPanelPrefab;
    [Space]
    [Header("Side panel settings")]
    [SerializeField] private GameObject _sidePanel;
    [SerializeField] private GameObject _sidePanelButtonPrefab;
    [SerializeField] private Vector2 _buttonStartPosition = new Vector2(0f, -125f);
    [SerializeField] private Vector2 _buttonOffsetPosition = new Vector2(0f, -175f);

    private Animal[] _animals;
    private GameObject _currentContentPanel;

    private void Start()
    {
        Assert.IsNotNull(_sidePanel);
        Assert.IsNotNull(_contentPanelPrefab);
        Assert.IsNotNull(_sidePanelButtonPrefab);
    }

    public void InitializePanel(Animal[] animals, Animal currentAnimal)
    {
        _animals = animals;

        SpawnSidePanelButtons();

        _currentContentPanel = Instantiate(_contentPanelPrefab);
        _currentContentPanel.transform.SetParent(transform, false);

        _currentContentPanel.GetComponent<ContentPanelScript>().InitializeContentPanel(currentAnimal);
    }

    public void SpawnSidePanelButtons()
    {
        foreach (Animal a in _animals)
        {
            GameObject b = Instantiate(_sidePanelButtonPrefab);

            b.GetComponent<Image>().sprite = a.AnimalSprite;
            b.GetComponent<Button>().onClick.AddListener(delegate { HandleButtonClick(a); });
            b.transform.SetParent(_sidePanel.transform, false);
        }
    }
    public void HandleButtonClick(Animal animal)
    {
        if (_currentContentPanel != null) Destroy(_currentContentPanel.gameObject);
        _currentContentPanel = Instantiate(_contentPanelPrefab);
        _currentContentPanel.transform.SetParent(transform, false);

        _currentContentPanel.GetComponent<ContentPanelScript>().InitializeContentPanel(animal);
    }

    public void ClosePanel()
    {
        Destroy(gameObject);
    }

}
