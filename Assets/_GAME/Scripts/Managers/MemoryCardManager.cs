using DG.Tweening;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System;
using Random = UnityEngine.Random;
using System.Collections;
using System.Linq;

public class MemoryCardManager : MonoBehaviour
{
    [Header("Elements")]
    //public LevelManager levelManager;
    public Transform cardParent;
    private MemoryCard firstSelectedCard;
    private MemoryCard secondSelectedCard;
    public GameObject cardPrefab;
    private List<MemoryCardSO> currentCards;
    public bool isProcessingCards = false;
    public bool isProcessingTrapCards = false;

    [Header("Cards")]
    public Levels[] levels;
    public List<MemoryCardSO> allCards;

    [Header("Move")]
    public TextMeshProUGUI movesText;
    private int movesRemaining = 2;
    [SerializeField] private Image movesIcon;
    [SerializeField] private Image movesBackg;

    public static Action OnMatchCard;
    public static Action<string> OnMatchHero;

    private int totalPairs; 
    private int matchedPairs;

    public Levels GenerateLevel(int numberOfPairs)
    {
        Levels generatedLevel = new Levels();
        List<MemoryCardSO> selectedCards = new List<MemoryCardSO>();

        List<MemoryCardSO> shuffledCards = new List<MemoryCardSO>(allCards);
        ShuffleList(shuffledCards);

        List<MemoryCardSO> heroCards = shuffledCards.Where(card => card.cardEffect == CardEffect.Hero).ToList();
        List<MemoryCardSO> otherCards = shuffledCards.Where(card => card.cardEffect == CardEffect.PowerUp || card.cardEffect == CardEffect.Coin || card.cardEffect == CardEffect.Trap).ToList();

        Debug.Log("Hero Card Count: "+ heroCards.Count);
        Debug.Log("Other Card Count: "+ otherCards.Count);

        int heroNeeded = Mathf.CeilToInt(numberOfPairs * 0.6f);
        int otherNeeded = numberOfPairs - heroNeeded;

        for (int i = 0; i < heroNeeded; i++)
        {
            if (heroCards.Count > 0)
            {
                selectedCards.Add(heroCards[0]);
                heroCards.RemoveAt(0);
            }
            else
            {
                if (otherCards.Count > 0)
                {
                    selectedCards.Add(otherCards[0]);
                    otherCards.RemoveAt(0);
                }
                else
                {
                    Debug.LogWarning("Yeterli kart bulunamadý!");
                    break;
                }
            }
        }

        for (int i = 0; i < otherNeeded; i++)
        {
            if (otherCards.Count > 0)
            {
                selectedCards.Add(otherCards[0]);
                otherCards.RemoveAt(0);
                Debug.Log("other kart var");
            }
            else
            {
                if (heroCards.Count > 0)
                {
                    selectedCards.Add(heroCards[0]);
                    heroCards.RemoveAt(0);
                }
                else
                {
                    Debug.LogWarning("Yeterli kart bulunamadý!");
                    break;
                }
            }
        }

        List<MemoryCardSO> pairedCards = new List<MemoryCardSO>();
        foreach (MemoryCardSO card in selectedCards)
        {
            pairedCards.Add(card);
            pairedCards.Add(card);
        }

        ShuffleList(pairedCards);

        generatedLevel.cards = pairedCards.ToArray();

        return generatedLevel;
    }






    private void ShuffleList<T>(List<T> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            T temp = list[i];
            int randomIndex = Random.Range(0, list.Count);
            list[i] = list[randomIndex];
            list[randomIndex] = temp;
        }
    }

    public void GamePlay()
    {
        if (DataManager.instance.TryPurchaseEnergy(0))
        {
            //UIManager.instance.GameUIStageChanged(GameState.Game);
            StartLevel(/*LevelManager.instance.LoadLevel()*/ 0);
            MoveUpdate(0);
        }
        else
        {
            Debug.Log("Enerjin yok");
            //UIManager.instance.OpenGameMidAdPanel();
        }
    }

    public void StartLevel(int levelIndex)
    {
        if (currentCards != null)
        {
            currentCards.Clear();
        }

        Levels currentLevel = levels[levelIndex];
        Levels generatedLevel = GenerateLevel(currentLevel.numberOfPairs);

        currentCards = new List<MemoryCardSO>(generatedLevel.cards);


        ShuffleCards(currentCards);
        InstantiateCards();

        totalPairs = currentLevel.numberOfPairs;
        matchedPairs = 0;

        movesRemaining = 2;
    }

    private void ShuffleCards(List<MemoryCardSO> cards)
    {
        for (int i = 0; i < cards.Count; i++)
        {
            MemoryCardSO temp = cards[i];
            int randomIndex = Random.Range(0, cards.Count);
            cards[i] = cards[randomIndex];
            cards[randomIndex] = temp;
        }
    }
    private void InstantiateCards()
    {
        foreach (MemoryCardSO cardSO in currentCards)
        {
            GameObject cardInstance = Instantiate(cardPrefab, cardParent);
            MemoryCard cardScript = cardInstance.GetComponent<MemoryCard>();
            cardScript.cardData = cardSO;
        }
    }
    public void OnCardMatch(MemoryCard card)
    {
        if (isProcessingTrapCards) return;
        if (isProcessingCards) return;

        if (firstSelectedCard == null)
        {
            firstSelectedCard = card;

            if (card.cardData.cardEffect == CardEffect.Trap)
            {
                isProcessingTrapCards = true;
                ApplyCardEffect(firstSelectedCard);
                StartCoroutine(CloseTrapCard(firstSelectedCard));
            }
        }
        else if (secondSelectedCard == null)
        {
            secondSelectedCard = card;
            isProcessingCards = true;
            StartCoroutine(CheckMatch());
        }
    }

    private IEnumerator CheckMatch()
    {

        yield return new WaitForSeconds(0.5f);

        if (firstSelectedCard.cardData.cardName == secondSelectedCard.cardData.cardName)
        {
            PlayAttackEffect(firstSelectedCard.gameObject, secondSelectedCard.gameObject);
            ApplyCardEffect(firstSelectedCard);
            yield return new WaitForSeconds(1.2f);
            firstSelectedCard.gameObject.SetActive(false);
            secondSelectedCard.gameObject.SetActive(false);

            matchedPairs++;

            OnMatchCard?.Invoke();

            if (matchedPairs == totalPairs)
            {
                yield return new WaitForSeconds(1f);
                //UIManager.instance.GameUIStageChanged(GameState.Finish);
                //UIManager.instance.IncompleteLevelPanelOpen();
                Debug.Log("bitti");
            }

        }
        else
        {
            ShakeCards(firstSelectedCard.gameObject, secondSelectedCard.gameObject);

            yield return new WaitForSeconds(0.5f);

            firstSelectedCard.HideCard();
            secondSelectedCard.HideCard();
            MoveUpdate(1);
        }

        firstSelectedCard = null;
        secondSelectedCard = null;



        if (movesRemaining <= 0)
        {
            yield return new WaitForSeconds(1.2f);

            MoveUpdate(-2);
        }

        isProcessingCards = false;

    }
    private IEnumerator CloseTrapCard(MemoryCard trapCard)
    {
        yield return new WaitForSeconds(1f);
        trapCard.HideCard();
        firstSelectedCard = null;
        MoveUpdate(1);

        if (movesRemaining <= 0)
        {
            yield return new WaitForSeconds(1.5f);

            MoveUpdate(-2);
        }

        isProcessingTrapCards = false;

    }
    private void ApplyCardEffect(MemoryCard card)
    {
        switch (card.cardData.cardEffect)
        {
            case CardEffect.None:
                break;
            case CardEffect.Trap:
                //PlayerTakeDamage(card.cardData.effectValue);
                break;
            case CardEffect.Health:
                //PlayerTakeDamage(-card.cardData.effectValue);
                break;
            case CardEffect.PowerUp:
                Debug.Log("powerup geldiiiii");
                break;
            case CardEffect.Hero:
                OnMatchHero?.Invoke(card.cardData.cardName);
                Debug.Log(card.cardData.cardName);
                break;
            case CardEffect.Coin:
                DataManager.instance.AddGold(card.cardData.effectValue);
                break;
        }

    }
    private void ShakeCards(GameObject firstCard, GameObject secondCard)
    {
        firstCard.transform.DOShakePosition(0.5f, strength: new Vector3(10, 0, 0), vibrato: 10, randomness: 90, snapping: false, fadeOut: true);
        secondCard.transform.DOShakePosition(0.5f, strength: new Vector3(10, 0, 0), vibrato: 10, randomness: 90, snapping: false, fadeOut: true);
    }

    private void PlayAttackEffect(GameObject firstCard, GameObject secondCard)
    {
        Vector3 targetPosition = new Vector3(0, 300, 0);

        float moveDuration = 0.3f;
        float attackDuration = 0.3f;

        firstCard.transform.DOMove(firstCard.transform.position + targetPosition, moveDuration).OnComplete(() =>
        {
            firstCard.transform.DOMoveY(firstCard.transform.position.x + 20f, attackDuration).SetLoops(2, LoopType.Yoyo);
        });

        secondCard.transform.DOMove(secondCard.transform.position + targetPosition, moveDuration).OnComplete(() =>
        {
            secondCard.transform.DOMoveY(secondCard.transform.position.x + 20f, attackDuration).SetLoops(2, LoopType.Yoyo);
        });
    }
    public void MoveUpdate(int damage)
    {
        movesRemaining -= damage;
        movesText.text = movesRemaining.ToString();
        movesIcon.transform.DOShakePosition(0.5f, strength: new Vector3(10, 0, 0), vibrato: 10, randomness: 90, snapping: false, fadeOut: true);

        Color originalColor = movesBackg.color;
        movesBackg.DOColor(Color.blue, 0.3f).OnComplete(() =>
        {
            movesBackg.DOColor(originalColor, 0.5f);
        });

    }
}

[Serializable]
public struct Levels
{
    public int numberOfPairs;
    public MemoryCardSO[] cards;
}
