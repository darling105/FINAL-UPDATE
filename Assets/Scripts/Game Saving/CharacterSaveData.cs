using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CharacterSaveData
{
    [Header("Scene Index")]
    public int sceneIndex = 1;

    [Header("Character Name")]
    public string characterName = "Character";

    [Header("Body Type")]
    public bool isMale = true;
    public int hairStyleID;
    public float hairColorRed;
    public float hairColorGreen;
    public float hairColorBlue;

    [Header("Time Played")]
    public float secondsPlayed;

    [Header("World Coordinates")]
    public float xPosition;
    public float yPosition;
    public float zPosition;

    [Header("Resources")]
    public int currentHealth;
    public float currentStamina;
    public int currentFocusPoints;

    [Header("Stats")]
    public int vitality;
    public int endurance;
    public int mind;

    [Header("Energy Site")]
    public SerializableDictionary<int, bool> energySites;

    [Header("Bosses")]
    public SerializableDictionary<int, bool> bossAwakened;
    public SerializableDictionary<int, bool> bossDefeated;

    [Header("World Item")]
    public SerializableDictionary<int, bool> worldItemsLooted;

    [Header("Equipment")]
    public int headEquipment;
    public int bodyEquipment;
    public int legEquipment;
    public int handEquipment;

    public int rightWeaponIndex;
    public SerializableWeapons rightWeapon01;
    public SerializableWeapons rightWeapon02;
    public SerializableWeapons rightWeapon03;

    public int leftWeaponIndex;
    public SerializableWeapons leftWeapon01;
    public SerializableWeapons leftWeapon02;
    public SerializableWeapons leftWeapon03;

    public int quickSlotIndex;
    public SerializableQuickSlotItem quickSlotItem01;
    public SerializableQuickSlotItem quickSlotItem02;
    public SerializableQuickSlotItem quickSlotItem03;

    public int currentHealthFlasksRemaining = 3;
    public int currentFocusPointsRemaining = 1;

    //inventory
    [Header("Inventory")]
    public List<SerializableWeapons> weaponsInventory;
    public List<SerializableQuickSlotItem> quickSlotItemsInventory;
    public List<int> headEquipmentInInventory;
    public List<int> bodyEquipmentInInventory;
    public List<int> legEquipmentInInventory;
    public List<int> handEquipmentInInventory;

    public CharacterSaveData()
    {
        energySites = new SerializableDictionary<int, bool>();
        bossAwakened = new SerializableDictionary<int, bool>();
        bossDefeated = new SerializableDictionary<int, bool>();
        worldItemsLooted = new SerializableDictionary<int, bool>();

        weaponsInventory = new List<SerializableWeapons>();
        quickSlotItemsInventory = new List<SerializableQuickSlotItem>();
        headEquipmentInInventory = new List<int>();
        bodyEquipmentInInventory = new List<int>();
        legEquipmentInInventory = new List<int>();
        handEquipmentInInventory = new List<int>();
    }
}
