using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveSystem : MonoBehaviour
{
    //type of saved data
    public enum Type
    {
        coins,
        jetPackLevel,
        boostLevel,
        healthLevel,
        weaponLevel,
        openedLevels
    }
    //save data
    public static void SaveData(Type type, int currentCount)
    {
        PlayerPrefs.SetInt(type.ToString(), currentCount);
    }
    //load data
    public static int LoadData(Type type)
    {
        return PlayerPrefs.GetInt(type.ToString());
    }

}
