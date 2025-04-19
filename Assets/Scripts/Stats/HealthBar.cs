using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider slider;
    public RectTransform rectTransform;

    [Header("Bar Option")]
    [SerializeField] protected bool scaleBarLengthWithStats = true;
    [SerializeField] protected float widthScaleMultiplier = 1;
     private void Start()
    {
        slider = GetComponent<Slider>();
    }

    public void SetMaxHealth(int maxHealth)
    {
        slider.maxValue = maxHealth;
        slider.value = maxHealth;
        if (scaleBarLengthWithStats)
        {
            rectTransform.sizeDelta = new Vector2(maxHealth * widthScaleMultiplier, rectTransform.sizeDelta.y);
        }
    }

    public void SetCurrentHealth(int currentHealth)
    {
        slider.value = currentHealth;
    }
}
