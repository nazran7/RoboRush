using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelEndCheckPoint : MonoBehaviour
{
    //singleton pattern
    #region singleton
    public static LevelEndCheckPoint singleton { get; private set; }
    private void Awake()
    {
        singleton = this;
    }
    #endregion
    //pay for remain time on/off
    [SerializeField] private bool isRemainTimePayed;
    //number of level
    [SerializeField] public int levelNumber;
    public delegate void LevelEndEvent();
    //level end event
    public LevelEndEvent OnLevelEnd;
    //coins add on level end event
    public LevelEndEvent OnCoinsAddForLevelEnd;
    //check player on trigger for level end
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<PlayerStatus>() != null)
        {
            OnLevelEnd?.Invoke();
            if (isRemainTimePayed)
                OnCoinsAddForLevelEnd?.Invoke();
            SaveSystem.SaveData(SaveSystem.Type.openedLevels, levelNumber);
            VideoManager.instance.StartVideo();
        }
    }


}
