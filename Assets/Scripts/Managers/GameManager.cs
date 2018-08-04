using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
	private Vector3 _playerSpawn;
	public Vector3 PlayerSpawn { get { return _playerSpawn; } }

	[Space]
	[Header("Animals")]
	[SerializeField]
	private Animal[] _availableAnimals;
	public Animal[] AvailableAnimals { get { return _availableAnimals; } }

	[Space]
	[SerializeField]
	private string _saveString = "/gameSave.dat";
	[SerializeField] [Tooltip("Maximum number of collectibles for each animal")] private int[] _maximumCollectibles;
	[Space]
	[Header("Collectibles")]
	[SerializeField]
	private string _collectibleTag = "Collectible";
	[Space]
	[Header("Levels")]
	[SerializeField]
	private int[] _levels;

	private int CurrentLevelNumber { get { return SceneManager.GetActiveScene().buildIndex; } }

	public int[] CollectiblesCount { get { return _collectiblesCount; } }

	private int[] _collectiblesCount;

	public List<float> _pickedUpCollectibles;

	private Save _currentLoadObject = null;


	public bool InPauseMenu = false;

	private int _numberOfAnimals = 0;
	private void Start()
	{
		_numberOfAnimals = _availableAnimals.Length;
		_collectiblesCount = new int[_numberOfAnimals];

		_pickedUpCollectibles = new List<float>();

		if (GameObject.FindGameObjectWithTag("Spawn") != null) _playerSpawn = GameObject.FindGameObjectWithTag("Spawn").transform.position;
	}

	public void LoadLevel(int index, bool newLevel)
	{
		SceneManager.LoadScene(index);
		if (newLevel) SceneManager.sceneLoaded += OnNewLevelLoaded;
	}

	private void OnNewLevelLoaded(Scene scene, LoadSceneMode mode)
	{
		_playerSpawn = GameObject.FindGameObjectWithTag("Spawn").transform.position;
		AnimalsManager.Instance.ResetSpawn();
		EventsManager.Instance.formChangeDelegate();
		SceneManager.sceneLoaded -= OnNewLevelLoaded;
	}

	public void StartGame()
	{
		if (SaveExists())
		{
			UIManager.Instance.MainMenuPlay();
		}
		else
		{
			PlayIntro();

		}
	}
	void PlayIntro()
	{
		LoadLevel(4, false);
	}

	public void NewGame()
	{
		File.Delete(Application.persistentDataPath + _saveString);

		_numberOfAnimals = _availableAnimals.Length;
		_collectiblesCount = new int[_numberOfAnimals];

		_pickedUpCollectibles = new List<float>();
		LoadLevel(_levels[0], true);
	}

	public void Continue()
	{
		LoadGame();
	}
	public void FinishGame()
	{
		Credits(3);
	}

	public void NextLevel()
	{
		int currentLevelIndex = SceneManager.GetActiveScene().buildIndex;
		LoadLevel(++currentLevelIndex, true);
	}

	public void QuitGame()
	{
		Application.Quit();
	}
	public void DeleteSavedGame()
	{

		File.Delete(Application.persistentDataPath + _saveString);
	}

	public void SaveGame(Transform newSpawn)
	{
		Save saveObject = new Save();

		BinaryFormatter bf = new BinaryFormatter();
		FileStream file = File.Open(Application.persistentDataPath + _saveString, FileMode.OpenOrCreate);

		// Saving Data
		saveObject.currentLevelNumber = this.CurrentLevelNumber;
		saveObject.collectiblesCounts = this.CollectiblesCount;
		saveObject.currentPlayerCheckpoint = new Vector3Ser(newSpawn.position.x, newSpawn.position.y, newSpawn.position.z);
		saveObject.pickedUpCollectiblesID = new List<float>();
		foreach (float c in _pickedUpCollectibles) saveObject.pickedUpCollectiblesID.Add(c);

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

			_pickedUpCollectibles = new List<float>();
			foreach (float c in loadObject.pickedUpCollectiblesID) _pickedUpCollectibles.Add(c);

			// Loading data
			LoadLevel(loadObject.currentLevelNumber, false);

			// -----------
			file.Close();
			_currentLoadObject = loadObject;
			_playerSpawn = new Vector3(_currentLoadObject.currentPlayerCheckpoint.x, _currentLoadObject.currentPlayerCheckpoint.y, _currentLoadObject.currentPlayerCheckpoint.z);
			SceneManager.sceneLoaded += OnSaveLoaded;
		}
	}

	private void OnSaveLoaded(Scene scene, LoadSceneMode mode)
	{
		_playerSpawn = new Vector3(_currentLoadObject.currentPlayerCheckpoint.x, _currentLoadObject.currentPlayerCheckpoint.y, _currentLoadObject.currentPlayerCheckpoint.z);
		_collectiblesCount = _currentLoadObject.collectiblesCounts;
		AnimalsManager.Instance.ResetSpawn();
		// Remove already collected collectibles

		GameObject[] allCollectibles = GameObject.FindGameObjectsWithTag(_collectibleTag);

		foreach (GameObject obj in allCollectibles)
		{
			if (_currentLoadObject.pickedUpCollectiblesID.Contains(obj.transform.position.sqrMagnitude))
			{
				Destroy(obj);
			}
		}


		SceneManager.sceneLoaded -= OnSaveLoaded;
		EventsManager.Instance.formChangeDelegate();
	}

	public bool SaveExists()
	{
		return File.Exists(Application.persistentDataPath + _saveString);
	}


	public void CollectiblePickup(GameObject collectible, AnimalForm type)
	{
		_pickedUpCollectibles.Add(collectible.transform.position.sqrMagnitude);
		_collectiblesCount[(int)type - 1]++;
		print(type + " -> " + _collectiblesCount[(int)type - 1]);
		EventsManager.Instance.collectibleChangeDelegate();
	}

	private bool CompareVectors(Vector3 a, Vector3Ser b)
	{
		return (a.x == b.x && a.y == b.y && a.z == b.z);
	}
	public bool IsAnimalAvailable(int number)
	{
		return (_collectiblesCount[number] == AvailableAnimals[number].RequiredParts);
	}

	public void Credits(int index)
	{
		LoadLevel(index, false);
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