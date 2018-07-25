using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine.Assertions;
using UnityEngine.UI;
using UnityEngine;

public class UIManager : Singleton<UIManager>
{
    [Space]
    [Header("Collectibles panel")]
    [SerializeField] private GameObject _collectibleImagePrefab;

    private Transform _collectiblesPanelTransform;

    private bool _inPlayMenu = false;
    private Animator _mainMenuAnimator = null;

    private List<GameObject> _collectibleImages;

    private void Start()
    {
        // Assert.IsNotNull(_buttonPrefab);
        if (GameObject.Find("MainMenu"))
        {
            _mainMenuAnimator = GameObject.Find("MainMenu").GetComponent<Animator>();
        }

        _collectiblesPanelTransform = GameObject.Find("CollectiblesPanel").transform;

        Assert.IsNotNull(_collectibleImagePrefab);
        Assert.IsNotNull(_collectiblesPanelTransform);

        EventsManager.Instance.collectibleChangeDelegate += collectibleCountChanged;

        _collectibleImages = new List<GameObject>();

        InitializeTopLeftPanel();
        EventsManager.Instance.collectibleChangeDelegate();
    }

    public void InitializeTopLeftPanel()
    {
        Animal[] animals = GameManager.Instance.AvailableAnimals;
        foreach (Animal a in animals)
        {
            GameObject obj = Instantiate(_collectibleImagePrefab);
            obj.transform.SetParent(_collectiblesPanelTransform, false);

            Image i = obj.GetComponent<Image>();
            i.color = a.AnimalColor;

            _collectibleImages.Add(obj);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            MainMenuGoBack();
        }
    }

    // private void SpawnSideButtons()
    // {
    //     Vector3 buttonPosition = Vector3.zero;
    //     buttonPosition = new Vector3(_startPosition.x, _startPosition.y, 0f);

    //     foreach (Animal a in _animals)
    //     {
    //         GameObject b = Instantiate(_buttonPrefab);

    //         b.GetComponent<Image>().sprite = a.AnimalSprite;
    //         b.GetComponent<Button>().onClick.AddListener(delegate { HandleButtonClick(a); });
    //         b.transform.position = buttonPosition;
    //         b.transform.SetParent(_canvas.transform, false);

    //         buttonPosition += new Vector3(_offsetEachButton.x, _offsetEachButton.y, 0f);
    //     }
    // }

    // private void HandleButtonClick(Animal animal)
    // {
    //     if (_currentlyOpenedPanel != null) Destroy(_currentlyOpenedPanel);

    //     GameObject panel = Instantiate(_panelPrefab);
    //     panel.transform.SetParent(_canvas.transform, false);
    //     panel.GetComponent<UIPanelScript>().InitializePanel(_animals, animal);
    //     _currentlyOpenedPanel = panel;
    // }

    // // Modal

    // public void UseModal(string titleText, string buttonText1, UnityAction buttonEvent1, string buttonText2, UnityAction buttonEvent2)
    // {
    //     _modalScript.SetUpModal(titleText, buttonText1, buttonEvent1, buttonText2, buttonEvent2);
    // }

    // Collectibles
    public void collectibleCountChanged()
    {
        int[] collectibleCounts = GameManager.Instance.CollectiblesCount;
        Animal[] animals = GameManager.Instance.AvailableAnimals;
        for (int i = 0; i < _collectibleImages.Count; i++)
        {
            Image im = _collectibleImages[i].GetComponent<Image>();
            Color temp = im.color;
            if (animals[i].RequiredParts == 0) temp.a = 1f;
            else temp.a = (float)collectibleCounts[i] / (float)animals[i].AnimalSprites.Length;

            im.color = temp;

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
