﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.Events;
using UnityEngine.Assertions;
using UnityEngine.UI;
using UnityEngine;

public class UIManager : Singleton<UIManager>
{
    [Space]
    [Header("Collectibles panel")]
    [SerializeField] private GameObject _collectibleImagePrefab;

    [Space]
    [Header("MainMenu")]
    [SerializeField] [Tooltip("Name of the scene that contains MainMenu")] private string _mainMenuScene = "MainMenu";
    [Space]
    [Header("InGame menu")]
    [SerializeField] [Tooltip("BuildIndex of a scene that contains Ingame menu")] private int _ingameMenuSceneNumber = 1;
    [SerializeField] [Tooltip("Name of the object that controls InGame menu")] private string _ingameMenuControl = "InGameMenuControl";

    private Transform _collectiblesPanelTransform;

    private bool _inMainMenu = false;
    private bool _inPlayMenu = false;
    private bool _ingameMenuOpened = false;

    private Animator _mainMenuAnimator = null;

    private void Start()
    {
        print("UI start");
        // Assert.IsNotNull(_buttonPrefab);
        if (GameObject.Find("MainMenuCanvas"))
        {
            _inMainMenu = true;
            _mainMenuAnimator = GameObject.Find("MainMenu").GetComponent<Animator>();
        }

        Assert.IsNotNull(_collectibleImagePrefab);
        Assert.IsNotNull(_collectiblesPanelTransform);

        SceneManager.sceneLoaded += OnNewSceneLoaded;
    }

    void OnNewSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        print("New scene loaded");
        if (SceneManager.GetActiveScene().name != _mainMenuScene && !SceneManager.GetSceneByBuildIndex(_ingameMenuSceneNumber).isLoaded)
        {
            print("Adding ingame ui");
            SceneManager.LoadScene(_ingameMenuSceneNumber, LoadSceneMode.Additive);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (SceneManager.GetActiveScene().name == _mainMenuScene)
            {
                MainMenuGoBack();
            }
            else if (SceneManager.GetSceneByBuildIndex(_ingameMenuSceneNumber).isLoaded)
            {
                GameObject obj = GameObject.Find(_ingameMenuControl);
                if (obj.GetComponent<InGameMenuControl>().IsOpened) CloseIngameMenu();
                else OpenInGameMenu();
            }

        }
    }

    public void OpenInGameMenu()
    {
        GameObject obj = GameObject.Find(_ingameMenuControl);

        if (obj != null)
        {
            obj.GetComponent<InGameMenuControl>().OpenInGameMenu();
        }

    }

    public void CloseIngameMenu()
    {
        GameObject obj = GameObject.Find(_ingameMenuControl);

        if (obj != null)
        {
            obj.GetComponent<InGameMenuControl>().CloseInGameMenu();
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
