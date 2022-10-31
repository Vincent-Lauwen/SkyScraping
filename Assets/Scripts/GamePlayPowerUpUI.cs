using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GamePlayPowerUpUI : MonoBehaviour
{
    [SerializeField] private Text timer;
    [SerializeField] private Image timeFiller;
    [SerializeField] private Button button;
    [SerializeField] private PowerUp powerUp;

    private bool isCooldown = false;
    private float currentCooldownTime = 0.0f;

    public PowerUp PowerUp
    {
        get { return powerUp; }
        set { SetupPowerUpUI(value); }
    }

    public void OnEnable()
    {
        currentCooldownTime = 0.0f;
    }

    private void SetupPowerUpUI(PowerUp newPowerUp)
    {
        button.onClick.RemoveAllListeners();
        //if (!newPowerUp)
        //{
        //    gameObject.SetActive(false);
        //    return;
        //}
        //else
        //{
        //    gameObject.SetActive(true);
        //    this.powerUp = newPowerUp;
        //    timer.gameObject.SetActive(false);
        //    timeFiller.fillAmount = 0.0f;
        //    timer.text = (powerUp.Cooldown).ToString();
        //    button.onClick.AddListener(delegate { ExecutePowerUp(this.powerUp); });
        //}
    }

    private void Update()
    {
        if (isCooldown)
        {
            ApplyCooldown();
        }
    }

    private void ApplyCooldown()
    {
        currentCooldownTime -= Time.deltaTime;

        if (currentCooldownTime < 0.0f)
        {
            isCooldown = false;
            timer.gameObject.SetActive(false);
            timeFiller.fillAmount = 0.0f;
        }
        else
        {
            timer.text = Mathf.RoundToInt(currentCooldownTime).ToString();
            timeFiller.fillAmount = currentCooldownTime / this.powerUp.Cooldown;
        }
    }

    private void ExecutePowerUp(PowerUp powerUp)
    {
        if (isCooldown)
        {
            return;
        }
        else
        {
            //execute timer
            isCooldown = true;
            timer.gameObject.SetActive(true);
            currentCooldownTime = powerUp.Cooldown;

            //execute Effect
            powerUp.Use();
        }
    }

    
}
