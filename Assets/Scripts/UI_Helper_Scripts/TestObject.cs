using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Vincent.Wanderlost.Code;

public class TestObject : MonoBehaviour
{
    [SerializeField] private Text id;
    [SerializeField] private Text Name;
    [SerializeField] private Text currency;
    [SerializeField] private Text health;
    [SerializeField] private Text scoremeter;
    [SerializeField] private Text scoreblock;
    [SerializeField] private Text poweruptype;
    [SerializeField] private Text powerupname;


    public void Start()
    {
        id.text = Player.playerData.Id;
        Name.text = Player.playerData.Name;
        currency.text = Player.playerData.GoldCurrency.ToString();
        health.text = Player.playerData.Health.ToString();
        scoremeter.text = Player.playerData.Highscore.Meters.ToString();
        scoreblock.text = Player.playerData.Highscore.BlockAmount.ToString();
        
        if (Player.playerData.UnlockedPowers != null)
        {
            string result1 = "";
            string result2 = "";
            Debug.LogError(Player.playerData.UnlockedPowers.Count);
            foreach (var item in Player.playerData.UnlockedPowers)
            {
                result1 += item.GetType().ToString() + "\n";
                result2 += item.PowerName + "\n";
            }
            poweruptype.text = result1;
            powerupname.text = result2;
        }
    }

}
