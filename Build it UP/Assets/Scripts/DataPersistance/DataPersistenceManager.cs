using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class DataPersistenceManager : MonoBehaviour
{
    // Start is called before the first frame update
    [Header("File Storage Config")]
    [SerializeField] private string fileName;
    private GameData gameData;
    private List<IDataPersistence> dataPersistenceObjects;
    private FileDataHandler dataHandler;

    public static DataPersistenceManager instance { get; private set; }
    void Start()
    {
        this.dataHandler = new FileDataHandler(Application.persistentDataPath, fileName);
        Debug.Log("path " + Application.persistentDataPath);
        this.dataPersistenceObjects = FindAllDataPersistenceObjects();
        //LoadGame();
        NewGame();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void OnApplicationQuit()
    {
        SaveGame();
    }


    private void Awake()
    {
        if (instance != null)
            Debug.Log("Found Data persistance manager in scene");
        instance = this;
    }

    public void NewGame()
    {
        this.gameData = new GameData();
    }

    public void LoadGame()
    {
        this.gameData = dataHandler.Load();
        if (this.gameData == null)
        {
            Debug.Log("No data found. Initializing...");
            NewGame();
        }

        foreach (IDataPersistence dataPersistenceObj in dataPersistenceObjects)
            dataPersistenceObj.LoadData(gameData);

        Debug.Log("Loaded correct answers:" + gameData.respuestasCorrectas);
    }

    public void SaveGame()
    {
        foreach (IDataPersistence dataPersistenceObj in dataPersistenceObjects)
        {
            dataPersistenceObj.SaveData(ref gameData);
        }
        Debug.Log("Saved correct answers:" + gameData.respuestasCorrectas);
        Debug.Log("Saved incorrect answers:" + gameData.respuestasIncorrectas);

        dataHandler.Save(gameData);
    }

    private List<IDataPersistence> FindAllDataPersistenceObjects()
    {
        IEnumerable<IDataPersistence> dataPersistenceObjects = FindObjectsOfType<MonoBehaviour>().OfType<IDataPersistence>();
        return new List<IDataPersistence>(dataPersistenceObjects);
    }
}
