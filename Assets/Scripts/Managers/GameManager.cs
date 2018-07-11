using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{

    public void loadScene(string sceneName)
    {
        Application.LoadLevel(sceneName);

        EventsManager.Instance.playerDeathDelegate();
    }

}