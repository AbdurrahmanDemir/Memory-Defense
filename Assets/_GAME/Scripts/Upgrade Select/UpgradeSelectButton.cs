using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeSelectButton : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI upgradeNameText;
    [SerializeField] private Image upgradeIconImage;
    [SerializeField] private TextMeshProUGUI upgradeDescText;
    public Button selectButton;
    
    

    public void Config(Sprite icon, string name, string description)
    {
        upgradeNameText.text=name;
        upgradeIconImage.sprite= icon;
        upgradeDescText.text= description;
    }
    public Button GetButton()
    {
        return selectButton;
    }
}
