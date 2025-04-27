using UnityEngine;
using System;
using UnityEngine.UI;
using Random = UnityEngine.Random;
using DG.Tweening;

public class UpgradeSelectManager : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] private UpgradeSelectSO[] upgradeData;
    [SerializeField] private GameObject powerUpPanel;
    [Header("Buttons")]
    [SerializeField] private GameObject buttonPrefabs;
    [SerializeField] private Transform buttonTransform;

    [Header("Action")]
    public static Action onPowerUpPanelOpened;
    public static Action onPowerUpPanelClosed;
    public static Action<int> towerHealthItem;
    public static Action hookLenghtItem;
    public static Action hookStranghtItem;
    public static Action<int> tokenAddItem;
    public static Action<int> heroDamageItem;
    public static Action<int> heroHealthItem;

    public void GetUpgrade()
    {
        buttonTransform.Clear();

        for (int i = 0; i < 3; i++)
        {
            GameObject buttonInstance = Instantiate(buttonPrefabs, buttonTransform);
            int randomTypes = Random.Range(0, upgradeData.Length);

            buttonInstance.GetComponent<UpgradeSelectButton>().Config(upgradeData[randomTypes].upgradeIcon, upgradeData[randomTypes].name, upgradeData[randomTypes].upgradeDescription);

            switch (upgradeData[randomTypes].upgradeType)
            {
                case UpgradeType.TowerHealth:
                    buttonInstance.GetComponent<UpgradeSelectButton>().GetButton().onClick
                .AddListener(() => TowerUpgradeHealthItem(upgradeData[randomTypes].amount));

                    break;
                case UpgradeType.HookLenght:
                    buttonInstance.GetComponent<UpgradeSelectButton>().GetButton().onClick
                .AddListener(() => HookLenghtUpgrade());

                    break;
                case UpgradeType.HookStrenght:
                    buttonInstance.GetComponent<UpgradeSelectButton>().GetButton().onClick
                .AddListener(() => HookStrangthItem());

                    break;
                case UpgradeType.EpicHeroCount:
                    break;
                case UpgradeType.LegendaryHeroCount:
                    break;
                case UpgradeType.DamageUpgrade:
                    buttonInstance.GetComponent<UpgradeSelectButton>().GetButton().onClick
                .AddListener(() => HeroDamageItem(upgradeData[randomTypes].amount));
                    break;
                case UpgradeType.HealthUpgrade:
                    buttonInstance.GetComponent<UpgradeSelectButton>().GetButton().onClick
                .AddListener(() => HeroHealthItem(upgradeData[randomTypes].amount));
                    break;
                case UpgradeType.TokenAdd:
                    buttonInstance.GetComponent<UpgradeSelectButton>().GetButton().onClick
                .AddListener(() => TokenAddItem(upgradeData[randomTypes].amount));

                    break;
                default:
                    break;
            }
        }
    }

    public void TowerUpgradeHealthItem(int amount)
    {
        towerHealthItem?.Invoke(amount);
        PowerUpPanelOpen();

    }
    public void HookLenghtUpgrade()
    {
        hookLenghtItem?.Invoke();
        PowerUpPanelOpen();

    }
    public void HookStrangthItem()
    {
        hookStranghtItem?.Invoke();
        PowerUpPanelOpen();

    }
    public void TokenAddItem(int amount)
    {
        tokenAddItem?.Invoke(amount);
        PowerUpPanelOpen();
    }
    public void HeroDamageItem(int amount)
    {
        heroDamageItem?.Invoke(amount);
        PowerUpPanelOpen();
    }
    public void HeroHealthItem(int amount)
    {
        heroHealthItem?.Invoke(amount);
        PowerUpPanelOpen();
    }

    public void PowerUpPanelOpen()
    {
        if (powerUpPanel.activeSelf)
        {
            powerUpPanel.transform.DOScale(Vector3.zero, 0.2f).SetEase(Ease.InBack).OnComplete(() => powerUpPanel.SetActive(false));
            onPowerUpPanelClosed?.Invoke();
        }
        else
        {
            powerUpPanel.SetActive(true);
            powerUpPanel.transform.localScale = Vector3.zero;
            powerUpPanel.transform.DOScale(Vector3.one, 0.2f).SetEase(Ease.OutBack);
            GetUpgrade();
            onPowerUpPanelOpened?.Invoke();
        }
    }
}
