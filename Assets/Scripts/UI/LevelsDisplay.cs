using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelsDisplay : MonoBehaviour
{
    [SerializeField] private GameObject[] levels;


    private void Start()
    {
        for (int i = 0; i <= OpenedLevels(); i++)
        {
            levels[i].SetActive(true);
        }
    }
    private int OpenedLevels()
    {
        return SaveSystem.LoadData(SaveSystem.Type.openedLevels);
    }
}
