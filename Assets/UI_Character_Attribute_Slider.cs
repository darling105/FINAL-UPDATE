using UnityEngine;

public class UI_Character_Attribute_Slider : MonoBehaviour
{
    [SerializeField] Attribute sliderAttribute;

    public void SetCurretSelectedAttributs()
    {
        PlayerUIManager.instance.playerUILevelUpManager.currentSelectedAttribute = sliderAttribute;
    }
}
