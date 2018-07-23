using System.Collections;
using System.Collections.Generic;
using UnityEngine.Assertions;
using UnityEngine.UI;
using UnityEngine;

public class ContentPanelScript : MonoBehaviour
{
    [SerializeField] private Image _titleImage;
    [SerializeField] private Text _titleText;

    private Image _panelImage;

    private void Start()
    {
        Assert.IsNotNull(_titleImage);
        Assert.IsNotNull(_titleText);
    }

    public void InitializeContentPanel(Animal animal)
    {
        _titleText.text = animal.Name;
        _titleImage.sprite = animal.AnimalSprite;
    }

    public void ClosePanel()
    {
        Destroy(gameObject);
    }
}
