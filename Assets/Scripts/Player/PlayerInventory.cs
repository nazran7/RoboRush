using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    //singleton pattern
    #region singleton
    public static PlayerInventory singleton { get; private set; }
    private void Awake()
    {
        singleton = this;
    }
    #endregion

    //count of coins
    private int coins = 0;


    private void Start()
    {
        //subscribe on coin add events
        PlayerItemPicker.singleton.OnCoinTake += CoinCountChange;
        BrokenMachine.OnCoinAdd += CoinCountChange;
        //load coin count from save system
        coins = SaveSystem.LoadData(SaveSystem.Type.coins);
        //update ui
        CoinCountChange(0);
    }
    private void OnDisable()
    {
        PlayerItemPicker.singleton.OnCoinTake -= CoinCountChange;
        BrokenMachine.OnCoinAdd -= CoinCountChange;
    }
    //coin count change event
    public delegate void OnCoinsCountChangeEvent(int currentCount);
    public OnCoinsCountChangeEvent OnCoinChange;
    //coin count change method
    private void CoinCountChange(int count)
    {
        int minusCheck = coins + count;
        if (minusCheck >= 0)
        {
            coins += count;
            //save coin count
            SaveSystem.SaveData(SaveSystem.Type.coins, coins);
            OnCoinChange?.Invoke(coins);
        }
    }


}
