using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Vincent.Wanderlost.Code
{
    public class AchievementProgression : MonoBehaviour
    {
        //Singleton
        public static AchievementProgression Instance;
        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
            }
            else
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
        }


        [SerializeField] private int showTime;
        [SerializeField] private GameObject AchievementPanel;
        [SerializeField] private Image Icon;
        [SerializeField] private Text Title;

        private async Task EarnAchievementPopup(Achievement achievement)
        {
            AchievementPanel.SetActive(true);
            Title.text = achievement.Title;

            await Task.Delay(showTime);

            AchievementPanel.SetActive(false);
        }

        public async Task UpdateAchievementProgression(string achievementId, int progressionIncrease)
        {
            try
            {
                string playerAchievementDocumentPath = ("Players/"+Player.playerData.Id+"/Achievements/" + achievementId);

                Achievement achievement = await Firestore.GetDocumentAsync<Achievement>(playerAchievementDocumentPath);
                if (!achievement.Completed)
                {
                    achievement.Progression += progressionIncrease;

                    if (achievement.Progression >= achievement.Completion)
                    {
                        achievement.Completed = true;
                        await EarnAchievementPopup(achievement);
                    }
                    await Firestore.UpdateUserAchievementAsync(achievement, playerAchievementDocumentPath);
                }
            }
            catch (Exception ex)
            {
                Debug.LogException(ex);
            }
        }


    } 
}
