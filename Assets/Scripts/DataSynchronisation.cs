using Firebase.Auth;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;
using Vincent.Wanderlost.Code;

public class DataSynchronisation
{
    public static void SynchronizeLiveWithLocalDatabase(PlayerData liveData, string fileName)
    {
        try
        {
            string path = Application.persistentDataPath + "/" + fileName + ".json";
            string json = JsonConvert.SerializeObject(liveData, Formatting.None);
            File.WriteAllText(path, json);
        }
        catch (Exception ex)
        {
            throw new Exception("Could not sync live to local data", ex);
        }
    }

    public static async Task SynchronizeLocalWithLiveDatabase(PlayerData localData, FirebaseUser user, params string[] fields)
    {
        try
        {
            //incase anonymous data is synced with live data and thus no ID and Name are available, we use the new Firebase user.
            localData.Id = user.UserId;
            localData.Name = user.DisplayName;

            await Firestore.MergeDataInDocumentAsync(localData, ("Players/" + localData.Id), fields);
        }
        catch (Exception ex)
        {
            throw new Exception("Could not sync local to live data", ex);
        }
    }
}
