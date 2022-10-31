using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Vincent.Wanderlost.Code;

public class ShopPowerUpUI : MonoBehaviour
{
    [SerializeField] private Text nameText;
    [SerializeField] private Text Amount;
    [SerializeField] private Button button;
    [SerializeField] private Text buttonText;
    [SerializeField] private PowerUp powerUp;

    public PowerUp PowerUp { get => powerUp; set => SetupPowerUpUI(value); }

    public void SetupPowerUpUI(PowerUp powerUp)
    {
        nameText.text = powerUp.PowerName;
        if (!Player.HasPowerUp(powerUp))
        {
            Amount.text = (powerUp.Cost + "G").ToString();
            buttonText.text = "Buy";
            button.onClick.AddListener(delegate { Buy(powerUp); });
            return;
        }
        else if (Player.HasEquipped(powerUp))
        {
            buttonText.text = "Unequip";
            button.onClick.AddListener(delegate { Unequip(powerUp); });
            return;
        }
        else
        {
            buttonText.text = "Equip";
            button.onClick.AddListener(delegate { Equip(powerUp); });
            return;
        }
        
    }

    public async void Buy(PowerUp powerUp)
    {
        try
        {
            if (Player.playerData.GoldCurrency >= powerUp.Cost)
            {
                Player.AddPowerUp(powerUp);
                Player.playerData.GoldCurrency -= powerUp.Cost;

                await Firestore.BuyItem(powerUp, ("Players/" + Player.playerData.Id + "/Powerups"));
                await SaveSystem.SaveProgression(Player.playerData);

                Amount.text = "";
                FindObjectOfType<ShopSystem>().UpdateCurrency();
                EquipSetListener();
            }
        }
        catch (Exception ex)
        {
            Player.RemovePowerUp(powerUp);
            Player.playerData.GoldCurrency += powerUp.Cost;
            Debug.LogException(ex);
        }

    }
    public async void Equip(PowerUp powerUp)
    {
        try
        {
            if (Player.playerData.UnlockedPowers.FindAll(tag => tag.Equipped).Count < 3)
            {
                foreach (PowerUp item in Player.playerData.UnlockedPowers)
                {
                    if (item.Id == powerUp.Id)
                    {
                        item.Equipped = true;
                        await SaveSystem.SaveProgression(Player.playerData);
                        break;
                    }
                }
                UnequipSetListener();
            }
        }
        catch
        {

        }

    }
    public async void Unequip(PowerUp powerUp)
    {
        try
        {
            foreach (PowerUp item in Player.playerData.UnlockedPowers)
            {
                if (item.Id == powerUp.Id)
                {
                    item.Equipped = false;
                    await SaveSystem.SaveProgression(Player.playerData);
                    break;
                }
            }
            EquipSetListener();
        }
        catch
        {

        }
    }
    private void EquipSetListener()
    {
        buttonText.text = "Equip";
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(delegate { Equip(PowerUp); });
    }
    private void UnequipSetListener()
    {
        buttonText.text = "Unequip";
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(delegate { Unequip(PowerUp); });
    }
}
