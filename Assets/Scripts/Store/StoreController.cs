using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class StoreController : MonoBehaviour
{
    //coin text ui
    [SerializeField] private TextMeshProUGUI coinsText;
    #region JetPackFields
    [SerializeField] private int[] jetPackUpgradeCost;
    [SerializeField] private TextMeshProUGUI jetPackLevelText;
    [SerializeField] private TextMeshProUGUI jetPackUpgradeCostText;
    #endregion
    #region BoostFields
    [SerializeField] private int[] boostUpgradeCost;
    [SerializeField] private TextMeshProUGUI boostLevelText;
    [SerializeField] private TextMeshProUGUI boostUpgradeCostText;
    #endregion
    #region HealthFields
    [SerializeField] private int[] healthUpgradeCost;
    [SerializeField] private TextMeshProUGUI healthLevelText;
    [SerializeField] private TextMeshProUGUI healthUpgradeCostText;
    #endregion
    #region WeaponFields
    [SerializeField] private int[] weaponUpgradeCost;
    [SerializeField] private TextMeshProUGUI weaponLevelText;
    [SerializeField] private TextMeshProUGUI weaponUpgradeCostText;
    #endregion

    public Button[] powerUPButtons;
    public GameObject[] arrows;

    private void Start()
    {
        UIUpdate();
        AllowPowerUps();
        ShowArrows();
    }
    //upgrade buy event
    public delegate void UpgradeBuyEvent(SaveSystem.Type type, int currentLevel);
    public UpgradeBuyEvent OnUpgradeBuy;
    //weapon upgrade buy
    public void WeaponUprade()
    {
        BuyUpgrage(SaveSystem.Type.weaponLevel, weaponUpgradeCost);
    }
    //health upgrade buy
    public void HealthUpgrade()
    {
        BuyUpgrage(SaveSystem.Type.healthLevel, healthUpgradeCost);
    }
    //jetpack upgrade buy
    public void JetPackUpgrade()
    {
        BuyUpgrage(SaveSystem.Type.jetPackLevel, jetPackUpgradeCost);
    }
    //boost upgrade buy
    public void BoostUpgrade()
    {
        BuyUpgrage(SaveSystem.Type.boostLevel, boostUpgradeCost);
    }
    //upgrade buy method
    private void BuyUpgrage(SaveSystem.Type type, int[] upgradeCosts)
    {
        int coins = SaveSystem.LoadData(SaveSystem.Type.coins);
        int currentLevel = SaveSystem.LoadData(type);
        if (currentLevel < upgradeCosts.Length - 1)
        {
            //coins count check for current upgrade
            if (coins >= upgradeCosts[currentLevel + 1])
            {
                currentLevel++;
                coins -= upgradeCosts[currentLevel];
                //save upgrade
                SaveSystem.SaveData(SaveSystem.Type.coins, coins);
                SaveSystem.SaveData(type, currentLevel);
                UIUpdate();
            }
        }
        UIUpdate();
        OnUpgradeBuy?.Invoke(type, currentLevel);
    }
    //update ui display
    private void UIUpdate()
    {
        int coins = SaveSystem.LoadData(SaveSystem.Type.coins);

        DisplayCount(coinsText, coins);

        DisplayUpgradeButton(SaveSystem.Type.jetPackLevel,
            jetPackUpgradeCost, jetPackLevelText, jetPackUpgradeCostText);
        DisplayUpgradeButton(SaveSystem.Type.boostLevel,
            boostUpgradeCost, boostLevelText, boostUpgradeCostText);
        DisplayUpgradeButton(SaveSystem.Type.healthLevel,
            healthUpgradeCost, healthLevelText, healthUpgradeCostText);
        DisplayUpgradeButton(SaveSystem.Type.weaponLevel,
            weaponUpgradeCost, weaponLevelText, weaponUpgradeCostText);

    }

    #region displayMethods
    //count display method
    private void DisplayCount(TextMeshProUGUI tmp, int count)
    {
        tmp.text = count.ToString();
    }
    //string display method
    private void DisplayCount(TextMeshProUGUI tmp, string text)
    {
        tmp.text = text;
    }
    //upgrade button ui display method
    private void DisplayUpgradeButton(SaveSystem.Type type, int[] costs, TextMeshProUGUI levelText,
        TextMeshProUGUI costText)
    {
        int level = SaveSystem.LoadData(type);
        if (level < costs.Length - 1)
        {
            DisplayCount(levelText, level + 1);
            DisplayCount(costText, costs[level + 1]);
        }
        else
        {
            DisplayCount(levelText, level + 1);
            DisplayCount(costText, "max");
        }
    }
    #endregion
    //reset progress method
    public void ResetSave()
    {
        SaveSystem.SaveData(SaveSystem.Type.coins, 0);
        SaveSystem.SaveData(SaveSystem.Type.jetPackLevel, 0);
        SaveSystem.SaveData(SaveSystem.Type.boostLevel, 0);
        SaveSystem.SaveData(SaveSystem.Type.healthLevel, 0);
        SaveSystem.SaveData(SaveSystem.Type.weaponLevel, 0);
        SaveSystem.SaveData(SaveSystem.Type.openedLevels, 0);

        OnUpgradeBuy?.Invoke(SaveSystem.Type.jetPackLevel, 0);
        OnUpgradeBuy?.Invoke(SaveSystem.Type.boostLevel, 0);
        OnUpgradeBuy?.Invoke(SaveSystem.Type.healthLevel, 0);
        OnUpgradeBuy?.Invoke(SaveSystem.Type.weaponLevel, 0);
        UIUpdate();
    }

    public void AllowPowerUps()
    {
        Debug.Log("lvevl " + SaveSystem.LoadData(SaveSystem.Type.openedLevels));
        if (SaveSystem.LoadData(SaveSystem.Type.openedLevels) > 1)
        {
            for (int i = 0; i < powerUPButtons.Length; i++)
            {
                powerUPButtons[i].interactable = false;
            }

            for (int i = 1; i < SaveSystem.LoadData(SaveSystem.Type.openedLevels); i++)
            {
                powerUPButtons[i - 1].interactable = true;
            }
        }
    }
    public void ShowArrows()
    {
        if (SaveSystem.LoadData(SaveSystem.Type.openedLevels) > 4 || SaveSystem.LoadData(SaveSystem.Type.openedLevels) < 2)
        {
            return;
        }
        for (int i = 1; i < arrows.Length; i++)
        {
            arrows[i - 1].SetActive(false);
        }
        arrows[SaveSystem.LoadData(SaveSystem.Type.openedLevels) - 2].SetActive(true);
    }
}