using System.Collections;
using System.Collections.Generic;
using UnityEngine.Assertions;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine;

public class ModalWindowScript : MonoBehaviour
{
    [SerializeField] private Text _titleText;
    [SerializeField] private Button _buttonChoice1;
    [SerializeField] private Button _buttonChoice2;

    private void Start()
    {
        Assert.IsNotNull(_titleText);
        Assert.IsNotNull(_buttonChoice1);
        Assert.IsNotNull(_buttonChoice2);
        HideModal();
    }

    public void SetUpModal(string titleText, string buttonText1, UnityAction buttonEvent1, string buttonText2, UnityAction buttonEvent2)
    {
        _titleText.text = titleText;



        _buttonChoice1.GetComponentInChildren<Text>().text = buttonText1;
        _buttonChoice1.onClick.RemoveAllListeners();
        _buttonChoice1.onClick.AddListener(buttonEvent1);

        _buttonChoice2.GetComponentInChildren<Text>().text = buttonText2;
        _buttonChoice2.onClick.RemoveAllListeners();
        _buttonChoice2.onClick.AddListener(buttonEvent1);

        ShowModal();
    }

    public void ShowModal()
    {
        gameObject.SetActive(true);
    }

    public void HideModal()
    {
        gameObject.SetActive(false);
    }
}
