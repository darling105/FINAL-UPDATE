using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Unity.Netcode;
using Unity.VisualScripting;
using TMPro;

public class TitleScreenManager : MonoBehaviour
{
    public static TitleScreenManager instance;
    [Header("Menus")]
    [SerializeField] GameObject titleScreenMainMenu;
    [SerializeField] GameObject titleScreenLoadGameMenu;
    [SerializeField] GameObject titleScreenCharacterCreationMenu;

    [Header("Main Menu Button")]
    [SerializeField] Button loadMenuReturnButton;
    [SerializeField] Button mainMenuNewGameButton;
    [SerializeField] Button mainMenuLoadGameButton;
    [SerializeField] Button deletaCharacterPopUpConfirmButton;

    [Header("Pop Ups")]
    [SerializeField] GameObject noCharacterSlotsPopUp;
    [SerializeField] Button noCharacterSlotsOkayButton;
    [SerializeField] GameObject deleteCharacterSlotPopUp;

    [Header("Character Creation Main Panel Button")]
    [SerializeField] Button characterNameButton;
    [SerializeField] Button characterClassButton;
    [SerializeField] Button characterHairButton;
    [SerializeField] Button characterHairColorButton;
    [SerializeField] Button characterGenderButton;
    [SerializeField] TextMeshProUGUI characterGenderText;
    [SerializeField] Button startGameButton;


    [Header("Character Creation Class Panel Button")]
    [SerializeField] Button[] characterClassButtons;
    [SerializeField] Button[] characterHairButtons;
    [SerializeField] Button[] characterHairColorButtons;

    [Header("Character Creation Secondery Panel Menu")]
    [SerializeField] GameObject characterClassMenu;
    [SerializeField] GameObject characterHairMenu;
    [SerializeField] GameObject characterHairColorMenu;
    [SerializeField] GameObject characterNameMenu;
    [SerializeField] TMP_InputField characterNameInputField;

    [Header("Color Sliders")]
    [SerializeField] Slider redSlider;
    [SerializeField] Slider greenSlider;
    [SerializeField] Slider blueSlider;

    [Header("Hidden Gears")]
    private HeadEquipmentItem hiddinHelmet;


    [Header("Character Slots")]
    public CharacterSlot currentSelectedSlot = CharacterSlot.NO_SLOT;

    [Header("Class")]
    public CharacterClass[] startingClasses;

    public void StartNetworkAsHost()
    {
        NetworkManager.Singleton.StartHost();
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void AttempToCreateNewCharacter()
    {
        if (WorldSaveGameManager.instance.HasFreeCharacterSlot())
        {
            OpenCharacterCreationMenu();
        }
        else
        {
            DisplayNoFreeCharacterSlotsPopUp();
        }
    }
   
    public void StartNewGame()
    {
        WorldSaveGameManager.instance.AttempToCreateNewGame();
    }

    public void ExitGame()
    {
      NetworkManager.Singleton.Shutdown(); // Dừng host/client/server nếu đang chạy

      Application.Quit(); // Thoát ứng dụng
    }

    public void OpenLoadGameMenu()
    {
        titleScreenMainMenu.SetActive(false);

        titleScreenLoadGameMenu.SetActive(true);

        loadMenuReturnButton.Select();
    }

    public void CloseLoadGameMenu()
    {
        titleScreenMainMenu.SetActive(true);

        titleScreenLoadGameMenu.SetActive(false);

        mainMenuLoadGameButton.Select();
    }

    public void ToggleBodyType()
    {
        PlayerManager player = NetworkManager.Singleton.LocalClient.PlayerObject.GetComponent<PlayerManager>();

        player.playerNetworkManager.isMale.Value = !player.playerNetworkManager.isMale.Value;

        if (player.playerNetworkManager.isMale.Value)
        {
            characterGenderText.text = "Male";
        }
        else
        {
            characterGenderText.text = "Female";
        }
    }

    public void OpenTitleScreenMainMenu()
    {
        titleScreenMainMenu.SetActive(true);
    }

    public void CloseTitleScreenMainMenu()
    {
        titleScreenMainMenu.SetActive(false);
    }

    public void OpenCharacterCreationMenu()
    {
        CloseTitleScreenMainMenu();
        titleScreenCharacterCreationMenu.SetActive(true);

        PlayerManager player = NetworkManager.Singleton.LocalClient.PlayerObject.GetComponent<PlayerManager>();
        player.playerBodyManager.ToggleBodyType(true);
    }

    public void CloseCharacterCreationMenu()
    {
        titleScreenCharacterCreationMenu.SetActive(false);

        OpenTitleScreenMainMenu();
    }

    public void OpenChooseCharacterClassSubMenu()
    {
        ToggleCharacterCreationScreenMainMenuButtons(false);

        characterClassMenu.SetActive(true);

        if (characterClassButtons.Length > 0)
        {
            characterClassButtons[0].Select();
            characterClassButtons[0].OnSelect(null);
        }
    }

    public void CloseChooseCharacterClassSubMenu()
    {
        ToggleCharacterCreationScreenMainMenuButtons(true);

        characterClassMenu.SetActive(false);

        characterClassButton.Select();
        characterClassButton.OnSelect(null);
    }

    public void OpenChooseCharacterHairStyleSubMenu()
    {
        PlayerManager player = NetworkManager.Singleton.LocalClient.PlayerObject.GetComponent<PlayerManager>();

        ToggleCharacterCreationScreenMainMenuButtons(false);

        characterHairMenu.SetActive(true);

        if (characterHairButtons.Length > 0)
        {
            characterHairButtons[0].Select();
            characterHairButtons[0].OnSelect(null);
        }

        if (player.playerInventoryManager.headEquipment != null)
            hiddinHelmet = Instantiate(player.playerInventoryManager.headEquipment);

        player.playerInventoryManager.headEquipment = null;
        player.playerEquipmentManager.EquipArmor();
    }

    public void CloseChooseCharacterHairStyleSubMenu()
    {
        PlayerManager player = NetworkManager.Singleton.LocalClient.PlayerObject.GetComponent<PlayerManager>();

        ToggleCharacterCreationScreenMainMenuButtons(true);

        characterHairMenu.SetActive(false);

        characterHairButton.Select();
        characterHairButton.OnSelect(null);

        if (hiddinHelmet != null)
            player.playerInventoryManager.headEquipment = hiddinHelmet;

        player.playerEquipmentManager.EquipArmor();
    }

    public void OpenChooseCharacterHairColorSubMenu()
    {
        PlayerManager player = NetworkManager.Singleton.LocalClient.PlayerObject.GetComponent<PlayerManager>();

        ToggleCharacterCreationScreenMainMenuButtons(false);

        characterHairColorMenu.SetActive(true);

        if (characterHairColorButtons.Length > 0)
        {
            characterHairColorButtons[0].Select();
            characterHairColorButtons[0].OnSelect(null);
        }

        if (player.playerInventoryManager.headEquipment != null)
            hiddinHelmet = Instantiate(player.playerInventoryManager.headEquipment);

        player.playerInventoryManager.headEquipment = null;
        player.playerEquipmentManager.EquipArmor();
    }

    public void CloseChooseCharacterHairColorSubMenu()
    {
        PlayerManager player = NetworkManager.Singleton.LocalClient.PlayerObject.GetComponent<PlayerManager>();

        ToggleCharacterCreationScreenMainMenuButtons(true);

        characterHairColorMenu.SetActive(false);

        characterHairColorButton.Select();
        characterHairColorButton.OnSelect(null);

        if (hiddinHelmet != null)
            player.playerInventoryManager.headEquipment = hiddinHelmet;

        player.playerEquipmentManager.EquipArmor();
    }

    public void OpenChooseCharacterNameSubMenu()
    {
        PlayerManager player = NetworkManager.Singleton.LocalClient.PlayerObject.GetComponent<PlayerManager>();

        ToggleCharacterCreationScreenMainMenuButtons(false);

        characterNameButton.gameObject.SetActive(false);
        characterNameMenu.SetActive(true);

        characterNameInputField.Select();
    }

    public void CloseChooseCharacterNameSubMenu()
    {
        PlayerManager player = NetworkManager.Singleton.LocalClient.PlayerObject.GetComponent<PlayerManager>();

        ToggleCharacterCreationScreenMainMenuButtons(true);

        characterNameMenu.SetActive(false);
        characterNameButton.gameObject.SetActive(true);

        characterNameButton.Select();

        player.playerNetworkManager.characterName.Value = characterNameInputField.text;
    }

    private void ToggleCharacterCreationScreenMainMenuButtons(bool status)
    {
        characterNameButton.enabled = status;
        characterClassButton.enabled = status;
        characterHairButton.enabled = status;
        characterHairColorButton.enabled = status;
        characterGenderButton.enabled = status;
        startGameButton.enabled = status;
    }

    public void DisplayNoFreeCharacterSlotsPopUp()
    {
        noCharacterSlotsPopUp.SetActive(true);
        noCharacterSlotsOkayButton.Select();
    }

    public void CloseNoFreeCharacterSlotsPopUp()
    {
        noCharacterSlotsPopUp.SetActive(false);
        mainMenuNewGameButton.Select();
    }

    public void SelectCharacterSlot(CharacterSlot characterSlot)
    {
        currentSelectedSlot = characterSlot;
    }

    public void SelectNoSlot()
    {
        currentSelectedSlot = CharacterSlot.NO_SLOT;
    }

    public void AttempToDeleteCharacterSlot()
    {
        if (currentSelectedSlot != CharacterSlot.NO_SLOT)
        {
            deleteCharacterSlotPopUp.SetActive(true);
            deletaCharacterPopUpConfirmButton.Select();
        }
    }

    public void DeleteCharacterSlot()
    {
        deleteCharacterSlotPopUp.SetActive(false);
        WorldSaveGameManager.instance.DeleteGame(currentSelectedSlot);
        titleScreenLoadGameMenu.SetActive(false);
        titleScreenLoadGameMenu.SetActive(true);
        loadMenuReturnButton.Select();
    }

    public void CloseDeleteCharactePopUp()
    {
        deleteCharacterSlotPopUp.SetActive(false);
        loadMenuReturnButton.Select();
    }

    public void SelectClass(int classID)
    {
        PlayerManager player = NetworkManager.Singleton.LocalClient.PlayerObject.GetComponent<PlayerManager>();

        if (startingClasses.Length <= 0)
            return;

        startingClasses[classID].SetClass(player);

        CloseChooseCharacterClassSubMenu();
    }

    public void PreviewClass(int classID)
    {
        PlayerManager player = NetworkManager.Singleton.LocalClient.PlayerObject.GetComponent<PlayerManager>();

        if (startingClasses.Length <= 0)
            return;

        startingClasses[classID].SetClass(player);
    }

    public void SetCharacterClass(PlayerManager player,
    int vitality,
    int endurance,
    int mind,
    int strength,
    int dexterity,
    int intelligence,
    int faith,
    WeaponItem[] mainHandWeapons,
    WeaponItem[] offHandWeapons,
    HeadEquipmentItem headEquipment,
    BodyEquipmentItem bodyEquipment,
    LegEquipmentItem legEquipment,
    HandEquipmentItem handEquipment,
    QuickSlotItem[] quickSlotItems)
    {
        hiddinHelmet = null;

        player.playerNetworkManager.vitality.Value = vitality;
        player.playerNetworkManager.endurance.Value = endurance;
        player.playerNetworkManager.mind.Value = mind;
        player.playerNetworkManager.strength.Value = strength;
        player.playerNetworkManager.dexterity.Value = dexterity;
        player.playerNetworkManager.intelligence.Value = intelligence;
        player.playerNetworkManager.faith.Value = faith;

        player.playerInventoryManager.weaponInRightHandSlot[0] = Instantiate(mainHandWeapons[0]);
        player.playerInventoryManager.weaponInRightHandSlot[1] = Instantiate(mainHandWeapons[1]);
        player.playerInventoryManager.weaponInRightHandSlot[2] = Instantiate(mainHandWeapons[2]);
        player.playerInventoryManager.currentRightHandWeapon = player.playerInventoryManager.weaponInRightHandSlot[0];
        player.playerNetworkManager.currentRightHandWeaponID.Value = player.playerInventoryManager.weaponInRightHandSlot[0].itemID;

        player.playerInventoryManager.weaponInLeftHandSlot[0] = Instantiate(offHandWeapons[0]);
        player.playerInventoryManager.weaponInLeftHandSlot[1] = Instantiate(offHandWeapons[1]);
        player.playerInventoryManager.weaponInLeftHandSlot[2] = Instantiate(offHandWeapons[2]);
        player.playerInventoryManager.currentLeftHandWeapon = player.playerInventoryManager.weaponInLeftHandSlot[0];
        player.playerNetworkManager.currentLeftHandWeaponID.Value = player.playerInventoryManager.weaponInLeftHandSlot[0].itemID;

        //EQUIPMENT
        if (headEquipment != null)
        {
            HeadEquipmentItem equipment = Instantiate(headEquipment);
            player.playerInventoryManager.headEquipment = equipment;
        }
        else
        {
            player.playerInventoryManager.headEquipment = null;
        }

        if (bodyEquipment != null)
        {
            BodyEquipmentItem equipment = Instantiate(bodyEquipment);
            player.playerInventoryManager.bodyEquipment = equipment;
        }
        else
        {
            player.playerInventoryManager.bodyEquipment = null;
        }

        if (legEquipment != null)
        {
            LegEquipmentItem equipment = Instantiate(legEquipment);
            player.playerInventoryManager.legEquipment = equipment;
        }
        else
        {
            player.playerInventoryManager.legEquipment = null;
        }

        if (handEquipment != null)
        {
            HandEquipmentItem equipment = Instantiate(handEquipment);
            player.playerInventoryManager.handEquipment = equipment;
        }
        else
        {
            player.playerInventoryManager.handEquipment = null;
        }

        player.playerEquipmentManager.EquipArmor();

        //quick slot

        player.playerInventoryManager.quickSlotItemIndex = 0;

        if (quickSlotItems[0] != null)
            player.playerInventoryManager.quickSlotItemsInQuickSlots[0] = Instantiate(quickSlotItems[0]);
        if (quickSlotItems[1] != null)
            player.playerInventoryManager.quickSlotItemsInQuickSlots[1] = Instantiate(quickSlotItems[1]);
        if (quickSlotItems[2] != null)
            player.playerInventoryManager.quickSlotItemsInQuickSlots[2] = Instantiate(quickSlotItems[2]);

        player.playerEquipmentManager.LoadQuickSlotItem(player.playerInventoryManager.quickSlotItemsInQuickSlots[player.playerInventoryManager.quickSlotItemIndex]);

    }

    //hair
    public void SelectHair(int hairID)
    {
        PlayerManager player = NetworkManager.Singleton.LocalClient.PlayerObject.GetComponent<PlayerManager>();

        player.playerNetworkManager.hairStyleID.Value = hairID;

        CloseChooseCharacterHairStyleSubMenu();
    }

    public void PreviewHair(int hairID)
    {
        PlayerManager player = NetworkManager.Singleton.LocalClient.PlayerObject.GetComponent<PlayerManager>();

        player.playerNetworkManager.hairStyleID.Value = hairID;
    }


    //hair color

    public void SelectHairColor()
    {
        PlayerManager player = NetworkManager.Singleton.LocalClient.PlayerObject.GetComponent<PlayerManager>();

        player.playerNetworkManager.hairColorRed.Value = redSlider.value;
        player.playerNetworkManager.hairColorGreen.Value = greenSlider.value;
        player.playerNetworkManager.hairColorBlue.Value = blueSlider.value;

        CloseChooseCharacterHairColorSubMenu();
    }

    public void PreviewHairColor()
    {
        PlayerManager player = NetworkManager.Singleton.LocalClient.PlayerObject.GetComponent<PlayerManager>();

        player.playerNetworkManager.hairColorRed.Value = redSlider.value;
        player.playerNetworkManager.hairColorGreen.Value = greenSlider.value;
        player.playerNetworkManager.hairColorBlue.Value = blueSlider.value;
    }

    public void SetRedColorSlider(float redValue)
    {
        redSlider.value = redValue;
    }

    public void SetGreenColorSlider(float greenValue)
    {
        greenSlider.value = greenValue;
    }

    public void SetBlueColorSlider(float blueValue)
    {
        blueSlider.value = blueValue;
    }

}
