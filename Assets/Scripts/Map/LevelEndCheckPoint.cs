using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelEndCheckPoint : MonoBehaviour
{
    #region singleton
    public static LevelEndCheckPoint singleton { get; private set; }
    private void Awake()
    {
        singleton = this;
    }
    #endregion

    public delegate void LevelEndEvent();
    public LevelEndEvent OnLevelEnd;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<PlayerStatus>() != null)
        {
            OnLevelEnd?.Invoke();
        }
    }
}
