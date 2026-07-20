using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using  UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour, ISaveManager
{
    public static GameManager instance;
    private Transform player;

    [SerializeField] private CheckPoint[] checkPoints;
    private string cloestCheckPointId;

    [Header("Lost currency")]
    [SerializeField] private GameObject lostCurrencyPrefab;
    public int lostCurrencyAmount;
    [SerializeField] private float lostCurrencyX;
    [SerializeField] private float lostCurrencyY;


    private void Awake()
    {
        if(instance != null)
            Destroy(instance.gameObject);
        else
            instance = this;

        checkPoints = FindObjectsOfType<CheckPoint>();
    }

    private void Start()
    {
        //checkPoints = FindObjectsOfType<CheckPoint>();
        player = PlayerManager.instance.player.transform;
    }


    public void RestartScene()
    {
        SaveManager.instance.SaveGame();

        Scene scene = SceneManager.GetActiveScene();

        SceneManager.LoadScene(scene.name);
    }

    public void LoadData(GameData _data)
    {
        StartCoroutine(LoadWithDelay(_data));

        Debug.Log("game manager load");
    }

    private void LoadCheckPoint(GameData _data)
    {
        foreach (KeyValuePair<string, bool> pair in _data.checkPoints)
        {
            foreach (CheckPoint checkPoint in checkPoints)
            {
                if (checkPoint.checkPointId == pair.Key && pair.Value == true)
                {
                    checkPoint.ActivateCheckPiont();
                }
            }
        }
    }

    private void LoadLostCurrency(GameData _data)
    {
        lostCurrencyX = _data.lostCurrencyX;
        lostCurrencyY = _data.lostCurrencyY;
        lostCurrencyAmount = _data.lostCurrencyAmount;

        if(lostCurrencyAmount > 0)
        {
            GameObject newLostCurrency = Instantiate(lostCurrencyPrefab, new Vector3(lostCurrencyX, lostCurrencyY), Quaternion.identity);
            newLostCurrency.GetComponent<LostCurrencyController>().currency = lostCurrencyAmount;
        }

        lostCurrencyAmount = 0;
    }

    private IEnumerator LoadWithDelay(GameData _data)
    {
        yield return new WaitForSeconds(0.1f);

        LoadCheckPoint(_data);
        LoadCloestCheckPoint(_data);
        LoadLostCurrency(_data);
    }

    public void SaveData(ref GameData _data)
    {
        SaveLostCurrency(_data);


        if(FindCloestCheckPoint() != null)
            _data.cloestCheckPointId = FindCloestCheckPoint().checkPointId;
        
        _data.checkPoints.Clear();

        foreach (CheckPoint checkPoint in checkPoints)
        {
            _data.checkPoints.Add(checkPoint.checkPointId, checkPoint.activated);
        }
    }

    private void SaveLostCurrency(GameData _data)
    {
        _data.lostCurrencyX = player.position.x;
        _data.lostCurrencyY = player.position.y;
        _data.lostCurrencyAmount = lostCurrencyAmount;
    }

    private void LoadCloestCheckPoint(GameData _data)
    {
        if(_data.cloestCheckPointId == null)
            return;

        cloestCheckPointId = _data.cloestCheckPointId;

        foreach (CheckPoint checkPoint in checkPoints)
        {
            if (checkPoint.checkPointId == cloestCheckPointId)
            {
                //PlayerManager.instance.player.transform.position = checkPoint.transform.position;
                // 由于我们将存档点的烛台的中心设在了底部中心位置，因此需要偏移
                player.position = new Vector2(checkPoint.transform.position.x,
                    checkPoint.transform.position.y + 1.5f);
            }
        }
    }

    private CheckPoint FindCloestCheckPoint()
    {
        float closestDistance = Mathf.Infinity;
        CheckPoint closestCheckPoint = null;

        foreach(CheckPoint checkPoint in checkPoints)
        {
            float distanceToCheckPoint = Vector2.Distance(player.position, checkPoint.transform.position);
            if(checkPoint.activated && distanceToCheckPoint < closestDistance)
            {
                closestDistance = distanceToCheckPoint;
                closestCheckPoint = checkPoint;
            }
        }

        return closestCheckPoint;
    }
}
