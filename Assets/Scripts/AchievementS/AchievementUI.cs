using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Vincent.Wanderlost.Code
{
    public class AchievementUI : MonoBehaviour
    {
        [SerializeField] private Image unlockedImage;
        [SerializeField] private Sprite locked, unlocked;

        [SerializeField] private Text title;
        [SerializeField] private Text description;
        [SerializeField] private Achievement achievement;
        [SerializeField] private Text progression;
        [SerializeField] private Slider progressionSlider;

        public Achievement Achievement { get => achievement; set => SetAchievement(value); }

        private void SetAchievement(Achievement achievement)
        {
            title.text = achievement.Title;
            description.text = achievement.Description;
            progression.text = (achievement.Progression + "/" + achievement.Completion).ToString();
            progressionSlider.maxValue = achievement.Completion;
            progressionSlider.value = achievement.Progression;
            if (achievement.Completed)
            {
                unlockedImage.sprite = unlocked;
            }
            else
            {
                unlockedImage.sprite = locked;
            }
        }
    } 
}
