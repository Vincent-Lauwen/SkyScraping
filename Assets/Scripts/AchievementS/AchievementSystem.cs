using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Vincent.Wanderlost.Code
{
    public class AchievementSystem : MonoBehaviour
    {
        [SerializeField] AchievementUI achievementPrefab;
        [SerializeField] RectTransform content;
        [SerializeField] Text errorlogs;

        private List<Achievement> UserAchievements = new List<Achievement>();

        // Start is called before the first frame update
        async void Start()
        {
            await UpdateContent();
        }

        private async Task UpdateContent()
        {
            try
            {
                foreach (Transform child in content.transform)
                {
                    Destroy(child.gameObject);
                }

                BroadcastErrorMessage("");

                UserAchievements = await Firestore.GetUserAchievementsAsync("Achievements", ("Players/"+Player.playerData.Id+"/Achievements"));
                foreach (Achievement achievement in UserAchievements)
                {
                    AchievementUI achievementUI = Instantiate(achievementPrefab, content, false);
                    achievementUI.Achievement = achievement;
                }
            }
            catch (Exception ex)
            {
                BroadcastErrorMessage("No achievements available");
                Debug.LogException(ex);
            }
        }


        private void BroadcastErrorMessage(string message)
        {
            errorlogs.text = message;
        }
    } 
}
