using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Vincent.Wanderlost.Code;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Slider healthSlider;

    public void SetHealth(int value)
    {
        healthSlider.maxValue = value;
        healthSlider.value = value;
    }

    public void UpdateHealth(int value)
    {
        healthSlider.value += value;
    }
}
