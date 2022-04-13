using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveSystem : MonoBehaviour
{
    public enum Type
    {
        coins,
        jetPackLevel,
        boostLevel,
        healthLevel,
        weaponLevel,
        openedLevels
    }
    public static void SaveData(Type type, int currentCount)
    {
        PlayerPrefs.SetInt(type.ToString(), currentCount);
    }
    public static int LoadData(Type type)
    {
        return PlayerPrefs.GetInt(type.ToString());
    }

}
