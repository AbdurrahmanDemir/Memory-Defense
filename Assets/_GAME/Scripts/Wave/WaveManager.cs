using UnityEngine;
using System.Collections.Generic;
using System;
using Random = UnityEngine.Random;

public class WaveManager : MonoBehaviour
{
    public static WaveManager instance;
    [SerializeField] private EnemyTowerController enemyTowerController;

    [Header("Elements")]
    [SerializeField] private Wave[] waves;
    private Wave currentWave;
    [SerializeField] private Transform[] creatEnemyPosition;
    [SerializeField] private Transform enemyParent;
    [SerializeField] private WaveUIManager waveUI;

    [Header("Settings")]
    [SerializeField] private float timer;
    private bool isTimerOn;
    private int currentWaveIndex;
    private int currentSegmentIndex;
    private int currentEnemySubIndex;
    private int currentEnemyIndex;
    public int currentEnemyCount;
    private float segmentDelay = 5f;

    [Header("Action")]
    private bool onThrow = false;


    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        Hook.onThrowStarting += OnThrowStartingCallBack;
        Hook.onThrowEnding += OnThrowEndingCallBack;

        TowerController.onGameLose += OnThrowStartingCallBack;
        EnemyTowerController.onGameWin += OnThrowStartingCallBack;
    }
    private void OnDestroy()
    {
        Hook.onThrowStarting -= OnThrowStartingCallBack;
        Hook.onThrowEnding -= OnThrowEndingCallBack;

        TowerController.onGameLose -= OnThrowStartingCallBack;
        EnemyTowerController.onGameWin -= OnThrowStartingCallBack;
    }

    //private void Start()
    //{
    //    //StartWaves(0);
    //    //isTimerOn = true;
    //}

    private void Update()
    {
        if (!isTimerOn)
            return;

        ManageCurrentWave();
    }

    public void StartWaves(int index)
    {
        currentWaveIndex = index;
        currentSegmentIndex = 0;
        currentEnemyIndex = 0;
        enemyTowerController.towerSO = waves[currentWaveIndex].waveTower;
        enemyTowerController.TowerInfoUpdate();
        currentWave = waves[currentWaveIndex];
        //waveUI.waveIndexText.text= currentWaveIndex.ToString();
        isTimerOn = true;
        SetupNextSegment();
        waveUI.waveSegmentText.text = "Wave " + currentSegmentIndex + " / " + currentWave.segments.Count;
    }

    private void ManageCurrentWave()
    {
        if (currentSegmentIndex >= currentWave.segments.Count)
        {
            isTimerOn = false;
            return;
        }

        if (onThrow)
            return;

        WaveSegmet currentSegment = currentWave.segments[currentSegmentIndex];

        timer += Time.deltaTime;

        if (timer >= currentSegment.segmetDuration)
        {
            if (SpawnEnemy(currentSegment))
            {
                timer = 0;
            }
            else
            {
                currentSegmentIndex++;
                Debug.Log("Moving to next segment. Current Index: " + currentSegmentIndex);

                if (currentSegmentIndex >= currentWave.segments.Count)
                {
                    Debug.Log("All segments in the wave completed.");
                             

                    
                    Debug.Log("All waves completed.");

                    isTimerOn = false;
                    return;
                }

                waveUI.waveSegmentText.text = "Wave " + (currentSegmentIndex + 1) + " / " + currentWave.segments.Count;

                isTimerOn = false;
                Invoke("StartNextSegment", segmentDelay);
            }
        }
    }

    private void StartNextSegment()
    {
        isTimerOn = true;
        timer = 0;
        Debug.Log("Starting next segment. Current Index: " + currentSegmentIndex);
        SetupNextSegment();
    }



    private void SetupNextSegment()
    {
        currentEnemyIndex = 0;
        currentEnemySubIndex = 0; 
        if (currentSegmentIndex < currentWave.segments.Count)
        {
            if (currentWave.segments[currentSegmentIndex].segmentEnemys.Length > 0)
            {
                currentEnemyCount = currentWave.segments[currentSegmentIndex].segmentEnemys[currentEnemyIndex].enemyCount;
                Debug.Log("Setting up next segment. Enemy Count: " + currentEnemyCount);
            }
            else
            {
                Debug.LogError("No enemies defined in the current segment.");
            }
        }
    }


    private bool SpawnEnemy(WaveSegmet segment)
    {
        if (currentEnemyCount <= 0)
        {
            currentEnemySubIndex++;
            if (currentEnemySubIndex < segment.segmentEnemys[currentEnemyIndex].enemy.Length)
            {
                currentEnemyCount = segment.segmentEnemys[currentEnemyIndex].enemyCount;
            }
            else
            {
                currentEnemySubIndex = 0;
                currentEnemyIndex++;
                if (currentEnemyIndex < segment.segmentEnemys.Length)
                {
                    currentEnemyCount = segment.segmentEnemys[currentEnemyIndex].enemyCount;
                }
                else
                {
                    return false; 
                }
            }
        }

        // Dizi sýnýr kontrolü
        if (currentEnemyIndex >= segment.segmentEnemys.Length ||
            currentEnemySubIndex >= segment.segmentEnemys[currentEnemyIndex].enemy.Length)
        {
            Debug.LogError("Index out of range error.");
            return false;
        }

        int randomCreatPos = Random.Range(0, creatEnemyPosition.Length);
        GameObject enemyInstance=  Instantiate(
            segment.segmentEnemys[currentEnemyIndex].enemy[currentEnemySubIndex],
            creatEnemyPosition[randomCreatPos].position,
            Quaternion.Euler(0f, 180f, 0f), enemyParent);

        Enemy enemy = enemyInstance.GetComponent<Enemy>();
        enemy.Initialize(segment.segmentEnemys[currentEnemyIndex].enemyLevel);



        currentEnemyCount--;
        return true;
    }






    public void OnThrowStartingCallBack()
    {
        onThrow = true;
        Time.timeScale = 1;
        Debug.Log("Avtipn çalýþtý" + onThrow);
    }
    public void OnThrowEndingCallBack()
    {
        onThrow = false;
        Debug.Log("Avtipn çalýþtý" + onThrow);
    }
}

[Serializable]
public struct Wave
{
    public string waveName;
    public TowerSO waveTower;
    public List<WaveSegmet> segments;
}

[Serializable]
public struct WaveSegmet
{
    public float segmetDuration;
    public WaveSegmentEnemyManage[] segmentEnemys;
}

[Serializable]
public struct WaveSegmentEnemyManage
{
    public GameObject[] enemy;
    public EnemySO enemyLevel;
    public int enemyCount;
}
