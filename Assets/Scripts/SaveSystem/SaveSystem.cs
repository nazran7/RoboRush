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
    public static void SaveCoins(int levelNumber, int coins)
    {
        for (int i = 1; i < 10; i++)
        {
            PlayerPrefs.SetInt("Coins" + i.ToString(), 0);
        }

        PlayerPrefs.SetInt("Coins" + levelNumber.ToString(), 0);

        PlayerPrefs.SetInt("Coins" + levelNumber.ToString(), coins);
        SaveData(Type.coins, GetCoins());
    }
    public static int GetCoins()
    {
        int totalCoins = 0;
        for (int i = 1; i < 10; i++)
        {
            totalCoins += PlayerPrefs.GetInt("Coins" + i);
        }
        return totalCoins;
    }
    //load data
    public static int LoadData(Type type)
    {
        return PlayerPrefs.GetInt(type.ToString());
    }
    public static void CheckCoins(int level)
    {
        if (PlayerPrefs.GetInt("Coins" + level.ToString()) > 0)
        {
            PlayerPrefs.SetInt(Type.coins.ToString(), PlayerPrefs.GetInt(Type.coins.ToString()) - PlayerPrefs.GetInt("Coins" + level.ToString()));
            PlayerPrefs.SetInt("Coins" + level, 0);
        }
    }
}
