using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using Newtonsoft.Json;
using System.Reflection;
using System.Linq;
using System.Threading.Tasks;
using Firebase.Auth;

public class LocalStorageManager : MonoBehaviour
{
    public static void SaveData(object data, string fileName)
    {
        string path = Application.persistentDataPath + "/" + fileName + ".json";
        string json = JsonConvert.SerializeObject(data, Formatting.None);
        File.WriteAllText(path, json);
    }

    public static T LoadData<T>(string fileName)
    {
        string path = Application.persistentDataPath + "/" + fileName + ".json";
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            T data = JsonConvert.DeserializeObject<T>(json);
            return data;
        }
        return default;
    }

    

    public static void RemoveFile(string fileName)
    {
        string FullFilePath = Application.persistentDataPath + "/" + fileName;
        if (File.Exists(FullFilePath))
        {
            File.Delete(FullFilePath);
        }
    }

    public static string getLatestSave()
    {
        string fileName = null;

        try
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(Application.persistentDataPath);
            FileInfo[] saveFiles = directoryInfo.GetFiles("*.json");
            FileInfo mostRecentFile = null;
            foreach (FileInfo fileInfo in saveFiles)
            {
                if (mostRecentFile == null)
                {
                    mostRecentFile = fileInfo;
                }
                else if (fileInfo.LastWriteTime > mostRecentFile.LastWriteTime)
                {
                    mostRecentFile = fileInfo;

                }
            }
            fileName = Path.GetFileNameWithoutExtension((Application.persistentDataPath + "/" + mostRecentFile.Name));
            return fileName;
        }
        catch (Exception ex)
        {
            Debug.LogException(ex);
            return fileName;
        }
    }

    
}
