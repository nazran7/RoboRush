using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    #region singleton
    public static PlayerInventory singleton { get; private set; }
    private void Awake()
    {
        singleton = this;
    }
    #endregion

    private int coins = 0;


    private void Start()
    {
        PlayerItemPicker.singleton.OnCoinTake += CoinCountChange;
        BrokenMachine.OnCoinAdd += CoinCountChange;

        coins = SaveSystem.LoadData(SaveSystem.Type.coins);
        CoinCountChange(0);
    }
    private void OnDisable()
    {
        PlayerItemPicker.singleton.OnCoinTake -= CoinCountChange;
        BrokenMachine.OnCoinAdd -= CoinCountChange;
    }

    public delegate void OnCoinsCountChangeEvent(int currentCount);
    public OnCoinsCountChangeEvent OnCoinChange;
    private void CoinCountChange(int count)
    {
        int minusCheck = coins + count;
        if (minusCheck >= 0)
        {
            coins += count;
            SaveSystem.SaveData(SaveSystem.Type.coins, coins);
            OnCoinChange?.Invoke(coins);
        }
    }


}
