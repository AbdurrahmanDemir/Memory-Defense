using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] private UpgradeSelectManager upgradeSelectManager;

    [Header("Elements")]
    [SerializeField] private GameObject[] allHeroes;
    [SerializeField] private Transform[] creatHeroPosition;
    [SerializeField] private Transform heroParent;
    [SerializeField] private Hook hook;
    [Header("Settings")]
    [SerializeField] private Slider powerUpSlider;
    [SerializeField] private int[] powerUpLevel;
    int powerUpIndex=0;
    [Header("Level Settings")]
    public int[] arenaWinReward;


    [Header("Enemy")]
    public static int enemyCount;

    [Header("Arena Tileset")]
    [SerializeField] private GameObject[] arenaTileset;

    private void Awake()
    {
        Hook.onThrowEnding += CreatHeroes;
        Enemy.onDead += PowerUpSliderUpdate;
    }
    private void OnDestroy()
    {
        Hook.onThrowEnding -= CreatHeroes;
        Enemy.onDead -= PowerUpSliderUpdate;

    }
    private void Start()
    {
        powerUpSlider.value = 0;
        powerUpSlider.maxValue = powerUpLevel[powerUpIndex];
    }
    public void CreatHeroes()
    {
        for (int i = 0; i < hook.hookedHero.Count; i++)
        {            
            switch (hook.hookedHero[i].GetHeroName())
            {
                case "Angel":
                    int RandomPos = Random.Range(0, creatHeroPosition.Length);
                    Instantiate(allHeroes[0], creatHeroPosition[RandomPos].position, Quaternion.Euler(0f, 0f, 0f), heroParent);
                    break;
                case "Range Angel":
                    int RandomPos1 = Random.Range(0, creatHeroPosition.Length);
                    Instantiate(allHeroes[1], creatHeroPosition[RandomPos1].position, Quaternion.Euler(0f, 0f, 0f), heroParent);

                    break;
                case "Angel Man":
                    int RandomPos2 = Random.Range(0, creatHeroPosition.Length);
                    Instantiate(allHeroes[2], creatHeroPosition[RandomPos2].position, Quaternion.Euler(0f, 0f, 0f), heroParent);
                    break;
                case "Ice Golem":
                    int RandomPos3 = Random.Range(0, creatHeroPosition.Length);
                    Instantiate(allHeroes[3], creatHeroPosition[RandomPos3].position, Quaternion.Euler(0f, 0f, 0f), heroParent);

                    break;
            }
        }
    }
    public void GameSpeedController()
    {
        if (Time.timeScale == 1)
            Time.timeScale = 3;
        else
            Time.timeScale = 1;
    }

    public void PowerUpSliderUpdate(Vector2 createPosition)
    {
        powerUpSlider.value++;

        if(powerUpSlider.value >= powerUpSlider.maxValue)
        {
            powerUpIndex++;
            powerUpSlider.maxValue = powerUpLevel[powerUpIndex];
            upgradeSelectManager.PowerUpPanelOpen();
            powerUpSlider.value = 0;
        }
    }
    public void PowerUpReset()
    {
        powerUpIndex = 0;
        powerUpSlider.value = 0;
        powerUpSlider.maxValue = powerUpLevel[powerUpIndex];
    }

    public GameObject GetArenaTileset(int index)
    {
        return arenaTileset[index];
    }
}
