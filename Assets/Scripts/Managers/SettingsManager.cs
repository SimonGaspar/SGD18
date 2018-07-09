using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * To work correctly, this Script has to be attached to game object in current scene !
 */

public class SettingsManager : Singleton<SettingsManager>
{
    // Settings accessible globally
    public readonly float GAME_RESOLUTION = 16f / 9f;
}
