using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelsUI : MonoBehaviour
{
    //panels for lose and win screen
    [SerializeField] private GameObject panel;
    [SerializeField] private GameObject loseMenu;
    [SerializeField] private GameObject winMenu;

    private void Start()
    {
        //subscribe on all event that controls panels
        PlayerStatus.singleton.OnPlayerDeath += ShowLosePanel;
        PlayerUI.singleton.OnTimeEnd += ShowLosePanel;
        LevelEndCheckPoint.singleton.OnLevelEnd += ShowWinPanel;
        Time.timeScale = 1.0f;
    }
    private void OnDisable()
    {
        PlayerStatus.singleton.OnPlayerDeath -= ShowLosePanel;
        PlayerUI.singleton.OnTimeEnd -= ShowLosePanel;
        LevelEndCheckPoint.singleton.OnLevelEnd -= ShowWinPanel;
    }
    //show lose panel and pause game 
    private void ShowLosePanel()
    {
        Time.timeScale = 0.0f;
        panel.SetActive(true);
        loseMenu.SetActive(true);
    }
    //show win panel and pause game 
    private void ShowWinPanel()
    {
        Time.timeScale = 0.0f;
        panel.SetActive(true);
        winMenu.SetActive(true);
    }
}
