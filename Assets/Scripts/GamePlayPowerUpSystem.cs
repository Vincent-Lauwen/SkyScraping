using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vincent.Wanderlost.Code;

public class GamePlayPowerUpSystem : MonoBehaviour
{
    [SerializeField] GamePlayPowerUpUI powerUpUIPrefab;
    [SerializeField] RectTransform powerUpContent;
    [SerializeField] List<GamePlayPowerUpUI> powerUp_UI_list;
    [SerializeField] private List<PowerUp> powerUps;

    // Start is called before the first frame update
    void Start()
    {
        Init();
    }

    private void Init()
    {
        powerUpContent.GetComponentsInChildren<GamePlayPowerUpUI>(includeInactive: true, result: powerUp_UI_list);
        powerUps = Player.playerData.UnlockedPowers.FindAll(tag => tag.Equipped);
        UpdateContent();
        
    }
    private void UpdateContent()
    {
        //PowerUps
        for (int i = 0; i < powerUps.Count; i++)
        {
            if (powerUp_UI_list.Count == i)
            {
                powerUp_UI_list.Add(Instantiate(powerUpUIPrefab, powerUpContent, false));
            }
            else if (powerUp_UI_list[i] == null)
            {
                powerUp_UI_list[i] = Instantiate(powerUpUIPrefab, powerUpContent, false);
            }
            powerUp_UI_list[i].PowerUp = powerUps[i];
        }
        for (int i = powerUps.Count; i < powerUp_UI_list.Count; i++)
        {
            powerUp_UI_list[i].PowerUp = null;
        }
    }
}
