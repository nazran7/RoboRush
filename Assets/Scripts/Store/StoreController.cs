using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StoreController : MonoBehaviour
{

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

    private void Start()
    {
        //ResetSave();
        UIUpdate();
    }
    public delegate void UpgradeBuyEvent(SaveSystem.Type type, int currentLevel);
    public UpgradeBuyEvent OnUpgradeBuy;

    public void WeaponUprade()
    {
        BuyUpgrage(SaveSystem.Type.weaponLevel, weaponUpgradeCost);
    }
    public void HealthUpgrade()
    {
        BuyUpgrage(SaveSystem.Type.healthLevel, healthUpgradeCost);
    }
    public void JetPackUpgrade()
    {
        BuyUpgrage(SaveSystem.Type.jetPackLevel, jetPackUpgradeCost);
    }
    public void BoostUpgrade()
    {
        BuyUpgrage(SaveSystem.Type.boostLevel, boostUpgradeCost);
    }

    private void BuyUpgrage(SaveSystem.Type type, int[] upgradeCosts)
    {
        int coins = SaveSystem.LoadData(SaveSystem.Type.coins);
        int currentLevel = SaveSystem.LoadData(type);
        if (currentLevel < upgradeCosts.Length - 1)
        {
            if (coins >= upgradeCosts[currentLevel + 1])
            {
                currentLevel++;
                coins -= upgradeCosts[currentLevel];
                SaveSystem.SaveData(SaveSystem.Type.coins, coins);
                SaveSystem.SaveData(type, currentLevel);
                UIUpdate();
            }
        }
        UIUpdate();
        OnUpgradeBuy?.Invoke(type , currentLevel);
    }
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
    private void DisplayCount(TextMeshProUGUI tmp, int count)
    {
        tmp.text = count.ToString();
    }
    private void DisplayCount(TextMeshProUGUI tmp, string text)
    {
        tmp.text = text;
    }
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

    public void ResetSave()
    {
        SaveSystem.SaveData(SaveSystem.Type.coins, 1000);
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
}