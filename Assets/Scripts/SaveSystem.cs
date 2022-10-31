using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using Vincent.Wanderlost.Code;

public class SaveSystem
{
    public static async Task SaveProgression(PlayerData playerData)
    {
        Debug.Log("sAVE");
        playerData.Timestamp = DateTime.Now;
        if (Social.localUser.authenticated)
        {
            Debug.Log("live");
            await Firestore.SaveUser(playerData, "Players", playerData.Id);
        }
        Debug.Log("local");
        LocalStorageManager.SaveData(playerData, playerData.Id);
    }
}
