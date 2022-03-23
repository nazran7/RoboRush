using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerItemPicker : MonoBehaviour
{
    #region singleton
    public static PlayerItemPicker singleton { get; private set; }
    private void Awake()
    {
        singleton = this;
    }
    #endregion
    [SerializeField] private int coinLayer;


    public delegate void PlayerCoinTakeEvent(int count);
    public PlayerCoinTakeEvent OnCoinTake;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == coinLayer)
        {
            OnCoinTake?.Invoke(1);
            Destroy(collision.gameObject);
        }
    }
}
