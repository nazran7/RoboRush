using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelsUI : MonoBehaviour
{
    [SerializeField] private GameObject panel;
    [SerializeField] private GameObject loseMenu;
    [SerializeField] private GameObject winMenu;

    private void Start()
    {
        PlayerStatus.singleton.OnPlayerDeath += ShowLosePanel;
        PlayerUI.singleton.OnTimeEnd += ShowLosePanel;
        LevelEndCheckPoint.singleton.OnLevelEnd += ShowWinPanel;
    }
    private void OnDisable()
    {
        PlayerStatus.singleton.OnPlayerDeath -= ShowLosePanel;
        PlayerUI.singleton.OnTimeEnd -= ShowLosePanel;
        LevelEndCheckPoint.singleton.OnLevelEnd -= ShowWinPanel;
    }

    private void ShowLosePanel()
    {
        panel.SetActive(true);
        loseMenu.SetActive(true);
    }
    private void ShowWinPanel()
    {
        panel.SetActive(true);
        winMenu.SetActive(true);
    }
}
