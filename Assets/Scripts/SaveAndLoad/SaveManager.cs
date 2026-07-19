using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class SaveManager : MonoBehaviour
{
    public static SaveManager instance;

    [SerializeField] private string fileName;
    [SerializeField] private bool encryptData;

    private GameData gameData;
    private List<ISaveManager> saveManagers;
    private FileDataHandler dataHandler;

    [ContextMenu("Delete Save File")]
    public void DeleteSaveData()
    {
        dataHandler = new FileDataHandler(Application.persistentDataPath, fileName, encryptData);
        dataHandler.Delete();
    }

    private void Awake()
    {
        if (instance != null)
            Destroy(instance.gameObject);
        else
            instance = this;

        dataHandler = new FileDataHandler(Application.persistentDataPath, fileName, encryptData);//基于不同的系统决定不同的路径，但是这里先根据教程进行学习
        Debug.Log($"Save file location: {Application.persistentDataPath}/{fileName}");
        saveManagers = FindAllSaveManagers();

        LoadGame();
    }

    //这里是将Awake和Start的内容进行了调整，主要是为了确保在Awake中就能加载数据，而不是等到Start之后，这样可以确保在其他脚本的Start方法中也能访问到已经加载的数据。
    //或许可以在scripts execution order中调整SaveManager的执行顺序，但是并不建议；或者使用invoke等方法
    private void Start()
    {
        //dataHandler = new FileDataHandler(Application.persistentDataPath, fileName);//基于不同的系统决定不同的路径，但是这里先根据教程进行学习
        //Debug.Log($"Save file location: {Application.persistentDataPath}/{fileName}");
        //saveManagers = FindAllSaveManagers();

        //LoadGame();
    }

    public void NewGame()
    {
        gameData = new GameData();
    }

    public void LoadGame()
    {
        gameData = dataHandler.Load();

        if (this.gameData == null) //这里后续可以修改
        {
            Debug.Log("No game data found, creating new game data");
            NewGame();
        }

        foreach(ISaveManager saveManager in saveManagers)
        {
            saveManager.LoadData(gameData);
        }

        Debug.Log("load currency: " + gameData.currency);
    }

    public void SaveGame()
    {
        Debug.Log("Game data saved");

        foreach (var saveManager in saveManagers)
        {
            saveManager.SaveData(ref gameData);
        }

        dataHandler.Save(gameData);
        Debug.Log("save currency: " + gameData.currency);
    }

    private void OnApplicationQuit()
    {
        SaveGame();
    }

    private List<ISaveManager> FindAllSaveManagers()
    {
        IEnumerable<ISaveManager> saveManagers = FindObjectsOfType<MonoBehaviour>(true).OfType<ISaveManager>();
        return new List<ISaveManager>(saveManagers);
    }

}
