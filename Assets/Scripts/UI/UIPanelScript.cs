using System.Collections;
using System.Collections.Generic;
using UnityEngine.Assertions;
using UnityEngine.UI;
using UnityEngine;

public class UIPanelScript : MonoBehaviour
{
    [SerializeField] private Image _titleImage;
    [SerializeField] private Text _titleText;

    private Image _panelImage;

    private void Start()
    {
        Assert.IsNotNull(_titleImage);
        Assert.IsNotNull(_titleText);
    }

    public void InitializePanel(Animal animal)
    {
        gameObject.SetActive(true);
    }

    public void ClosePanel()
    {
        Destroy(gameObject);
    }

}
