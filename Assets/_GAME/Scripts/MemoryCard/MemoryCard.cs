using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening.Core.Easing;

public class MemoryCard : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public MemoryCardSO cardData;

    [SerializeField] private TextMeshProUGUI cardNameText;
    [SerializeField] private TextMeshProUGUI cardDamageText;
    [SerializeField] private Image cardIconImage;
    [SerializeField] private GameObject cardBackImage;
    [SerializeField] private GameObject cardFrontImage;

    MemoryCardManager memoryCardManager;

    public bool isFlipped = false;
    private Vector3 originalScale;

    private void Start()
    {
        memoryCardManager = GameObject.FindGameObjectWithTag("MemoryCardManager").GetComponent<MemoryCardManager>();

        originalScale = transform.localScale;

        cardFrontImage.gameObject.SetActive(false);
        cardBackImage.gameObject.SetActive(true);

    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        transform.DOScale(originalScale * 1.1f, 0.2f);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        transform.DOScale(originalScale, 0.2f);
    }
    public void OnCardClick()
    {
        if (!isFlipped && !memoryCardManager.isProcessingCards && !memoryCardManager.isProcessingTrapCards)
        {
            FlipCard();
            memoryCardManager.OnCardMatch(this);
            transform.DOScale(originalScale * 1.1f, 0.2f);
        }
    }

    public void FlipCard()
    {
        isFlipped = true;

        transform.DORotate(new Vector3(0, 90, 0), 0.25f).OnComplete(() =>
        {
            cardBackImage.gameObject.SetActive(false);
            cardFrontImage.gameObject.SetActive(true);

            cardNameText.text = cardData.cardName;
            cardDamageText.text = cardData.damage.ToString();
            cardIconImage.sprite = cardData.cardImage;

            transform.DORotate(new Vector3(0, 0, 0), 0.25f);
        });
    }

    public void HideCard()
    {
        isFlipped = false;
        transform.DORotate(new Vector3(0, 90, 0), 0.25f).OnComplete(() =>
        {
            cardFrontImage.gameObject.SetActive(false);
            cardBackImage.gameObject.SetActive(true);

            transform.DORotate(new Vector3(0, 0, 0), 0.25f);
        });
    }

}
