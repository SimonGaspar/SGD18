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
    [SerializeField] private Vector2 _startPosition = new Vector2(39f, -105f);
    [SerializeField] private Vector2 _offsetEachButton = new Vector2(0f, 205);
    [SerializeField] private Animal[] _animals;
    [SerializeField] private ModalWindowScript _modalScript;

    [Header("Panel")]
    [SerializeField] private GameObject _panelPrefab;
    private GameObject _currentlyOpenedPanel;

    private void Start()
    {
        Assert.IsNotNull(_canvas);
        Assert.IsNotNull(_buttonPrefab);
        Assert.IsNotNull(_panelPrefab);

        SpawnMainMenu();
        SpawnSideButtons();
    }
    private void SpawnMainMenu()
    {

    }

    private void SpawnSideButtons()
    {
        Vector3 buttonPosition = Vector3.zero;
        buttonPosition = new Vector3(_startPosition.x, _startPosition.y, 0f);

        foreach (Animal a in _animals)
        {
            GameObject b = Instantiate(_buttonPrefab);

            b.GetComponent<Image>().sprite = a.AnimalSprite;
            b.GetComponent<Button>().onClick.AddListener(delegate { HandleButtonClick(a); });
            b.transform.position = buttonPosition;
            b.transform.SetParent(_canvas.transform, false);

            buttonPosition += new Vector3(_offsetEachButton.x, _offsetEachButton.y, 0f);
        }
    }

    private void HandleButtonClick(Animal animal)
    {
        if (_currentlyOpenedPanel != null) Destroy(_currentlyOpenedPanel);

        GameObject panel = Instantiate(_panelPrefab);
        panel.transform.SetParent(_canvas.transform, false);
        panel.GetComponent<UIPanelScript>().InitializePanel(_animals, animal);
        _currentlyOpenedPanel = panel;
    }

    public void ShowModal()
    {
        _modalScript.SetUpModal("Hello there", "YES", delegate { _modalScript.HideModal(); }, "NO", delegate { _modalScript.HideModal(); });
    }
}
