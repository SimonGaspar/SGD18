using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[System.Serializable]
public class MyButtonEvent : UnityEvent<int>
{
}

public class InGameMenuControl : MonoBehaviour
{
    [SerializeField] private CanvasGroup _canvasGroup;
    [SerializeField] private Button _exitButton;

    [Space]
    [Header("Left panel stuff")]
    [SerializeField] private Transform _leftPanelTransform;

    [Space]
    [Header("Button images")]
    [SerializeField] private GameObject _buttonPrefab;
    [SerializeField] private Sprite _buttonActive;
    [SerializeField] private Sprite _buttonClaimed;
    [SerializeField] private Sprite _buttonNotClaimed;
    [SerializeField] private Sprite _buttonNotAvailable;

    [Space]
    [Header("Content panel")]
    [SerializeField] private Image _titleImage;
    [SerializeField] private Text _keyBind;
    [SerializeField] private Text _contentText;


    [Space]
    [Header("Buttons stuff")]
    [SerializeField] private Animator _buttonsAnimator;
    [SerializeField] private string _questionText;
    [SerializeField] private Button _choiceYes;
    [SerializeField] private Button _choiceNo;

    [Space]
    [Header("Collectibles panel")]
    [SerializeField] private GameObject _collectibleImagePrefab;
    [SerializeField] private Transform _collectiblesPanelTransform;
    [SerializeField] private CanvasGroup _collectiblesCanvasGroup;

    public List<GameObject> _leftPanelButtons;
    public List<GameObject> _collectibleImages;

    private bool _isOpened = false;
    public bool IsOpened { get { return _isOpened; } }

    private Animal[] _animals;

    private Animal _currentAnimalContent;

    private void Start()
    {
        _collectibleImages = new List<GameObject>();
        _leftPanelButtons = new List<GameObject>();

        _animals = GameManager.Instance.AvailableAnimals;
        for (int i = 0; i < _animals.Length; i++)
        {
            GameObject temp = Instantiate(_buttonPrefab);

            temp.name = i.ToString();
            temp.transform.SetParent(_leftPanelTransform, false);

            temp.GetComponent<Image>().sprite = (GameManager.Instance.IsAnimalAvailable(i)) ? _buttonClaimed : _buttonNotClaimed;

            temp.GetComponent<Button>().onClick.AddListener(delegate { HandleLeftButtonClick(int.Parse(temp.name)); });

            _leftPanelButtons.Add(temp);
        }

        // inactive left buttons
        GameObject obj = Instantiate(_buttonPrefab);
        obj.transform.SetParent(_leftPanelTransform, false);
        obj.GetComponent<Image>().sprite = _buttonNotAvailable;
        obj = Instantiate(_buttonPrefab);
        obj.transform.SetParent(_leftPanelTransform, false);
        obj.GetComponent<Image>().sprite = _buttonNotAvailable;


        _currentAnimalContent = _animals[0]; // human

        InitializeTopLeftPanel();
        CloseInGameMenu();

        EventsManager.Instance.collectibleChangeDelegate += collectibleCountChanged;
    }

    private void OnDestroy()
    {
        EventsManager.Instance.collectibleChangeDelegate -= collectibleCountChanged;
    }

    public void InitializeTopLeftPanel()
    {
        Animal[] animals = GameManager.Instance.AvailableAnimals;
        foreach (Animal a in animals)
        {
            GameObject obj = Instantiate(_collectibleImagePrefab);
            obj.transform.SetParent(_collectiblesPanelTransform, false);

            _collectibleImages.Add(obj);
        }
        // init
        collectibleCountChanged();
    }

    public void collectibleCountChanged()
    {
        int[] collectibleCounts = GameManager.Instance.CollectiblesCount;
        Animal[] animals = GameManager.Instance.AvailableAnimals;

        for (int i = 0; i < _collectibleImages.Count; i++)
        {
            Image im = _collectibleImages[i].GetComponent<Image>();
            Sprite newSprite;

            if (animals[i].RequiredParts > 0)
                newSprite = animals[i].AnimalSprites[collectibleCounts[i]];
            else
                newSprite = animals[i].AnimalSprites[collectibleCounts[0]];

            im.GetComponent<RectTransform>().sizeDelta = new Vector2(newSprite.texture.width, newSprite.texture.height);
            im.sprite = newSprite;
        }
    }

    public void HandleLeftButtonClick(int number)
    {
        if (number < _animals.Length)
        {
            _currentAnimalContent = _animals[number];
            ReloadContent();
        }
    }

    public void ReloadContent()
    {
        _titleImage.sprite = _currentAnimalContent.TitleSprite;
        _keyBind.text = _currentAnimalContent.KeyBind;
        _contentText.text = _currentAnimalContent.ContentText;
    }

    public void OpenInGameMenu()
    {
        CloseTopLeftPanel();
        _canvasGroup.alpha = 1f;
        _canvasGroup.interactable = true;
        _isOpened = true;
    }

    public void CloseInGameMenu()
    {
        OpenTopLeftPanel();
        _canvasGroup.alpha = 0f;
        _canvasGroup.interactable = false;
        _isOpened = false;
    }

    public void OpenTopLeftPanel()
    {
        _collectiblesCanvasGroup.interactable = true;
        _collectiblesCanvasGroup.alpha = 1f;
    }

    public void CloseTopLeftPanel()
    {
        _collectiblesCanvasGroup.interactable = false;
        _collectiblesCanvasGroup.alpha = 0f;
    }

    public void QuitGame()
    {
        _buttonsAnimator.SetTrigger("ButtonClicked");
        GameManager.Instance.QuitGame();
    }

}
