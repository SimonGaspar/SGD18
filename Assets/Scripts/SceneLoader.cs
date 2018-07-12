using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class SceneLoader : MonoBehaviour
{

    public Animator sceneLoaderAnimator;
    private int _levelToLoad;

    public void FadeToLevel(int levelIndex)
    {
        _levelToLoad = levelIndex;
        sceneLoaderAnimator.SetTrigger("fadeOut");
    }

    public void onFadeComplete()
    {
        SceneManager.LoadScene(_levelToLoad);
    }
}
