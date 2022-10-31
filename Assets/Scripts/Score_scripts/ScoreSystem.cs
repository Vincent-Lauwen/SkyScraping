using UnityEngine;
using UnityEngine.UI;
using Vincent.Wanderlost.Code;

public class ScoreSystem : MonoBehaviour
{
    [SerializeField] private Text scoreText;

    private int placedBlockAmount = 0;
    private int meters = 0;

    public async void SetEndScore()
    {
        Score currentScore = new Score(meters, placedBlockAmount);

        if (Player.playerData.Highscore.Meters < currentScore.Meters)
        {
            Player.playerData.Highscore = currentScore;
        }
        
        Player.playerData.GoldCurrency += (currentScore.Meters + currentScore.BlockAmount);

        await SaveSystem.SaveProgression(Player.playerData);
        

        PlayerPrefs.SetInt("GainedGold", (currentScore.Meters + currentScore.BlockAmount));
        PlayerPrefs.SetInt("Score", currentScore.Meters);
        PlayerPrefs.SetInt("Blocks", currentScore.BlockAmount);
    }

    public void UpdateScore(float value)
    {
        placedBlockAmount++;
        this.meters += Mathf.RoundToInt(value);
        scoreText.text = (this.meters + "M").ToString();
    }
}



