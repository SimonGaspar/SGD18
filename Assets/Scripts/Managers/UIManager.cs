using System.Collections;
using System.Collections.Generic;
using UnityEngine.Assertions;
using UnityEngine.UI;
using UnityEngine;

public class UIManager : Singleton<UIManager>
{
    [SerializeField] private Canvas _canvas;
    [Space]
    [Header("Top left UI")]
    [SerializeField] private GameObject _buttonPrefab;
    [SerializeField] private Vector2 _offset = new Vector2(39f, -79f);
    [SerializeField] private Animal[] _animals;

    [Header("Panel")]
    [SerializeField] private GameObject _panelPrefab;
    private GameObject _currentlyOpenedPanel;

    private void Start()
    {
        Assert.IsNotNull(_canvas);
        Assert.IsNotNull(_buttonPrefab);
        Assert.IsNotNull(_panelPrefab);

        SpawnButtons();
    }

    private void SpawnButtons()
    {
        Vector3 buttonPosition = Vector3.zero;
        buttonPosition.x = _offset.x;
        buttonPosition.y = -38;
        foreach (Animal a in _animals)
        {
            GameObject b = Instantiate(_buttonPrefab);

            b.GetComponent<Image>().sprite = a.AnimalSprite;
            b.GetComponent<Button>().onClick.AddListener(delegate { HandleClick(a); });
            b.transform.position = buttonPosition;
            b.transform.SetParent(_canvas.transform, false);

            buttonPosition.y += _offset.y;
        }
    }

    private void HandleClick(Animal a)
    {
        if (_currentlyOpenedPanel != null) Destroy(_currentlyOpenedPanel);

        GameObject panel = Instantiate(_panelPrefab);
        panel.transform.SetParent(_canvas.transform, false);
        panel.GetComponent<UIPanelScript>().InitializePanel(a);
        _currentlyOpenedPanel = panel;
    }
}
