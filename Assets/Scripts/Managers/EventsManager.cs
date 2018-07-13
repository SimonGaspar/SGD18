using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventsManager : Singleton<EventsManager>
{
    public delegate void onGameStart();
    public onGameStart gameStartDelegate;

    public delegate void onPlayerDeath();
    public onPlayerDeath playerDeathDelegate;

    public delegate void onFormChange();
    public onFormChange formChangeDelegate;
}
