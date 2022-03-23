using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    #region singleton
    public static SaveManager singleton { get; private set; }
    private void Awake()
    {
        if (singleton != null)
            Destroy(singleton.gameObject);
        singleton = this;
    }
    #endregion
    private void Start()
    {
        DontDestroyOnLoad(this);

        //SaveInitialization();
    }

    //private void SaveInitialization()
    //{
    //    DataToSave dts = new DataToSave();
    //    if (!SaveSystem.IsSaveExists())
    //    {
    //        //заполняем первый раз
    //        dts.CharactersList = startCharacters;
    //        dts.Gold = 0;
    //        SaveSystem.SaveData(dts);
    //    }
    //    else
    //    {
    //        dts = SaveSystem.LoadData();
    //    }
    //}
    //private void CharacterSelected(Character character)
    //{
    //    selectedCharacter = character;
    //}


}
