using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine.SceneManagement;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    private Vector3 _playerSpawn;
    public Vector3 PlayerSpawn { get { return _playerSpawn; } }

    [Space]
    [SerializeField] private string _saveString = "/gameSave.dat";
    [SerializeField] private int _numberOfAnimals = 2;
    [SerializeField] [Tooltip("Maximum number of collectibles for each animal")] private int[] _maximumCollectibles;
    [Space]
    [Header("Collectibles")]
    [SerializeField] private string _collectibleTag = "Collectible";

    private int CurrentLevelNumber { get { return SceneManager.GetActiveScene().buildIndex; } }

    public int[] CollectiblesCount { get { return _collectiblesCount; } }
    public int[] MaxCollectiblesCount { get { return _maximumCollectibles; } }

    private int[] _collectiblesCount;

    public List<float> _pickedUpCollectibles;

    private Save _currentLoadObject = null;

    private void Start()
    {
        _collectiblesCount = new int[_numberOfAnimals];
        _pickedUpCollectibles = new List<float>();
    }

    private void Awake()
    {
        _playerSpawn = GameObject.FindGameObjectWithTag("Spawn").transform.position;
    }

    public void LoadLevel(int index)
    {
        SceneManager.LoadScene(index);
    }

    public void StartGame()
    {
        if (SaveExists())
        {
            UIManager.Instance.MainMenuPlay();
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

        // Saving Data
        saveObject.currentLevelNumber = this.CurrentLevelNumber;
        saveObject.collectiblesCounts = this.CollectiblesCount;
        saveObject.currentPlayerCheckpoint = new Vector3Ser(PlayerSpawn.x, PlayerSpawn.y, PlayerSpawn.z);
        saveObject.pickedUpCollectiblesID = new List<float>();
        foreach (float c in _pickedUpCollectibles)
        {
            saveObject.pickedUpCollectiblesID.Add(c);
        }

        // ------------
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

            // Loading data
            LoadLevel(loadObject.currentLevelNumber);

            // -----------
            file.Close();
            _currentLoadObject = loadObject;
            _playerSpawn = new Vector3(_currentLoadObject.currentPlayerCheckpoint.x, _currentLoadObject.currentPlayerCheckpoint.y, _currentLoadObject.currentPlayerCheckpoint.z);
            SceneManager.sceneLoaded += OnNewSceneLoaded;
        }
    }

    private void OnNewSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        GameObject[] collectiblesInLevel = GameObject.FindGameObjectsWithTag("Collectible");
        foreach (GameObject collectible in collectiblesInLevel)
        {
            if (_currentLoadObject.pickedUpCollectiblesID.Contains(collectible.transform.position.sqrMagnitude))
                Destroy(collectible);

        }
        _playerSpawn = new Vector3(_currentLoadObject.currentPlayerCheckpoint.x, _currentLoadObject.currentPlayerCheckpoint.y, _currentLoadObject.currentPlayerCheckpoint.z);
        AnimalsManager.Instance.ResetSpawn();
        SceneManager.sceneLoaded -= OnNewSceneLoaded;
    }

    public bool SaveExists()
    {
        return File.Exists(Application.persistentDataPath + _saveString);
    }


    public void CollectiblePickup(GameObject collectible, CollectibleType type)
    {
        _pickedUpCollectibles.Add(collectible.transform.position.sqrMagnitude);
        print(_pickedUpCollectibles.Count);
        _collectiblesCount[(int)type]++;
        //EventsManager.Instance.collectibleChangeDelegate();
    }

    private bool CompareVectors(Vector3 a, Vector3Ser b)
    {
        return (a.x == b.x && a.y == b.y && a.z == b.z);
    }
}

[System.Serializable]
class Save
{
    public int currentLevelNumber;
    public Vector3Ser currentPlayerCheckpoint;
    public List<float> pickedUpCollectiblesID;

    public int[] collectiblesCounts;
}

[System.Serializable]
public class Vector3Ser
{
    public Vector3Ser(float x, float y, float z)
    {
        this.x = x;
        this.y = y;
        this.z = z;
    }
    public float x, y, z;
}