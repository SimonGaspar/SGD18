using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * To work correctly, this Script has to be attached to game object in current scene !
 */

public class SettingsManager : MonoBehaviour
{
    // Singleton stuff
    private static SettingsManager _instance;

    public static SettingsManager Instance { get { return _instance; } }

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            _instance = this;
        }
    }

    // Settings accessible globally
    public readonly float GAME_RESOLUTION = 16f / 9f;
}
