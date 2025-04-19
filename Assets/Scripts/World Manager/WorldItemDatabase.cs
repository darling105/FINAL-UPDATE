using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class WorldItemDatabase : MonoBehaviour
{
    public static WorldItemDatabase instance;

    public WeaponItem unarmedWeapon;

    public GameObject pickUpItemPrefab;

    [Header("Weapons")]
    [SerializeField] List<WeaponItem> weapons = new List<WeaponItem>();

    [Header("Armors")]
    [SerializeField] List<HeadEquipmentItem> headEquipment = new List<HeadEquipmentItem>();
    [SerializeField] List<BodyEquipmentItem> bodyEquipment = new List<BodyEquipmentItem>();
    [SerializeField] List<LegEquipmentItem> legEquipment = new List<LegEquipmentItem>();
    [SerializeField] List<HandEquipmentItem> handEquipment = new List<HandEquipmentItem>();

    [Header("Ashes Of War")]
    [SerializeField] List<AshOfWar> ashesOfWar = new List<AshOfWar>();

    [Header("Quick Slot")]
    [SerializeField] List<QuickSlotItem> quickSlotItems = new List<QuickSlotItem>();

    [Header("Items")]
    private List<Item> items = new List<Item>();

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }

        //Weapons
        foreach (var weapon in weapons)
        {
            items.Add(weapon);
        }

        //Armors
        foreach (var item in headEquipment)
        {
            items.Add(item);
        }
        foreach (var item in bodyEquipment)
        {
            items.Add(item);
        }
        foreach (var item in legEquipment)
        {
            items.Add(item);
        }
        foreach (var item in handEquipment)
        {
            items.Add(item);
        }

        //Ashes Of War
        foreach (var item in ashesOfWar)
        {
            items.Add(item);
        }

        //Quick Slot Items
        foreach (var item in quickSlotItems)
        {
            items.Add(item);
        }

        //Items
        for (int i = 0; i < items.Count; i++)
        {
            items[i].itemID = i;
        }
    }

    public WeaponItem GetWeaponByID(int ID)
    {
        return weapons.FirstOrDefault(weapon => weapon.itemID == ID);
    }

    public HeadEquipmentItem GetHeadEquipmentByID(int ID)
    {
        return headEquipment.FirstOrDefault(equipment => equipment.itemID == ID);
    }
    public BodyEquipmentItem GetBodyEquipmentByID(int ID)
    {
        return bodyEquipment.FirstOrDefault(equipment => equipment.itemID == ID);
    }
    public LegEquipmentItem GetLegEquipmentByID(int ID)
    {
        return legEquipment.FirstOrDefault(equipment => equipment.itemID == ID);
    }
    public HandEquipmentItem GetHandEquipmentByID(int ID)
    {
        return handEquipment.FirstOrDefault(equipment => equipment.itemID == ID);
    }

    public AshOfWar GetAshOfWarByID(int ID)
    {
        return ashesOfWar.FirstOrDefault(item => item.itemID == ID);
    }

    public Item GetItemByID(int ID)
    {
        return items.FirstOrDefault(item => item.itemID == ID);
    }

    public QuickSlotItem GetQuickSlotItemByID(int ID)
    {
        return quickSlotItems.FirstOrDefault(item => item.itemID == ID);
    }

    //item serialization

    public WeaponItem GetWeaponFromSerializedData(SerializableWeapons serializableWeapon)
    {
        WeaponItem weapon = null;
        if (GetWeaponByID(serializableWeapon.itemID))
        {
            weapon = Instantiate(GetWeaponByID(serializableWeapon.itemID));
        }

        if (weapon == null)
            return Instantiate(unarmedWeapon);


        if (GetAshOfWarByID(serializableWeapon.ashOfWarID))
        {
            AshOfWar ashOfWar = Instantiate(GetAshOfWarByID(serializableWeapon.ashOfWarID));
            weapon.ashOfWarAction = ashOfWar;
        }

        return weapon;

    }

    public FlaskItem GetFlaskFromSerializedData(SerializableFlask serializableFlask)
    {
        FlaskItem flask = null;
        if (GetQuickSlotItemByID(serializableFlask.itemID))
            flask = Instantiate(GetQuickSlotItemByID(serializableFlask.itemID)) as FlaskItem;

        return flask;
    }

    public QuickSlotItem GetQuickSlotItemFromSerializedData(SerializableQuickSlotItem serializableQuickSlotItem)
    {
        QuickSlotItem quickSlotItem = null;

        if (GetQuickSlotItemByID(serializableQuickSlotItem.itemID))
        {
            quickSlotItem = Instantiate(GetQuickSlotItemByID(serializableQuickSlotItem.itemID));
            quickSlotItem.itemAmount = serializableQuickSlotItem.itemAmount;
        }

        return quickSlotItem;
    }
}
