using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MenuHeroListCard : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI cardNameText;
    [SerializeField] private Image cardIconImage;
    [SerializeField] private GameObject[] cardTypes;
    public Button detailsButton;
    public int cardIndex;


    public void Config(string name, Sprite icon, string type)
    {
        cardNameText.text = name;
        cardIconImage.sprite = icon;
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
}
