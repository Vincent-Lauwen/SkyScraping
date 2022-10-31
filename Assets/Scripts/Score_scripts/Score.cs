using Firebase.Firestore;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Vincent.Wanderlost.Code
{
    [FirestoreData]
    [Serializable]
    public class Score
    {
        [FirestoreProperty]
        public int Meters { get; set; }
        [FirestoreProperty]
        public int BlockAmount { get; set; }

        public Score(int meters, int blockAmount)
        {
            Meters = meters;
            BlockAmount = blockAmount;
        }

        public Score()
        {

        }
    } 
}
