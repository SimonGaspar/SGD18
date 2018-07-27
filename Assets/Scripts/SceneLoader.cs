using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.Events;
using UnityEngine;

public class SceneLoader : MonoBehaviour
{

    public Animator sceneLoaderAnimator;
    UnityAction<GameObject> _eventToInvoke;

    public void FadeToLevel(UnityAction<GameObject> eventTo)
    {
        _eventToInvoke = eventTo;
        sceneLoaderAnimator.SetTrigger("TriggerLeavesOUT");
    }

    public void onFadeComplete()
    {
        _eventToInvoke.Invoke(gameObject);
    }
}
