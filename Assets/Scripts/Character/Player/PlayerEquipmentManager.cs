using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerEquipmentManager : CharacterEquipmentManager
{
    PlayerManager player;
    [Header("Weapon Model Instantiation Slots")]
    [HideInInspector] public WeaponModelInstantiationSlot rightHandWeaponSlot;
    [HideInInspector] public WeaponModelInstantiationSlot leftHandWeaponSlot;
    [HideInInspector] public WeaponModelInstantiationSlot leftHandShieldSlot;
    [HideInInspector] public WeaponModelInstantiationSlot backSlot;

    [Header("Weapon Models")]
    [HideInInspector] public GameObject rightHandWeaponModel;
    [HideInInspector] public GameObject leftHandWeaponModel;

    [Header("Weapon Managers")]
    public WeaponManager rightWeaponManager;
    public WeaponManager leftWeaponManager;

    [Header("General Equipment Models")]
    public GameObject hatsObject;
    [HideInInspector] public GameObject[] hats;
    public GameObject hoodsObject;
    [HideInInspector] public GameObject[] hoods;
    public GameObject faceCoversObject;
    [HideInInspector] public GameObject[] faceCovers;
    public GameObject helmetAccessoriesObject;
    [HideInInspector] public GameObject[] helmetAccessories;
    public GameObject backAccessoriesObject;
    [HideInInspector] public GameObject[] backAccessories;
    public GameObject hipAccessoriesObject;
    [HideInInspector] public GameObject[] hipAccessories;
    public GameObject rightShoulderObject;
    [HideInInspector] public GameObject[] rightShoulder;
    public GameObject rightElbowObject;
    [HideInInspector] public GameObject[] rightElbow;
    public GameObject rightKneeObject;
    [HideInInspector] public GameObject[] rightKnee;
    public GameObject leftShoulderObject;
    [HideInInspector] public GameObject[] leftShoulder;
    public GameObject leftElbowObject;
    [HideInInspector] public GameObject[] leftElbow;
    public GameObject leftKneeObject;
    [HideInInspector] public GameObject[] leftKnee;

    [Header("Male Equipment Models")]
    public GameObject maleFullHelmetObject;
    [HideInInspector] public GameObject[] maleHeadFullHelmets;
    public GameObject maleFullBodyObject;
    [HideInInspector] public GameObject[] maleBodies;
    public GameObject maleRightUpperArmObject;
    [HideInInspector] public GameObject[] maleRightUpperArms;
    public GameObject maleRightLowerArmObject;
    [HideInInspector] public GameObject[] maleRightLowerArms;
    public GameObject maleRightHandObject;
    [HideInInspector] public GameObject[] maleRightHands;
    public GameObject maleLeftUpperArmObject;
    [HideInInspector] public GameObject[] maleLeftUpperArms;
    public GameObject maleLeftLowerArmObject;
    [HideInInspector] public GameObject[] maleLeftLowerArms;
    public GameObject maleLeftHandObject;
    [HideInInspector] public GameObject[] maleLeftHands;
    public GameObject maleHipsObject;
    [HideInInspector] public GameObject[] maleHips;
    public GameObject maleRightLegObject;
    [HideInInspector] public GameObject[] maleRightLegs;
    public GameObject maleLeftLegObject;
    [HideInInspector] public GameObject[] maleLeftLegs;

    [Header("Female Equipment Models")]
    public GameObject femaleFullHelmetObject;
    [HideInInspector] public GameObject[] femaleHeadFullHelmets;
    public GameObject femaleFullBodyObject;
    [HideInInspector] public GameObject[] femaleBodies;
    public GameObject femaleRightUpperArmObject;
    [HideInInspector] public GameObject[] femaleRightUpperArms;
    public GameObject femaleRightLowerArmObject;
    [HideInInspector] public GameObject[] femaleRightLowerArms;
    public GameObject femaleRightHandObject;
    [HideInInspector] public GameObject[] femaleRightHands;
    public GameObject femaleLeftUpperArmObject;
    [HideInInspector] public GameObject[] femaleLeftUpperArms;
    public GameObject femaleLeftLowerArmObject;
    [HideInInspector] public GameObject[] femaleLeftLowerArms;
    public GameObject femaleLeftHandObject;
    [HideInInspector] public GameObject[] femaleLeftHands;
    public GameObject femaleHipsObject;
    [HideInInspector] public GameObject[] femaleHips;
    public GameObject femaleRightLegObject;
    [HideInInspector] public GameObject[] femaleRightLegs;
    public GameObject femaleLeftLegObject;
    [HideInInspector] public GameObject[] femaleLeftLegs;

    protected override void Awake()
    {
        base.Awake();

        player = GetComponent<PlayerManager>();

        InitializeWeaponSlots();
        InitializeArmorModels();
    }

    protected override void Start()
    {
        base.Start();
        LoadWeaponsOnBothHands();
    }

    public void EquipArmor()
    {
        LoadHeadEquipment(player.playerInventoryManager.headEquipment);
        LoadBodyEquipment(player.playerInventoryManager.bodyEquipment);
        LoadLegEquipment(player.playerInventoryManager.legEquipment);
        LoadHandEquipment(player.playerInventoryManager.handEquipment);
    }

    //Quick Slot
    public void SwitchQuickSlotItem()
    {
        if (!player.IsOwner)
            return;
        if (player.isDead.Value)
            return;

        QuickSlotItem selectedItem = null;

        player.playerInventoryManager.quickSlotItemIndex += 1;

        if (player.playerInventoryManager.quickSlotItemIndex < 0 || player.playerInventoryManager.quickSlotItemIndex > 2)
        {
            player.playerInventoryManager.quickSlotItemIndex = 0;

            float itemCount = 0;
            QuickSlotItem firstItem = null;
            int firstItemPosition = 0;

            for (int i = 0; i < player.playerInventoryManager.quickSlotItemsInQuickSlots.Length; i++)
            {
                if (player.playerInventoryManager.quickSlotItemsInQuickSlots[i] != null)
                {
                    itemCount += 1;

                    if (firstItem == null)
                    {
                        firstItem = player.playerInventoryManager.quickSlotItemsInQuickSlots[i];
                        firstItemPosition = i;
                    }
                }
            }

            if (itemCount <= 1)
            {
                player.playerInventoryManager.quickSlotItemIndex = -1;
                selectedItem = null;
                player.playerNetworkManager.currentQuickSlotItemID.Value = -1;
            }
            else
            {
                player.playerInventoryManager.quickSlotItemIndex = firstItemPosition;
                player.playerNetworkManager.currentQuickSlotItemID.Value = firstItem.itemID;
            }

            return;
        }

        if (player.playerInventoryManager.quickSlotItemsInQuickSlots[player.playerInventoryManager.quickSlotItemIndex] != null)
        {
            selectedItem = player.playerInventoryManager.quickSlotItemsInQuickSlots[player.playerInventoryManager.quickSlotItemIndex];

            player.playerNetworkManager.currentQuickSlotItemID.Value =
             player.playerInventoryManager.quickSlotItemsInQuickSlots[player.playerInventoryManager.quickSlotItemIndex].itemID;
        }
        else
        {
            player.playerNetworkManager.currentQuickSlotItemID.Value = -1;
        }

        if (selectedItem == null && player.playerInventoryManager.quickSlotItemIndex <= 2)
        {
            SwitchQuickSlotItem();
        }

    }

    public void LoadQuickSlotItem(QuickSlotItem equipment)
    {
        if(equipment == null)
        {
            if(player.IsOwner)
            player.playerNetworkManager.currentQuickSlotItemID.Value = -1;

            player.playerInventoryManager.currentQuickSlotItem = null;
            return;
        }

        player.playerInventoryManager.currentQuickSlotItem = equipment;

        if(player.IsOwner)
        player.playerNetworkManager.currentQuickSlotItemID.Value = equipment.itemID;
    }

    //Equipment

    private void InitializeArmorModels()
    {
        //hats
        List<GameObject> hatsList = new List<GameObject>();
        foreach (Transform child in hatsObject.transform)
        {
            hatsList.Add(child.gameObject);
        }

        //hoods
        List<GameObject> hoodsList = new List<GameObject>();
        foreach (Transform child in hoodsObject.transform)
        {
            hoodsList.Add(child.gameObject);
        }
        hoods = hoodsList.ToArray();

        //face covers
        List<GameObject> faceCoversList = new List<GameObject>();
        foreach (Transform child in faceCoversObject.transform)
        {
            faceCoversList.Add(child.gameObject);
        }
        faceCovers = faceCoversList.ToArray();

        //helmet accessories
        List<GameObject> helmetAccessoriesList = new List<GameObject>();
        foreach (Transform child in helmetAccessoriesObject.transform)
        {
            helmetAccessoriesList.Add(child.gameObject);
        }
        helmetAccessories = helmetAccessoriesList.ToArray();

        //back accessories
        List<GameObject> backAccessoriesList = new List<GameObject>();
        foreach (Transform child in backAccessoriesObject.transform)
        {
            backAccessoriesList.Add(child.gameObject);
        }
        backAccessories = backAccessoriesList.ToArray();

        //hip accessories
        List<GameObject> hipAccessoriesList = new List<GameObject>();
        foreach (Transform child in hipAccessoriesObject.transform)
        {
            hipAccessoriesList.Add(child.gameObject);
        }
        hipAccessories = hipAccessoriesList.ToArray();

        //right shoulder
        List<GameObject> rightShoulderList = new List<GameObject>();
        foreach (Transform child in rightShoulderObject.transform)
        {
            rightShoulderList.Add(child.gameObject);
        }
        rightShoulder = rightShoulderList.ToArray();

        //right elbow
        List<GameObject> rightElbowList = new List<GameObject>();
        foreach (Transform child in rightElbowObject.transform)
        {
            rightElbowList.Add(child.gameObject);
        }
        rightElbow = rightElbowList.ToArray();

        //right knee
        List<GameObject> rightKneeList = new List<GameObject>();
        foreach (Transform child in rightKneeObject.transform)
        {
            rightKneeList.Add(child.gameObject);
        }
        rightKnee = rightKneeList.ToArray();

        //left shoulder
        List<GameObject> leftShoulderList = new List<GameObject>();
        foreach (Transform child in leftShoulderObject.transform)
        {
            leftShoulderList.Add(child.gameObject);
        }
        leftShoulder = leftShoulderList.ToArray();

        //left elbow
        List<GameObject> leftElbowList = new List<GameObject>();
        foreach (Transform child in leftElbowObject.transform)
        {
            leftElbowList.Add(child.gameObject);
        }
        leftElbow = leftElbowList.ToArray();

        //left knee
        List<GameObject> leftKneeList = new List<GameObject>();
        foreach (Transform child in leftKneeObject.transform)
        {
            leftKneeList.Add(child.gameObject);
        }
        leftKnee = leftKneeList.ToArray();

        //male full helmet
        List<GameObject> maleFullHelmetList = new List<GameObject>();
        foreach (Transform child in maleFullHelmetObject.transform)
        {
            maleFullHelmetList.Add(child.gameObject);
        }
        maleHeadFullHelmets = maleFullHelmetList.ToArray();

        //male bodies
        List<GameObject> maleBodiesList = new List<GameObject>();
        foreach (Transform child in maleFullBodyObject.transform)
        {
            maleBodiesList.Add(child.gameObject);
        }
        maleBodies = maleBodiesList.ToArray();

        //male right upper arm
        List<GameObject> maleRightUpperArmsList = new List<GameObject>();
        foreach (Transform child in maleRightUpperArmObject.transform)
        {
            maleRightUpperArmsList.Add(child.gameObject);
        }
        maleRightUpperArms = maleRightUpperArmsList.ToArray();

        //male right lower arm
        List<GameObject> maleRightLowerArmsList = new List<GameObject>();
        foreach (Transform child in maleRightLowerArmObject.transform)
        {
            maleRightLowerArmsList.Add(child.gameObject);
        }
        maleRightLowerArms = maleRightLowerArmsList.ToArray();

        //male right hand
        List<GameObject> maleRightHandsList = new List<GameObject>();
        foreach (Transform child in maleRightHandObject.transform)
        {
            maleRightHandsList.Add(child.gameObject);
        }
        maleRightHands = maleRightHandsList.ToArray();

        //male left upper arm
        List<GameObject> maleLeftUpperArmsList = new List<GameObject>();
        foreach (Transform child in maleLeftUpperArmObject.transform)
        {
            maleLeftUpperArmsList.Add(child.gameObject);
        }
        maleLeftUpperArms = maleLeftUpperArmsList.ToArray();

        //male left lower arm
        List<GameObject> maleLeftLowerArmsList = new List<GameObject>();
        foreach (Transform child in maleLeftLowerArmObject.transform)
        {
            maleLeftLowerArmsList.Add(child.gameObject);
        }
        maleLeftLowerArms = maleLeftLowerArmsList.ToArray();

        //male left hand
        List<GameObject> maleLeftHandsList = new List<GameObject>();
        foreach (Transform child in maleLeftHandObject.transform)
        {
            maleLeftHandsList.Add(child.gameObject);
        }
        maleLeftHands = maleLeftHandsList.ToArray();

        //male hips
        List<GameObject> maleHipsList = new List<GameObject>();
        foreach (Transform child in maleHipsObject.transform)
        {
            maleHipsList.Add(child.gameObject);
        }
        maleHips = maleHipsList.ToArray();

        //male right leg
        List<GameObject> maleRightLegsList = new List<GameObject>();
        foreach (Transform child in maleRightLegObject.transform)
        {
            maleRightLegsList.Add(child.gameObject);
        }
        maleRightLegs = maleRightLegsList.ToArray();

        //male left leg
        List<GameObject> maleLeftLegsList = new List<GameObject>();
        foreach (Transform child in maleLeftLegObject.transform)
        {
            maleLeftLegsList.Add(child.gameObject);
        }
        maleLeftLegs = maleLeftLegsList.ToArray();


        //female full helmet
        List<GameObject> femaleFullHelmetList = new List<GameObject>();
        foreach (Transform child in femaleFullHelmetObject.transform)
        {
            femaleFullHelmetList.Add(child.gameObject);
        }
        femaleHeadFullHelmets = femaleFullHelmetList.ToArray();

        //female bodies
        List<GameObject> femaleBodiesList = new List<GameObject>();
        foreach (Transform child in femaleFullBodyObject.transform)
        {
            femaleBodiesList.Add(child.gameObject);
        }
        femaleBodies = femaleBodiesList.ToArray();

        //female right upper arm
        List<GameObject> femaleRightUpperArmsList = new List<GameObject>();
        foreach (Transform child in femaleRightUpperArmObject.transform)
        {
            femaleRightUpperArmsList.Add(child.gameObject);
        }
        femaleRightUpperArms = femaleRightUpperArmsList.ToArray();

        //female right lower arm
        List<GameObject> femaleRightLowerArmsList = new List<GameObject>();
        foreach (Transform child in femaleRightLowerArmObject.transform)
        {
            femaleRightLowerArmsList.Add(child.gameObject);
        }
        femaleRightLowerArms = femaleRightLowerArmsList.ToArray();

        //female right hand
        List<GameObject> femaleRightHandsList = new List<GameObject>();
        foreach (Transform child in femaleRightHandObject.transform)
        {
            femaleRightHandsList.Add(child.gameObject);
        }
        femaleRightHands = femaleRightHandsList.ToArray();

        //female left upper arm
        List<GameObject> femaleLeftUpperArmsList = new List<GameObject>();
        foreach (Transform child in femaleLeftUpperArmObject.transform)
        {
            femaleLeftUpperArmsList.Add(child.gameObject);
        }
        femaleLeftUpperArms = femaleLeftUpperArmsList.ToArray();

        //female left lower arm
        List<GameObject> femaleLeftLowerArmsList = new List<GameObject>();
        foreach (Transform child in femaleLeftLowerArmObject.transform)
        {
            femaleLeftLowerArmsList.Add(child.gameObject);
        }
        femaleLeftLowerArms = femaleLeftLowerArmsList.ToArray();

        //female left hand
        List<GameObject> femaleLeftHandsList = new List<GameObject>();
        foreach (Transform child in femaleLeftHandObject.transform)
        {
            femaleLeftHandsList.Add(child.gameObject);
        }
        femaleLeftHands = femaleLeftHandsList.ToArray();

        //female hips
        List<GameObject> femaleHipsList = new List<GameObject>();
        foreach (Transform child in femaleHipsObject.transform)
        {
            femaleHipsList.Add(child.gameObject);
        }
        femaleHips = femaleHipsList.ToArray();

        //female right leg
        List<GameObject> femaleRightLegsList = new List<GameObject>();
        foreach (Transform child in femaleRightLegObject.transform)
        {
            femaleRightLegsList.Add(child.gameObject);
        }
        femaleRightLegs = femaleRightLegsList.ToArray();

        //female left leg
        List<GameObject> femaleLeftLegsList = new List<GameObject>();
        foreach (Transform child in femaleLeftLegObject.transform)
        {
            femaleLeftLegsList.Add(child.gameObject);
        }
        femaleLeftLegs = femaleLeftLegsList.ToArray();
    }

    public void LoadHeadEquipment(HeadEquipmentItem equipment)
    {
        UnLoadHeadEquipmentModels();

        if (equipment == null)
        {
            if (player.IsOwner)
                player.playerNetworkManager.headEquipmentID.Value = -1;

            player.playerInventoryManager.headEquipment = null;
            return;
        }

        player.playerInventoryManager.headEquipment = equipment;

        switch (equipment.headEquipmentType)
        {
            case HeadEquipmentType.FullHelmet:
                player.playerBodyManager.DisableHead();
                player.playerBodyManager.DisableHair();
                break;
            case HeadEquipmentType.Hat:
                break;
            case HeadEquipmentType.Hood:
                player.playerBodyManager.DisableHair();
                break;
            case HeadEquipmentType.FaceCover:
                player.playerBodyManager.DisableFacialHair();
                break;
            default:
                break;
        }

        foreach (var model in equipment.equipmentModels)
        {
            model.LoadModel(player, player.playerNetworkManager.isMale.Value);
        }

        player.playerStatsManager.CalculateTotalArmorAbsorption();

        if (player.IsOwner)
            player.playerNetworkManager.headEquipmentID.Value = equipment.itemID;
    }

    private void UnLoadHeadEquipmentModels()
    {
        foreach (var model in maleHeadFullHelmets)
        {
            model.SetActive(false);
        }

        foreach (var model in femaleHeadFullHelmets)
        {
            model.SetActive(false);
        }

        foreach (var model in hats)
        {
            model.SetActive(false);
        }

        foreach (var model in faceCovers)
        {
            model.SetActive(false);
        }

        foreach (var model in hoods)
        {
            model.SetActive(false);
        }

        foreach (var model in helmetAccessories)
        {
            model.SetActive(false);
        }
        player.playerBodyManager.EnableHead();
        player.playerBodyManager.EnableHair();
    }

    public void LoadBodyEquipment(BodyEquipmentItem equipment)
    {
        UnLoadBodyEquipmentModels();

        if (equipment == null)
        {
            if (player.IsOwner)
                player.playerNetworkManager.bodyEquipmentID.Value = -1;

            player.playerInventoryManager.bodyEquipment = null;
            return;
        }

        player.playerInventoryManager.bodyEquipment = equipment;

        player.playerBodyManager.DisableBody();

        foreach (var model in equipment.equipmentModels)
        {
            model.LoadModel(player, player.playerNetworkManager.isMale.Value);
        }

        player.playerStatsManager.CalculateTotalArmorAbsorption();

        if (player.IsOwner)
            player.playerNetworkManager.bodyEquipmentID.Value = equipment.itemID;
    }

    private void UnLoadBodyEquipmentModels()
    {
        foreach (var model in rightShoulder)
        {
            model.SetActive(false);
        }
        foreach (var model in rightElbow)
        {
            model.SetActive(false);
        }

        foreach (var model in leftShoulder)
        {
            model.SetActive(false);
        }
        foreach (var model in leftElbow)
        {
            model.SetActive(false);
        }

        foreach (var model in backAccessories)
        {
            model.SetActive(false);
        }
        //male
        foreach (var model in maleBodies)
        {
            model.SetActive(false);
        }
        foreach (var model in maleRightUpperArms)
        {
            model.SetActive(false);
        }
        foreach (var model in maleLeftUpperArms)
        {
            model.SetActive(false);
        }
        //female
        foreach (var model in femaleBodies)
        {
            model.SetActive(false);
        }
        foreach (var model in femaleRightUpperArms)
        {
            model.SetActive(false);
        }
        foreach (var model in femaleLeftUpperArms)
        {
            model.SetActive(false);
        }

        player.playerBodyManager.EnableBody();
    }

    public void LoadLegEquipment(LegEquipmentItem equipment)
    {
        UnLoadLegEquipmentModels();

        if (equipment == null)
        {
            if (player.IsOwner)
                player.playerNetworkManager.legEquipmentID.Value = -1;

            player.playerInventoryManager.legEquipment = null;
            return;
        }

        player.playerInventoryManager.legEquipment = equipment;

        player.playerBodyManager.DisableLowerBody();

        foreach (var model in equipment.equipmentModels)
        {
            model.LoadModel(player, player.playerNetworkManager.isMale.Value);
        }

        player.playerStatsManager.CalculateTotalArmorAbsorption();

        if (player.IsOwner)
            player.playerNetworkManager.legEquipmentID.Value = equipment.itemID;
    }

    private void UnLoadLegEquipmentModels()
    {

        foreach (var model in maleHips)
        {
            model.SetActive(false);
        }
        foreach (var model in femaleHips)
        {
            model.SetActive(false);
        }

        foreach (var model in rightKnee)
        {
            model.SetActive(false);
        }
        foreach (var model in leftKnee)
        {
            model.SetActive(false);
        }

        foreach (var model in maleLeftLegs)
        {
            model.SetActive(false);
        }
        foreach (var model in maleRightLegs)
        {
            model.SetActive(false);
        }

        foreach (var model in femaleLeftLegs)
        {
            model.SetActive(false);
        }
        foreach (var model in femaleRightLegs)
        {
            model.SetActive(false);
        }

        player.playerBodyManager.EnableLowerBody();
    }

    public void LoadHandEquipment(HandEquipmentItem equipment)
    {
        UnLoadHandEquipmentModels();

        if (equipment == null)
        {
            if (player.IsOwner)
                player.playerNetworkManager.handEquipmentID.Value = -1;

            player.playerInventoryManager.handEquipment = null;
            return;
        }

        player.playerInventoryManager.handEquipment = equipment;

        player.playerBodyManager.DisableArms();

        foreach (var model in equipment.equipmentModels)
        {
            model.LoadModel(player, player.playerNetworkManager.isMale.Value);
        }

        player.playerStatsManager.CalculateTotalArmorAbsorption();

        if (player.IsOwner)
            player.playerNetworkManager.handEquipmentID.Value = equipment.itemID;
    }

    private void UnLoadHandEquipmentModels()
    {
        //male
        foreach (var model in maleLeftLowerArms)
        {
            model.SetActive(false);
        }
        foreach (var model in maleRightLowerArms)
        {
            model.SetActive(false);
        }
        foreach (var model in maleRightHands)
        {
            model.SetActive(false);
        }
        foreach (var model in maleLeftHands)
        {
            model.SetActive(false);
        }

        //female
        foreach (var model in femaleLeftLowerArms)
        {
            model.SetActive(false);
        }
        foreach (var model in femaleRightLowerArms)
        {
            model.SetActive(false);
        }
        foreach (var model in femaleRightHands)
        {
            model.SetActive(false);
        }
        foreach (var model in femaleLeftHands)
        {
            model.SetActive(false);
        }

        player.playerBodyManager.EnableArms();
    }

    private void InitializeWeaponSlots()
    {
        WeaponModelInstantiationSlot[] weaponSlots = GetComponentsInChildren<WeaponModelInstantiationSlot>();
        foreach (var weaponSlot in weaponSlots)
        {
            if (weaponSlot.weaponSlot == WeaponModelSlot.RightHand)
            {
                rightHandWeaponSlot = weaponSlot;
            }
            else if (weaponSlot.weaponSlot == WeaponModelSlot.LeftHandWeaponSlot)
            {
                leftHandWeaponSlot = weaponSlot;
            }
            else if (weaponSlot.weaponSlot == WeaponModelSlot.LeftHandShieldSlot)
            {
                leftHandShieldSlot = weaponSlot;
            }
            else if (weaponSlot.weaponSlot == WeaponModelSlot.BackSlot)
            {
                backSlot = weaponSlot;
            }
        }
    }

    public void LoadWeaponsOnBothHands()
    {
        LoadRightWeapon();
        LoadLeftWeapon();
    }

    public void SwitchRightWeapon()
    {
        if (!player.IsOwner)
            return;
        if (player.isDead.Value)
            return;

        player.playerNetworkManager.isTwoHandingWeapon.Value = false;

        player.playerAnimatorManager.PlayTargetActionAnimation("Swap_Right_Weapon_01", false, false, true, true);

        WeaponItem selectedWeapon = null;

        player.playerInventoryManager.rightHandWeaponIndex += 1;

        if (player.playerInventoryManager.rightHandWeaponIndex < 0 || player.playerInventoryManager.rightHandWeaponIndex > 2)
        {
            player.playerInventoryManager.rightHandWeaponIndex = 0;

            float weaponCount = 0;
            WeaponItem firstWeapon = null;
            int firstWeaponPosition = 0;

            for (int i = 0; i < player.playerInventoryManager.weaponInRightHandSlot.Length; i++)
            {
                if (player.playerInventoryManager.weaponInRightHandSlot[i].itemID != WorldItemDatabase.instance.unarmedWeapon.itemID)
                {
                    weaponCount += 1;
                    if (firstWeapon == null)
                    {
                        firstWeapon = player.playerInventoryManager.weaponInRightHandSlot[i];
                        firstWeaponPosition = i;
                    }
                }
            }

            if (weaponCount <= 1)
            {
                player.playerInventoryManager.rightHandWeaponIndex = -1;
                selectedWeapon = WorldItemDatabase.instance.unarmedWeapon;
                player.playerInventoryManager.currentRightHandWeapon = selectedWeapon;
                player.playerNetworkManager.currentRightHandWeaponID.Value = selectedWeapon.itemID;
            }
            else
            {
                player.playerInventoryManager.rightHandWeaponIndex = firstWeaponPosition;
                player.playerInventoryManager.currentRightHandWeapon = firstWeapon;
                player.playerNetworkManager.currentRightHandWeaponID.Value = firstWeapon.itemID;
            }
            return;
        }

        foreach (WeaponItem weapon in player.playerInventoryManager.weaponInRightHandSlot)
        {
            if (player.playerInventoryManager.weaponInRightHandSlot[player.playerInventoryManager.rightHandWeaponIndex].itemID != WorldItemDatabase.instance.unarmedWeapon.itemID)
            {
                selectedWeapon = player.playerInventoryManager.weaponInRightHandSlot[player.playerInventoryManager.rightHandWeaponIndex];
                player.playerInventoryManager.currentRightHandWeapon = selectedWeapon;

                player.playerNetworkManager.currentRightHandWeaponID.Value = selectedWeapon.itemID;
                return;
            }
        }

        if (selectedWeapon == null && player.playerInventoryManager.rightHandWeaponIndex <= 2)
        {
            SwitchRightWeapon();
        }
    }

    public void LoadRightWeapon()
    {
        if (player.playerInventoryManager.currentRightHandWeapon != null)
        {
            rightHandWeaponSlot.UnloadWeaponModel();
            rightHandWeaponModel = Instantiate(player.playerInventoryManager.currentRightHandWeapon.weaponModel);
            rightHandWeaponSlot.PlaceWeaponModelIntoSlot(rightHandWeaponModel);
            rightWeaponManager = rightHandWeaponModel.GetComponent<WeaponManager>();
            rightWeaponManager.SetWeaponDamage(player, player.playerInventoryManager.currentRightHandWeapon);
            player.playerAnimatorManager.UpdateAnimatorController(player.playerInventoryManager.currentRightHandWeapon.weaponAnimator);
        }
    }

    public void SwitchLeftWeapon()
    {
        if (!player.IsOwner)
            return;
        if (player.isDead.Value)
            return;

        player.playerNetworkManager.isTwoHandingWeapon.Value = false;

        player.playerAnimatorManager.PlayTargetActionAnimation("Swap_Left_Weapon_01", false, false, true, true);

        WeaponItem selectedWeapon = null;

        player.playerInventoryManager.leftHandWeaponIndex += 1;

        if (player.playerInventoryManager.leftHandWeaponIndex < 0 || player.playerInventoryManager.leftHandWeaponIndex > 2)
        {
            player.playerInventoryManager.leftHandWeaponIndex = 0;

            float weaponCount = 0;
            WeaponItem firstWeapon = null;
            int firstWeaponPosition = 0;

            for (int i = 0; i < player.playerInventoryManager.weaponInLeftHandSlot.Length; i++)
            {
                if (player.playerInventoryManager.weaponInLeftHandSlot[i].itemID != WorldItemDatabase.instance.unarmedWeapon.itemID)
                {
                    weaponCount += 1;
                    if (firstWeapon == null)
                    {
                        firstWeapon = player.playerInventoryManager.weaponInLeftHandSlot[i];
                        firstWeaponPosition = i;
                    }
                }
            }

            if (weaponCount <= 1)
            {
                player.playerInventoryManager.leftHandWeaponIndex = -1;
                selectedWeapon = WorldItemDatabase.instance.unarmedWeapon;
                player.playerInventoryManager.currentLeftHandWeapon = selectedWeapon;
                player.playerNetworkManager.currentLeftHandWeaponID.Value = selectedWeapon.itemID;
            }
            else
            {
                player.playerInventoryManager.leftHandWeaponIndex = firstWeaponPosition;
                player.playerInventoryManager.currentLeftHandWeapon = selectedWeapon;
                player.playerNetworkManager.currentLeftHandWeaponID.Value = firstWeapon.itemID;
            }
            return;
        }

        foreach (WeaponItem weapon in player.playerInventoryManager.weaponInLeftHandSlot)
        {
            if (player.playerInventoryManager.weaponInLeftHandSlot[player.playerInventoryManager.leftHandWeaponIndex].itemID != WorldItemDatabase.instance.unarmedWeapon.itemID)
            {
                selectedWeapon = player.playerInventoryManager.weaponInLeftHandSlot[player.playerInventoryManager.leftHandWeaponIndex];
                player.playerInventoryManager.currentLeftHandWeapon = selectedWeapon;

                player.playerNetworkManager.currentLeftHandWeaponID.Value = selectedWeapon.itemID;
                return;
            }
        }

        if (selectedWeapon == null && player.playerInventoryManager.leftHandWeaponIndex <= 2)
        {
            SwitchLeftWeapon();
        }

    }

    public void LoadLeftWeapon()
    {
        if (player.playerInventoryManager.currentLeftHandWeapon != null)
        {
            if (leftHandWeaponSlot.currentWeaponModel != null)
                leftHandWeaponSlot.UnloadWeaponModel();

            if (leftHandShieldSlot.currentWeaponModel != null)
                leftHandShieldSlot.UnloadWeaponModel();

            leftHandWeaponModel = Instantiate(player.playerInventoryManager.currentLeftHandWeapon.weaponModel);

            switch (player.playerInventoryManager.currentLeftHandWeapon.weaponModelType)
            {
                case WeaponModelType.Weapon:
                    leftHandWeaponSlot.PlaceWeaponModelIntoSlot(leftHandWeaponModel);
                    break;
                case WeaponModelType.Shield:
                    leftHandShieldSlot.PlaceWeaponModelIntoSlot(leftHandWeaponModel);
                    break;
            }

            leftWeaponManager = leftHandWeaponModel.GetComponent<WeaponManager>();
            leftWeaponManager.SetWeaponDamage(player, player.playerInventoryManager.currentLeftHandWeapon);
        }
    }

    public void UnTwoHandWeapon()
    {
        player.playerAnimatorManager.UpdateAnimatorController(player.playerInventoryManager.currentRightHandWeapon.weaponAnimator);

        if (player.playerInventoryManager.currentLeftHandWeapon.weaponModelType == WeaponModelType.Weapon)
        {
            leftHandWeaponSlot.PlaceWeaponModelIntoSlot(leftHandWeaponModel);
        }
        else if (player.playerInventoryManager.currentLeftHandWeapon.weaponModelType == WeaponModelType.Shield)
        {
            leftHandShieldSlot.PlaceWeaponModelIntoSlot(leftHandWeaponModel);
        }

        rightHandWeaponSlot.PlaceWeaponModelIntoSlot(rightHandWeaponModel);

        rightWeaponManager.SetWeaponDamage(player, player.playerInventoryManager.currentRightHandWeapon);
        leftWeaponManager.SetWeaponDamage(player, player.playerInventoryManager.currentLeftHandWeapon);
    }

    public void TwoHandRightWeapon()
    {
        if (player.playerInventoryManager.currentRightHandWeapon == WorldItemDatabase.instance.unarmedWeapon)
        {
            if (player.IsOwner)
            {
                player.playerNetworkManager.isTwoHandingRightWeapon.Value = false;
                player.playerNetworkManager.isTwoHandingWeapon.Value = false;
            }
            return;
        }
        player.playerAnimatorManager.UpdateAnimatorController(player.playerInventoryManager.currentRightHandWeapon.weaponAnimator);

        backSlot.PlaceWeaponModelInUnequipSlot(leftHandWeaponModel, player.playerInventoryManager.currentLeftHandWeapon.weaponClass, player);

        rightHandWeaponSlot.PlaceWeaponModelIntoSlot(rightHandWeaponModel);

        rightWeaponManager.SetWeaponDamage(player, player.playerInventoryManager.currentRightHandWeapon);
        leftWeaponManager.SetWeaponDamage(player, player.playerInventoryManager.currentLeftHandWeapon);
    }

    public void TwoHandLeftWeapon()
    {
        if (player.playerInventoryManager.currentLeftHandWeapon == WorldItemDatabase.instance.unarmedWeapon)
        {
            if (player.IsOwner)
            {
                player.playerNetworkManager.isTwoHandingLeftWeapon.Value = false;
                player.playerNetworkManager.isTwoHandingWeapon.Value = false;
            }
            return;
        }
        player.playerAnimatorManager.UpdateAnimatorController(player.playerInventoryManager.currentLeftHandWeapon.weaponAnimator);

        backSlot.PlaceWeaponModelInUnequipSlot(rightHandWeaponModel, player.playerInventoryManager.currentRightHandWeapon.weaponClass, player);

        rightHandWeaponSlot.PlaceWeaponModelIntoSlot(leftHandWeaponModel);

        rightWeaponManager.SetWeaponDamage(player, player.playerInventoryManager.currentRightHandWeapon);
        leftWeaponManager.SetWeaponDamage(player, player.playerInventoryManager.currentLeftHandWeapon);
    }

    public void OpenDamageCollider()
    {
        if (player.playerNetworkManager.isUsingRightHand.Value)
        {
            rightWeaponManager.meleeDamageCollider.EnableDamageCollider();
            player.characterSoundFXManager.PlaySoundFX(
            WorldSoundFXManager.instance.ChooseRandomSFXFromArray(player.playerInventoryManager.currentRightHandWeapon.whooshes));
        }
        else if (player.playerNetworkManager.isUsingLeftHand.Value)
        {
            leftWeaponManager.meleeDamageCollider.EnableDamageCollider();
            player.characterSoundFXManager.PlaySoundFX(
            WorldSoundFXManager.instance.ChooseRandomSFXFromArray(player.playerInventoryManager.currentLeftHandWeapon.whooshes));
        }

        //play whoosh sfx
    }

    public void CloseDamageCollider()
    {
        if (player.playerNetworkManager.isUsingRightHand.Value)
        {
            rightWeaponManager.meleeDamageCollider.DisableDamageCollider();
        }
        else if (player.playerNetworkManager.isUsingLeftHand.Value)
        {
            leftWeaponManager.meleeDamageCollider.DisableDamageCollider();
        }
    }

    public void UnHideWeapons()
    {
        if (player.playerEquipmentManager.rightHandWeaponModel != null)
            player.playerEquipmentManager.rightHandWeaponModel.SetActive(true);

        if (player.playerEquipmentManager.leftHandWeaponModel != null)
            player.playerEquipmentManager.leftHandWeaponModel.SetActive(true);
    }

}
