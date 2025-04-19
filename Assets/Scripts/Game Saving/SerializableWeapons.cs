using UnityEngine;

[System.Serializable]
public class SerializableWeapons : ISerializationCallbackReceiver
{
    [SerializeField] public int itemID;
    [SerializeField] public int ashOfWarID;

    public WeaponItem GetWeapon()
    {
        WeaponItem weapon = WorldItemDatabase.instance.GetWeaponFromSerializedData(this);
        return weapon;
        
    }

    public void OnAfterDeserialize()
    {

    }

    public void OnBeforeSerialize()
    {

    }
}
