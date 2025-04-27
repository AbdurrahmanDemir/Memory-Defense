using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MenuHeroDetails : MonoBehaviour
{
    [Header("Card")]
    [SerializeField] private TextMeshProUGUI cardNameText;
    [SerializeField] private Image cardIconImage;
    [SerializeField] private GameObject[] cardTypes;
    [SerializeField] private TextMeshProUGUI cardText;
    [Header("Stats")]
    [SerializeField] private TextMeshProUGUI damageText;
    [SerializeField] private TextMeshProUGUI healthText;
    [SerializeField] private TextMeshProUGUI rangeText;
    [SerializeField] private TextMeshProUGUI attackSpeedText;
    [SerializeField] private TextMeshProUGUI moveSpeedText;
    [SerializeField] private TextMeshProUGUI specialStatNameText;
    [SerializeField] private TextMeshProUGUI specialStatText;
    [Header("Other")]
    [SerializeField] TextMeshProUGUI upgradePriceText;
    [SerializeField] Button upgradeButton;

    public void Config(string name, Sprite icon, string type, string cardDetails,
                       int damage, int health, float range, float attackSpeed, float moveSpeed,
                       int price, string specialStatName, float specialStat)
    {
        cardNameText.text = name;
        cardIconImage.sprite = icon;
        cardText.text = cardDetails;
        damageText.text = damage.ToString();
        healthText.text = health.ToString();
        rangeText.text = range.ToString();
        attackSpeedText.text = attackSpeed.ToString();
        moveSpeedText.text = moveSpeed.ToString();
        upgradePriceText.text = price.ToString();
        specialStatNameText.text = specialStatName;
        specialStatText.text = specialStat.ToString();

        switch (type)
        {
            case "Cammon":
                cardTypes[0].SetActive(true);
                cardTypes[1].SetActive(false);
                cardTypes[2].SetActive(false);
                cardTypes[3].SetActive(false);
                break;
            case "Rare":
                cardTypes[0].SetActive(false);
                cardTypes[1].SetActive(true);
                cardTypes[2].SetActive(false);
                cardTypes[3].SetActive(false);
                break;
            case "Epic":
                cardTypes[0].SetActive(false);
                cardTypes[1].SetActive(false);
                cardTypes[2].SetActive(true);
                cardTypes[3].SetActive(false);
                break;
            case "Legendary":
                cardTypes[0].SetActive(false);
                cardTypes[1].SetActive(false);
                cardTypes[2].SetActive(false);
                cardTypes[3].SetActive(true);
                break;
        }

    }

    public Button GetUpgradeButton()
    {
        return upgradeButton;
    }
    public void TogglePanel()
    {
        if (gameObject.activeSelf)
        {
            gameObject.transform.DOScale(Vector3.zero, 0.2f).SetEase(Ease.InBack).OnComplete(() => gameObject.SetActive(false));
            DOTween.Kill(gameObject);
            Destroy(gameObject,5f);
        }
        else
        {
            gameObject.SetActive(true);
            gameObject.transform.localScale = Vector3.zero;
            gameObject.transform.DOScale(Vector3.one, 0.2f).SetEase(Ease.OutBack);
        }
    }


}
