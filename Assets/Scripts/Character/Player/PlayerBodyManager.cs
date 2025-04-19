using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBodyManager : MonoBehaviour
{
    PlayerManager player;
    [Header("Hair")]
    [SerializeField] public GameObject hair;
    [SerializeField] private GameObject[] hairObjects;
    [SerializeField] public GameObject facialHair;

    [Header("Male")]
    [SerializeField] public GameObject maleObject;
    [SerializeField] public GameObject maleHead;
    [SerializeField] public GameObject[] maleBody; //chest, upper right and left arm
    [SerializeField] public GameObject[] maleArms; //lower right and left arm, right and left hand
    [SerializeField] public GameObject[] maleLegs; //right and left leg, hip
    [SerializeField] public GameObject maleEyebrows;
    [SerializeField] public GameObject maleFacialHair;

    [Header("Female")]
    [SerializeField] public GameObject femaleObject;
    [SerializeField] public GameObject femaleHead;
    [SerializeField] public GameObject[] femaleBody;
    [SerializeField] public GameObject[] femaleArms;
    [SerializeField] public GameObject[] femaleLegs;
    [SerializeField] public GameObject femaleEyebrows;

    private void Awake()
    {
        player = GetComponent<PlayerManager>();
    }


    public void EnableHead()
    {
        maleHead.SetActive(true);
        femaleHead.SetActive(true);

        maleEyebrows.SetActive(true);
        femaleEyebrows.SetActive(true);
    }

    public void DisableHead()
    {
        maleHead.SetActive(false);
        femaleHead.SetActive(false);

        maleEyebrows.SetActive(false);
        femaleEyebrows.SetActive(false);
    }

    public void EnableHair()
    {
        hair.SetActive(true);
    }

    public void DisableHair()
    {
        hair.SetActive(false);
    }

    public void EnableFacialHair()
    {
        facialHair.SetActive(true);
    }

    public void DisableFacialHair()
    {
        facialHair.SetActive(false);
    }

    public void EnableBody()
    {
        foreach (var model in maleBody)
        {
            model.SetActive(true);
        }
        foreach (var model in femaleBody)
        {
            model.SetActive(true);
        }
    }

    public void DisableBody()
    {
        foreach (var model in maleBody)
        {
            model.SetActive(false);
        }
        foreach (var model in femaleBody)
        {
            model.SetActive(false);
        }
    }

    public void EnableArms()
    {
        foreach (var model in maleArms)
        {
            model.SetActive(true);
        }
        foreach (var model in femaleArms)
        {
            model.SetActive(true);
        }
    }

    public void DisableArms()
    {
        foreach (var model in maleArms)
        {
            model.SetActive(false);
        }
        foreach (var model in femaleArms)
        {
            model.SetActive(false);
        }
    }

    public void EnableLowerBody()
    {
        foreach (var model in maleLegs)
        {
            model.SetActive(true);
        }
        foreach (var model in femaleLegs)
        {
            model.SetActive(true);
        }
    }

    public void DisableLowerBody()
    {
        foreach (var model in maleLegs)
        {
            model.SetActive(false);
        }
        foreach (var model in femaleLegs)
        {
            model.SetActive(false);
        }
    }

    public void ToggleBodyType(bool isMale)
    {
        if (isMale)
        {
            maleObject.SetActive(true);
            femaleObject.SetActive(false);
        }
        else
        {
            maleObject.SetActive(false);
            femaleObject.SetActive(true);
        }

        player.playerEquipmentManager.EquipArmor();
    }

    public void ToggleHairStyle(int hairType)
    {
        for (int i = 0; i < hairObjects.Length; i++)
        {
            hairObjects[i].SetActive(false);
        }

        hairObjects[hairType].SetActive(true);
    }

    public void SetHairColor()
    {
        Color32 hairColor;

        byte red = (byte)player.playerNetworkManager.hairColorRed.Value;
        byte green = (byte)player.playerNetworkManager.hairColorGreen.Value;
        byte blue = (byte)player.playerNetworkManager.hairColorBlue.Value;

        hairColor = new Color32(red, green, blue, 255);

        for (int i = 0; i < hairObjects.Length; i++)
        {
            SkinnedMeshRenderer skinMeshRenderer = hairObjects[i].GetComponent<SkinnedMeshRenderer>();

            if (skinMeshRenderer != null)
                skinMeshRenderer.material.SetColor("_Color_Hair", hairColor);
        }
    }

}
