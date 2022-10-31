using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Vincent.Wanderlost.Code
{
    public class Leaderbord : MonoBehaviour
    {
        [SerializeField] private Text listText;
        [SerializeField] private Text errorlogs;

        private void Start()
        {
            GetWorldScoreList();
        }

        public async void GetWorldScoreList()
        {
            try
            {
                listText.text = "";
                BroadcastErrorMessage("");

                IList<IDictionary<string, object>> dataList = await Firestore.GetLeaderbordAsync("Players", 100, "Highscore.Meters", "Name", "Highscore.Meters", "Highscore.BlockAmount");
                listText.text = ListToText(dataList);
            }
            catch (Exception ex)
            {
                Debug.LogException(ex);
                BroadcastErrorMessage("No scores available");
            }
        }

        public void GetPersonalScoreList()
        {
            try
            {
                listText.text = "";
                BroadcastErrorMessage("");

                listText.text = "1. " + Player.playerData.Name + ": " + Player.playerData.Highscore.Meters + "M - " + Player.playerData.Highscore.BlockAmount + " Blocks" + "\n";
            }
            catch
            {
                BroadcastErrorMessage("No scores available");
            }
        }
        private string ListToText(IList<IDictionary<string, object>> dataList)
        {
            int placement = 1;
            string result = "";
            foreach (IDictionary<string, object> data in dataList)
            {
                // 1. Guest: 100m - 100 Blocks
                
                string name = data.Values.ElementAt(0).ToString();
                string meters = data.Values.ElementAt(1).ToString();
                string blockamount = data.Values.ElementAt(2).ToString();

                result += (placement++).ToString() + ". " + name + ": " + meters + "M - " + blockamount + " Blocks" + "\n";
                
                
                
                
            }
            return result;
        }

        private void BroadcastErrorMessage(string message)
        {
            errorlogs.text = message;
        }
    } 
}
