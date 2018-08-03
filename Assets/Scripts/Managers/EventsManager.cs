using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventsManager : Singleton<EventsManager>
{
    public delegate void onGameStart();
    public onGameStart gameStartDelegate = delegate { };

    public delegate void onPlayerDeath();
    public onPlayerDeath playerDeathDelegate = delegate { };

    public delegate void onFormChange();
    public onFormChange formChangeDelegate = delegate { };

	public delegate void onCollectibleCountChange();
	public onCollectibleCountChange collectibleChangeDelegate = delegate { };

	public delegate void OpenPauseMenuDelegate();
	public OpenPauseMenuDelegate openPauseMenuDelegate = delegate { };
}
