using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerItemPicker : MonoBehaviour
{
    //singleton pattern
    #region singleton
    public static PlayerItemPicker singleton { get; private set; }
    private void Awake()
    {
        singleton = this;
    }
    #endregion
    //layer of coin game object for interaction
    [SerializeField] private int coinLayer;

    private void Start()
    {
        //subscribe on coin add event on level end
        LevelEndCheckPoint.singleton.OnCoinsAddForLevelEnd += CoinsAddOnLevelEnd;
    }

    private void OnDisable()
    {
        LevelEndCheckPoint.singleton.OnCoinsAddForLevelEnd -= CoinsAddOnLevelEnd;
    }
    //coin take event
    public delegate void PlayerCoinTakeEvent(int count);
    public PlayerCoinTakeEvent OnCoinTake;
    //if player reach coin trigger - take coin
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == coinLayer)
        {
            OnCoinTake?.Invoke(1);
            Destroy(collision.gameObject);
        }
    }
    //coins add on end level
    private void CoinsAddOnLevelEnd()
    {
        OnCoinTake?.Invoke(PlayerUI.remainTime);
    }
}
