using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Vincent.Wanderlost.Code;

public class GameOverSystem : MonoBehaviour
{
    [SerializeField] Text scoreText, highscoreText, CurrencyText;

    private void Start()
    {
        try
        {
            scoreText.text = ("Score:\n" + PlayerPrefs.GetInt("Score").ToString() + " Meters\n" + PlayerPrefs.GetInt("Blocks").ToString() + " Blocks");
            highscoreText.text = ("Highscore:\n" + Player.playerData.Highscore.Meters + " Meters\n" + Player.playerData.Highscore.BlockAmount + " Blocks");
            CurrencyText.text = ("Currency:\n" + PlayerPrefs.GetInt("GainedGold").ToString() +"+ Gold");
        }
        catch (System.Exception ex)
        {
            Debug.LogException(ex);
        }
    }
}
