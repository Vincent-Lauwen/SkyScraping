using Firebase.Firestore;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vincent.Wanderlost.Code;


[FirestoreData]
[Serializable]
public class HealEffect : PowerUp
{
    [SerializeField] private int healingEffect;

    [FirestoreProperty][SerializeField]
    public int HealingEffect1 { get => healingEffect; set => healingEffect = value; }

    public override void Use()
    {
        Player.playerData.Health += healingEffect;
    }
}