using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StaminaBar : MonoBehaviour
{ public Slider slider;
    public RectTransform rectTransform;

    [Header("Bar Option")]
    [SerializeField] protected bool scaleBarLengthWithStats = true;
    [SerializeField] protected float widthScaleMultiplier = 1;
    private void Start()
    {
        slider = GetComponent<Slider>();
    }

    public void SetMaxStamina(float maxStamina)
    {
        slider.maxValue = maxStamina;
        slider.value = maxStamina;
        if (scaleBarLengthWithStats)
        {
            rectTransform.sizeDelta = new Vector2(maxStamina * widthScaleMultiplier, rectTransform.sizeDelta.y);
        }
    }

    public void SetCurrentStamina(float currentStamina)
    {
        slider.value = currentStamina;
    }
}
