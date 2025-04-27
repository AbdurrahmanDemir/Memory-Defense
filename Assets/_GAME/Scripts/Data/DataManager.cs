using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class DataManager : MonoBehaviour
{
    public static DataManager instance;

    [Header(" Data ")]
    [SerializeField] private int gold;
    [SerializeField] private int xp;
    [SerializeField] private int energy;
    [Header(" Text ")]
    [SerializeField] private TextMeshProUGUI[] GoldText;
    [SerializeField] private TextMeshProUGUI[] XpText;
    [SerializeField] private TextMeshProUGUI[] EnergyText;

    private GameObject popUp;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        LoadData();

    }


    public bool TryPurchaseGold(int price)
    {
        if (price <= gold)
        {
            gold -= price;
            SaveData();
            UpdateGoldText();
            return true;
        }
        else
        {
            PopUpController.instance.OpenPopUp("NOT ENOUGH GOLD");

        }
        return false;
    }

    public void AddGold(int value)
    {
        gold += value;
        UpdateGoldText();
        SaveData();
    }

    public void AddXP(int value)
    {
        xp += value;
        UpdateXPText();
        SaveData();
    }

    private void UpdateGoldText()
    {
        for (int i = 0; i < GoldText.Length; i++)
        {
            GoldText[i].text= gold.ToString();
        }
    }

    private void UpdateXPText()
    {
        //TextMeshProUGUI xpText = GameObject.FindGameObjectWithTag("XpText").GetComponent<TextMeshProUGUI>();
        //xpText.text = xp.ToString();
        for (int i = 0; i < XpText.Length; i++)
        {
            XpText[i].text = xp.ToString();
        }

    }

    private void SaveData()
    {
        PlayerPrefs.SetInt("Gold", gold);
        PlayerPrefs.SetInt("XP", xp);
        PlayerPrefs.SetInt("Energy", energy);
    }

    private void LoadData()
    {
        if (PlayerPrefs.HasKey("Gold"))
        {
            gold = PlayerPrefs.GetInt("Gold");
        }
        else
        {
            AddGold(100);
            AddEnergy(5);
        }
        xp = PlayerPrefs.GetInt("XP", 0);
        energy = PlayerPrefs.GetInt("Energy", energy);

        Debug.Log("GOLD" + gold + "XP" + xp);

        SaveData();
        UpdateGoldText();
        UpdateXPText();
        UpdateEnergyText();
    }

    // Energy

    public bool TryPurchaseEnergy(int price)
    {
        if (price <= energy)
        {
            energy -= price;
            SaveData();
            UpdateEnergyText();
            return true;
        }
        else
        {
            PopUpController.instance.OpenPopUp("NOT ENOUGH ENERGY");
        }
        return false;
    }

    private void UpdateEnergyText()
    {
        for (int i = 0; i < EnergyText.Length; i++)
        {
            EnergyText[i].text = energy.ToString();
        }
    }

    public void AddEnergy(int value)
    {
        energy += value;
        UpdateEnergyText();
        SaveData();
    }
}
