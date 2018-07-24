using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
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

    [Header("Collectible panel")]
    [SerializeField] private Image[] _collectiblesImages;


    private bool _inPlayMenu = false;
    private Animator _mainMenuAnimator;

    private void Start()
    {
        Assert.IsNotNull(_canvas);
        // Assert.IsNotNull(_buttonPrefab);
        // Assert.IsNotNull(_panelPrefab);

        EventsManager.Instance.collectibleChangeDelegate += collectibleCountChanged;

        _mainMenuAnimator = GameObject.Find("MainMenu").GetComponent<Animator>();

        SpawnSideButtons();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            MainMenuGoBack();
        }
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

    // Modal

    public void UseModal(string titleText, string buttonText1, UnityAction buttonEvent1, string buttonText2, UnityAction buttonEvent2)
    {
        _modalScript.SetUpModal(titleText, buttonText1, buttonEvent1, buttonText2, buttonEvent2);
    }

    // Collectibles
    public void collectibleCountChanged()
    {
        int[] maxCollectibles = GameManager.Instance.MaxCollectiblesCount;
        int[] countCollectibles = GameManager.Instance.CollectiblesCount;

        for (int i = 0; i < _collectiblesImages.Length; i++)
        {
            if (countCollectibles[i] == 0) return;

            Image img = _collectiblesImages[i];

            Color temp = img.color;
            temp.a = (float)countCollectibles[i] / (float)maxCollectibles[i];
            img.color = temp;
        }
    }

    public void MainMenuPlay()
    {
        if (!_inPlayMenu)
        {
            _inPlayMenu = true;
            _mainMenuAnimator.ResetTrigger("GoBackClicked");
            _mainMenuAnimator.SetTrigger("PlayClicked");
        }
    }

    public void MainMenuGoBack()
    {
        if (_inPlayMenu)
        {
            _inPlayMenu = false;
            _mainMenuAnimator.ResetTrigger("PlayClicked");
            _mainMenuAnimator.SetTrigger("GoBackClicked");
        }
    }
}
