using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerUI : MonoBehaviour
{
    #region singleton
    public static PlayerUI singleton { get; private set; }
    private void Awake()
    {
        singleton = this;
    }
    #endregion
    [SerializeField] private TextMeshProUGUI coinText;
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private int timeForLevel;
    [SerializeField] private Image healthBar;
    private void Start()
    {
        PlayerInventory.singleton.OnCoinChange += CoinUIChange;
        PlayerStatus.singleton.OnTakeDamage += HealthBarChange;
        StartCoroutine(Timer());
    }
    private void OnDisable()
    {
        PlayerInventory.singleton.OnCoinChange -= CoinUIChange;
        PlayerStatus.singleton.OnTakeDamage -= HealthBarChange;
    }
    private void CoinUIChange(int count)
    {
        coinText.text = count.ToString();
    }
    #region timer
    public delegate void TimeEndEvent();
    public TimeEndEvent OnTimeEnd;
    private IEnumerator Timer()
    {
        for (int i = 0; i <= timeForLevel; i++)
        {
            timerText.text = (timeForLevel - i).ToString();
            yield return new WaitForSeconds(1f);
        }
        OnTimeEnd?.Invoke();
    }
    #endregion

    private void HealthBarChange(int currentHealth)
    {
        float healthBarState = ((float)currentHealth) / ((float)PlayerStatus.singleton.maxHealth);
        healthBar.fillAmount = healthBarState;
    }
}
