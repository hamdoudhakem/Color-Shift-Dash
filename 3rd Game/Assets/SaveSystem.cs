using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public static class SaveSystem
{
    public static void Save()
    {
        BinaryFormatter formatter = new BinaryFormatter();

        PlayerData data = new PlayerData();

        string path = Application.persistentDataPath + "/Something.ay";

        FileStream stream = new FileStream(path, FileMode.Create);

        formatter.Serialize(stream, data);

        stream.Close();
    }

    public static void Load()
    {
        string path = Application.persistentDataPath + "/Something.ay";

        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();

            FileStream stream = new FileStream(path, FileMode.Open);

            PlayerData data = formatter.Deserialize(stream) as PlayerData;           

            stream.Close();

            data.Assign();
        }
        else
        {
            Debug.LogWarning("You Don't Have Any Save");
        }
            
    }
}
