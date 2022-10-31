using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Firestore;

namespace Vincent.Wanderlost.Code
{
    [FirestoreData]
    public class Achievement
    {
        [FirestoreProperty]
        public string Id { get; set; }
        [FirestoreProperty]
        public string Title { get; set; } = "Missing Title";
        [FirestoreProperty]
        public string Description { get; set; } = "Missing Description";
        [FirestoreProperty]
        public int Progression { get; set; } = 0;
        [FirestoreProperty]
        public int Completion { get; set; } = 1;
        [FirestoreProperty]
        public bool Completed { get; set; } = false;
    } 
}
