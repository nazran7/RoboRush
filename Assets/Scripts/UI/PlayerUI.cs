using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerUI : MonoBehaviour
{
    //singleton pattern
    #region singleton
    public static PlayerUI singleton { get; private set; }
    private void Awake()
    {
        singleton = this;
    }
    #endregion
    //all ui elements
    [SerializeField] private TextMeshProUGUI coinText;
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private int timeForLevel;
    [SerializeField] private Image healthBar;
    //remain time static
    public static int remainTime;
    private void Start()
    {
        //subscribe on coin change event
        PlayerInventory.singleton.OnCoinChange += CoinUIChange;
        //subscribe on health change event
        PlayerStatus.singleton.OnTakeDamage += HealthBarChange;
        StartCoroutine(Timer());
    }
    private void OnDisable()
    {
        PlayerInventory.singleton.OnCoinChange -= CoinUIChange;
        PlayerStatus.singleton.OnTakeDamage -= HealthBarChange;
    }
    //coin ui change method
    private void CoinUIChange(int count)
    {
        coinText.text = count.ToString();
    }
    #region timer
    //time end event
    public delegate void TimeEndEvent();
    public TimeEndEvent OnTimeEnd;
    //timer for level
    private IEnumerator Timer()
    {
        for (int i = 0; i <= timeForLevel; i++)
        {
            remainTime = timeForLevel - i;
            timerText.text = (timeForLevel - i).ToString();
            yield return new WaitForSeconds(1f);
        }
        //if time end - player lose
        OnTimeEnd?.Invoke();
    }
    #endregion
    //health bar ui update method
    private void HealthBarChange(int currentHealth)
    {
        float healthBarState = ((float)currentHealth) / ((float)PlayerStatus.singleton.maxHealth);
        healthBar.fillAmount = healthBarState;
    }
}
