using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelsDisplay : MonoBehaviour
{
    //all levels
    [SerializeField] private GameObject[] levels;


    private void OnEnable()
    {
        //display opened levels
        for (int i = 0; i <= OpenedLevels(); i++)
        {
            levels[i].SetActive(true);
        }
    }
    //load count of opened levels
    private int OpenedLevels()
    {
        return SaveSystem.LoadData(SaveSystem.Type.openedLevels);
    }
    private void OnDisable()
    {
        for (int i = 1; i <= OpenedLevels(); i++)
        {
            levels[i].SetActive(false);
        }
    }
}
