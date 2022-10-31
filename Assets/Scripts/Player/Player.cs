using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Firebase.Firestore;
using Firebase.Extensions;
using UnityEngine.Events;
using System;

namespace Vincent.Wanderlost.Code
{
    public class Player
    {
        public static PlayerData playerData;

        public static void AddPowerUp(PowerUp powerUp)
        {
            playerData.UnlockedPowers.Add(powerUp);
        }
        public static void RemovePowerUp(PowerUp powerUp)
        {
            playerData.UnlockedPowers.Remove(powerUp);
        }
        public static bool HasPowerUp(PowerUp powerUp)
        {
            try
            {
                foreach (PowerUp item in playerData.UnlockedPowers)
                {
                    if (item != null && item.Id == powerUp.Id)
                    {
                        return true;
                    }
                }
                return false;
            }
            catch
            {
                return false;
            }
        }
        public static bool HasEquipped(PowerUp powerUp)
        {
            try
            {
                foreach (PowerUp item in playerData.UnlockedPowers)
                {
                    if (item != null && item.Id == powerUp.Id && item.Equipped)
                    {
                        return true;
                    }
                }
                return false;
            }
            catch
            {
                return false;
            }
        }
    } 
}
