using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    [SerializeField] private string _saveString = "/gameSave.dat";
    [SerializeField] private int _numberOfAnimals = 2;
    [SerializeField] [Tooltip("Maximum number of collectibles for each animal")] private int[] _maximumCollectibles;


    public int[] CollectiblesCount { get { return _collectiblesCount; } }
    public int[] MaxCollectiblesCount { get { return _maximumCollectibles; } }

    private int[] _collectiblesCount;

    private void Start()
    {
        _collectiblesCount = new int[_numberOfAnimals];
    }

    public void StartGame()
    {
        if (SaveExists())
        {
            UIManager.Instance.UseModal("Do you wish to continue from previous save ?", "Continue", delegate { Continue(); }, "New Game", delegate { NewGame(); });

        }
    }

    public void NewGame()
    {
        Debug.Log("New Game");
        File.Delete(Application.persistentDataPath + _saveString);
    }

    public void Continue()
    {
        Debug.Log("Continue");
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void SaveGame()
    {
        Save saveObject = new Save();

        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Open(Application.persistentDataPath + _saveString, FileMode.OpenOrCreate);
        saveObject.foo = "bar";
        bf.Serialize(file, saveObject);
        file.Close();
    }

    public void LoadGame()
    {
        if (File.Exists(Application.persistentDataPath + _saveString))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + _saveString, FileMode.Open);
            Save loadObject = (Save)bf.Deserialize(file);

            Debug.Log(loadObject.foo);
            file.Close();
        }
    }

    public bool SaveExists()
    {
        return File.Exists(Application.persistentDataPath + _saveString);
    }

    public void CollectiblePickup(CollectibleType type)
    {
        Debug.Log("Picked up: " + type);
        _collectiblesCount[(int)type]++;
        EventsManager.Instance.collectibleChangeDelegate();
    }
}

[System.Serializable]
class Save
{
    public string foo;
}