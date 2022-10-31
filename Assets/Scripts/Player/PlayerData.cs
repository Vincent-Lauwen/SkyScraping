using Firebase.Firestore;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Vincent.Wanderlost.Code
{
    [FirestoreData]
    [Serializable]
    public class PlayerData
    {
        [FirestoreProperty]
        public string Id { get; set; }
        [FirestoreProperty]
        public string Name { get; set; } = "Guest";
        [FirestoreProperty]
        public int Health { get; set; } = 100;
        public int CurrentHealth { get; set; }
        [FirestoreProperty]
        public int GoldCurrency { get; set; } = 0;
        [FirestoreProperty]
        public DateTime Timestamp { get; set; }
        [FirestoreProperty]
        public Score Highscore { get; set; } = new Score(0, 0);


        public List<Achievement> Achievements { get; set; }
        
        public List<PowerUp> UnlockedPowers { get; set; }

        public PlayerData(string id, string name, int health, int currency, Score score, List<PowerUp> powerUps, List<Achievement> achievements)
        {
            Id = id;
            Name = name;
            Health = health;
            GoldCurrency = currency;
            Highscore = score;
            UnlockedPowers = powerUps;
            Achievements = achievements;
        }

        public PlayerData()
        {

        }
    } 
}
