using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;


public class UIManager : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;   

    [Header("Elements")]
    [SerializeField] private GameObject menuPanel;
    [SerializeField] private GameObject gamePanel;
    [SerializeField] private GameObject gameWinPanel;
    [SerializeField] private GameObject gameLosePanel;
    [SerializeField] private GameObject menuBar;
    [Header("Settings")]
    [SerializeField] private GameObject throwStartingButton;
    [SerializeField] private GameObject upgradeHookPanel;
    [Header("Level")]
    [SerializeField] private Transform enemyParent;
    [SerializeField] private Transform heroParent;
    [SerializeField] private TowerController towerController;
    [Header("Game Win/Lose Panel Settings")]
    [SerializeField] private TextMeshProUGUI winArenaText;
    [SerializeField] private TextMeshProUGUI winGoldText;
    [SerializeField] private TextMeshProUGUI winBonusGoldText;
    [SerializeField] private TextMeshProUGUI winEnemyCountText;
    [SerializeField] private TextMeshProUGUI loseArenaText;
    [SerializeField] private TextMeshProUGUI loseGoldText;
    [SerializeField] private TextMeshProUGUI loseBonusGoldText;
    [SerializeField] private TextMeshProUGUI loseEnemyCountText;

    private void Awake()
    {
        Hook.onThrowStarting += StartingThrow;
        Hook.onThrowEnding += EndingThrow;

        TowerController.onGameLose += GameLosePanel;
        EnemyTowerController.onGameWin += GameWinPanel;
    }
    private void OnDestroy()
    {
        Hook.onThrowStarting -= StartingThrow;
        Hook.onThrowEnding -= EndingThrow;

        TowerController.onGameLose -= GameLosePanel;
        EnemyTowerController.onGameWin -= GameWinPanel;


    }

    private void Start()
    {
        GameUIStageChanged(UIGameStage.Menu);
    }
    void StartingThrow()
    {
        throwStartingButton.SetActive(false);
        upgradeHookPanel.SetActive(false);
    }

    void EndingThrow()
    {
        upgradeHookPanel.SetActive(true);
        throwStartingButton.SetActive(true);
    }

    public void StartWave()
    {
        upgradeHookPanel.SetActive(false);
        throwStartingButton.SetActive(true);
    }
    public void PlayButton()
    {
        GameUIStageChanged(UIGameStage.Game);
        int waveIndex = PlayerPrefs.GetInt("WaveIndex", 0);

        switch (waveIndex)
        {
            case 0:
                GameObject arena = gameManager.GetArenaTileset(0);
                arena.SetActive(true);
                break;
            case 1:
                GameObject arena1 = gameManager.GetArenaTileset(0);
                arena1.SetActive(true);
                break;
            case 2:
                GameObject arena2 = gameManager.GetArenaTileset(1);
                arena2.SetActive(true);
                break;
            case 3:
                GameObject arena3 = gameManager.GetArenaTileset(1);
                arena3.SetActive(true);
                break;
        }

        WaveManager.instance.StartWaves(waveIndex);
    }
    public void GameLosePanel()
    {
        GameUIStageChanged(UIGameStage.GameLose);

        loseArenaText.text= (PlayerPrefs.GetInt("WaveIndex", 0)-1).ToString();
        int enemyCount = GameManager.enemyCount;
        loseEnemyCountText.text = "Number of enemies killed: " + enemyCount.ToString();
        int rewardedGold = enemyCount * 5;
        loseBonusGoldText.text = rewardedGold.ToString();
        loseGoldText.text = 0.ToString();


        DataManager.instance.AddGold(rewardedGold);
    }
    public void GameLoseButton()
    {
        GameUIStageChanged(UIGameStage.Menu);

        towerController.ResetTower();
        gameManager.PowerUpReset();

        SceneManager.LoadScene(0);
    }

    public void GameWinPanel()
    {
        GameUIStageChanged(UIGameStage.GameWin);

        winArenaText.text = (PlayerPrefs.GetInt("WaveIndex", 0)).ToString();
        int enemyCount = GameManager.enemyCount;
        winEnemyCountText.text = "Number of enemies killed: " + enemyCount.ToString();
        int rewardedGold = enemyCount * 5;
        winBonusGoldText.text = rewardedGold.ToString();
        winGoldText.text = gameManager.arenaWinReward[(PlayerPrefs.GetInt("WaveIndex", 0))].ToString();

        DataManager.instance.AddGold(rewardedGold+ gameManager.arenaWinReward[(PlayerPrefs.GetInt("WaveIndex", 0))]);

    }
    public void GameWinButton()
    {
        towerController.ResetTower();
        gameManager.PowerUpReset();
        SceneManager.LoadScene(0);


    }
    public void GameUIStageChanged(UIGameStage stage)
    {
        switch (stage)
        {
            case UIGameStage.Menu:
                menuPanel.SetActive(true);
                gamePanel.SetActive(false);
                gameWinPanel.SetActive(false);
                gameLosePanel.SetActive(false);
                menuBar.SetActive(true);
                break;
            case UIGameStage.Game:
                menuPanel.SetActive(false);
                gamePanel.SetActive(true);
                gameWinPanel.SetActive(false);
                gameLosePanel.SetActive(false);
                menuBar.SetActive(false);

                break;
            case UIGameStage.GameWin:
                menuPanel.SetActive(false);
                gamePanel.SetActive(false);
                gameWinPanel.SetActive(true);
                gameLosePanel.SetActive(false);


                break;
            case UIGameStage.GameLose:
                menuPanel.SetActive(false);
                gamePanel.SetActive(false);
                gameWinPanel.SetActive(false);
                gameLosePanel.SetActive(true);
                break;

            default:
                break;
        }

    }

    

}
public enum UIGameStage
{
    Menu,
    Game,
    GameWin,
    GameLose
}
