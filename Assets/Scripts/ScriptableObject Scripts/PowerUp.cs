using Firebase.Firestore;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Firebase.Extensions;
using System;


[FirestoreData]
[Serializable]
public class PowerUp
{
    [FirestoreProperty]
    public string Id { get; set; }
    [FirestoreProperty]
    public string PowerName { get; set; } = "null";
    [FirestoreProperty]
    public int Cost { get; set; } = 0;
    [FirestoreProperty]
    public float Cooldown { get; set; } = 0f;
    [FirestoreProperty]
    public bool Equipped { get; set; } = false;

    public virtual void Use()
    {

    }
}

public class PowerUpData
{
    
}
