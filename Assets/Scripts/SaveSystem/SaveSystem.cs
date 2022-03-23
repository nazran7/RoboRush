using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem
{
    public static void SaveData(DataToSave dts)
    {
        string path = Application.persistentDataPath + "/save.sav";

        BinaryFormatter formater = new BinaryFormatter();
        FileStream stream = new FileStream(path, FileMode.Create);

        formater.Serialize(stream, dts);
        stream.Close();
    }

    public static DataToSave LoadData()
    {
        string path = Application.persistentDataPath + "/save.sav";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            DataToSave dts = formatter.Deserialize(stream) as DataToSave;

            return dts;
        }
        else
        {
            Debug.LogError("Save dosn't exists");
            return null;
        }
    }
    public static bool IsSaveExists()
    {
        string path = Application.persistentDataPath + "/save.sav";
        if (!File.Exists(path))
            return false;
        else
            return true;
    }
}
