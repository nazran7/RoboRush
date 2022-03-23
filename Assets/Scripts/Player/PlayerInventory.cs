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
    }
    private void OnDisable()
    {
        PlayerItemPicker.singleton.OnCoinTake -= CoinCountChange;
    }

    public delegate void OnCoinsCountChangeEvent(int currentCount);
    public OnCoinsCountChangeEvent OnCoinChange;
    private void CoinCountChange(int count)
    {
        int minusCheck = coins + count;
        if (minusCheck > 0)
        {
            coins += count;
            OnCoinChange?.Invoke(coins);
        }
    }

}
