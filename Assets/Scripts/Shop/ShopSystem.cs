using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using Vincent.Wanderlost.Code;

public class ShopSystem : MonoBehaviour
{
    [SerializeField] ShopPowerUpUI powerUpUIPrefab;
    [SerializeField] RectTransform powerUpContent;
    [SerializeField] Text currencyText;
    [SerializeField] Text errorlogs;

    public IList<PowerUp> powerUps;


    private async void Start()
    {
        await UpdateContent();
        UpdateCurrency();
    }
    private async Task UpdateContent()
    {
        try
        {
            foreach (Transform child in powerUpContent.transform)
            {
                Destroy(child.gameObject);
            }
            BroadcastErrorMessage("");

            powerUps = await Firestore.GetShopItems("Powerups");

            foreach (PowerUp powerUp in powerUps)
            {
                if (powerUp == null || (!Player.HasPowerUp(powerUp) && !Social.localUser.authenticated))
                {
                    continue;
                }
                ShopPowerUpUI powerUpUI = Instantiate(powerUpUIPrefab, powerUpContent, false);
                powerUpUI.PowerUp = powerUp;
            }
        }
        catch (Exception ex)
        {
            BroadcastErrorMessage("No items available");
            Debug.LogException(ex);
        }
        
    }

    public void UpdateCurrency()
    {
        try
        {
            currencyText.text = (Player.playerData.GoldCurrency + "G").ToString();
        }
        catch
        {

        }
            
    }
    private void BroadcastErrorMessage(string message)
    {
        errorlogs.text = message;
    }
}
