using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadialMenuSpawner : Singleton<RadialMenuSpawner> {
    
    public GameObject menuPrefab;
    private GameObject newMenu;
    private RadialMenu script;
    public GameObject camObject;


    public void SpawnMenu(AnimalsManager.FormPrefabs[] obj) {
        newMenu = Instantiate(menuPrefab);
        newMenu.transform.SetParent(transform, false);

        
        newMenu.transform.position = camObject.GetComponent<Camera>().WorldToScreenPoint(AnimalsManager.Instance.GetPosition());

        script = newMenu.GetComponent<RadialMenu>();
        script.SpawnButtons(obj);
    }

    public void SpawnMenu(Object obj) { 
       
    }

    private void Update()
    {
        if(Input.GetButton("MenuHero") && newMenu != null) {
        newMenu.transform.position = camObject.GetComponent<Camera>().WorldToScreenPoint(AnimalsManager.Instance.GetPosition());
        }
        Debug.Log(SettingsManager.Instance.GAME_RESOLUTION);
    }
}
