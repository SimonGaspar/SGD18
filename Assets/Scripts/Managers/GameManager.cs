using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine.SceneManagement;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    [SerializeField] private string _saveString = "/gameSave.dat";
    [SerializeField] private int _numberOfAnimals = 2;
    [SerializeField] [Tooltip("Maximum number of collectibles for each animal")] private int[] _maximumCollectibles;
    [Space]
    [Header("Collectibles")]
    [SerializeField] private GameObject[] _collectiblesPrefabs;
    [Space]
    [Header("Collectible spawns on each level")]
    [SerializeField] private SpawnsOnLevel[] _spawnsOnLevel;

    public int[] CollectiblesCount { get { return _collectiblesCount; } }
    public int[] MaxCollectiblesCount { get { return _maximumCollectibles; } }

    private int[] _collectiblesCount;

    private void Start()
    {
        _collectiblesCount = new int[_numberOfAnimals];
        Debug.Log(SceneManager.GetActiveScene().name);
        SpawnCollectiblesInCurrentLevel();
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
    // Collectibles
    public void SpawnCollectiblesInCurrentLevel()
    {
        int foundIndex = -1;
        for (int i = 0; i < _spawnsOnLevel.Length; i++)
        {
            if (_spawnsOnLevel[i].LevelName == SceneManager.GetActiveScene().name)
            {
                foundIndex = i;
            }
        }
        if (foundIndex == -1) return;

        CollectibleSpawn[] spawns = _spawnsOnLevel[foundIndex].CollectibleSpawns;

        foreach (CollectibleSpawn s in spawns)
        {
            GameObject obj = Instantiate(_collectiblesPrefabs[(int)s.Type]);
            obj.transform.position = s.Transform.position;
        }

    }

    public void CollectiblePickup(Collectible collectible, CollectibleType type)
    {
        Debug.Log("Picked up: " + type);
        _collectiblesCount[(int)type]++;
        EventsManager.Instance.collectibleChangeDelegate();
    }
}

[System.Serializable]
class SpawnsOnLevel
{
    [Tooltip("Level name has to be exactly the same as Scene name")] public string LevelName;
    public CollectibleSpawn[] CollectibleSpawns;
}

[System.Serializable]
class CollectibleSpawn
{
    [Tooltip("Transform of desired spawn")] public Transform Transform;
    [Tooltip("Type of collectible to be spawned")] public CollectibleType Type;
}

[System.Serializable]
class Save
{
    public string foo;
}