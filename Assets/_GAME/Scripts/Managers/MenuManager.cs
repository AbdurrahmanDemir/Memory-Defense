using UnityEngine;
using TMPro;
using UnityEngine.UI;
using DG.Tweening;

public class MenuManager : MonoBehaviour
{
    [Header("Arena Settings")]
    int arenaIndex;
    int completedArenaIndex;
    [SerializeField] private TextMeshProUGUI arenaName;
    [SerializeField] private Image arenaImage;
    [SerializeField] private TextMeshProUGUI arenaDescription;

    [Header("Arena Elements")]
    [SerializeField] private string[] arenaNames;
    [SerializeField] private Sprite[] arenaImages;
    [SerializeField] private string[] arenaDescriptions;

    private RectTransform imageRect;

    private void Start()
    {
        imageRect = arenaImage.GetComponent<RectTransform>();
        arenaIndex = PlayerPrefs.GetInt("WaveIndex", 0);
        completedArenaIndex = arenaIndex;
        ArenaPanelUpdate(arenaIndex, instant: true);
    }

    public void ArenaPanelUpdate(int index, int direction = 1, bool instant = false)
    {
        arenaName.text = arenaNames[index];

        if (!instant)
        {
            float width = 800f; 

            Sequence seq = DOTween.Sequence();
            seq.Append(imageRect.DOAnchorPosX(-direction * width, 0.1f))
                .AppendCallback(() =>
                {
                    arenaImage.sprite = arenaImages[index];
                    imageRect.anchoredPosition = new Vector2(direction * width, imageRect.anchoredPosition.y); 
                })
                .Append(imageRect.DOAnchorPosX(0, 0.1f));
        }
        else
        {
            arenaImage.sprite = arenaImages[index];
        }

        if (index == completedArenaIndex)
        {
            arenaDescription.text = arenaDescriptions[index];
        }
        else if (index > completedArenaIndex)
        {
            arenaDescription.text = "Complete the previous arena to play in this arena.";
        }
        else if (index < completedArenaIndex)
        {
            arenaDescription.text = "You have completed this arena.";
        }
    }


    public void ArenaChangeLeft()
    {
        if (arenaIndex == 0)
            return;
        arenaIndex--;
        ArenaPanelUpdate(arenaIndex, direction: -1);
    }

    public void ArenaChangeRight()
    {
        if (arenaIndex + 1 == arenaNames.Length)
            return;
        arenaIndex++;
        ArenaPanelUpdate(arenaIndex, direction: 1);
    }

}
