using UnityEngine;

public class PlayerEquipmentManager : CharacterEquipmentManager
{
    PlayerManager player;

    public WeaponModelInstatiationSlot rightHandSlot;
    public WeaponModelInstatiationSlot leftHandSlot;

    [SerializeField] WeaponManager rightWeaponManager;
    [SerializeField] WeaponManager leftWeaponManager;

    public GameObject rightHandWeaponModel;
    public GameObject leftHandWeaponModel;
    protected override void Awake()
    {
        base.Awake();
        player = GetComponent<PlayerManager>();
        InitializeWeaponSlots();
    }
    protected override void Start()
    {
        base.Start();
        LoadWeaponsOnBothHands();
    }
    private void InitializeWeaponSlots()
    {
        WeaponModelInstatiationSlot[] weaponSlots = GetComponentsInChildren<WeaponModelInstatiationSlot>();
        foreach (var weaponSlot in weaponSlots)
        {
            if (weaponSlot.weaponSlot == WeaponModelSlot.RightHand)
            {
                rightHandSlot = weaponSlot;
            }
            else if (weaponSlot.weaponSlot == WeaponModelSlot.LeftHand)
            {
                leftHandSlot = weaponSlot;
            }
        }
    }
    public void LoadWeaponsOnBothHands()
    {
        LoadRightWeapon();
        LoadLeftWeapon();
    }
    public void LoadRightWeapon()
    {
        WeaponItem weaponItem = player.playerInventoryManager.currentRightHandWeapon;
        if (weaponItem != null)
        {
            rightHandSlot.UnloadWeapon();

            rightHandWeaponModel = Instantiate(weaponItem.weaponModel);
            rightHandSlot.LoadWeapon(rightHandWeaponModel);
            rightWeaponManager = rightHandWeaponModel.GetComponent<WeaponManager>();

            rightWeaponManager.SetWeaponDamage(player, weaponItem);
        }
    }
    public void SwitchRightWeapon()
    {
        if (!player.IsOwner)
            return;

        player.playerAnimationManager.PlayActionAnimation("Swap_Right_Weapon_01", false);

        player.playerInventoryManager.rightHandWeaponIndex++;

        if (player.playerInventoryManager.rightHandWeaponIndex < 0 || player.playerInventoryManager.rightHandWeaponIndex > 2)
        {
            player.playerInventoryManager.rightHandWeaponIndex = 0;

            // Find first weapon that's not unarmed
            int firstWeaponPosition = -1;
            for (int i = 0; i < player.playerInventoryManager.weaponInRightHandSlots.Length; i++)
            {
                WeaponItem weapon = player.playerInventoryManager.weaponInRightHandSlots[i];
                if (weapon != null && weapon.itemID != WorldItemDatabase.instance.unarmedWeapon.itemID)
                {
                    firstWeaponPosition = i;
                    break;
                }
            }

            if (firstWeaponPosition == -1)
            {
                // Equip unarmed
                player.playerInventoryManager.rightHandWeaponIndex = -1;
                player.playerNetworkManager.EquipWeaponServerRpc(WorldItemDatabase.instance.unarmedWeapon.itemID);
            }
            else
            {
                // Equip weapon and send id to network
                player.playerInventoryManager.rightHandWeaponIndex = firstWeaponPosition;
                WeaponItem selectedWeapon = player.playerInventoryManager.weaponInRightHandSlots[firstWeaponPosition];
                player.playerNetworkManager.EquipWeaponServerRpc(selectedWeapon.itemID);
            }
            return;
        }

        // If null or unarmed, recursion
        if (player.playerInventoryManager.rightHandWeaponIndex <= 2)
        {
            WeaponItem currentWeapon = player.playerInventoryManager.weaponInRightHandSlots[player.playerInventoryManager.rightHandWeaponIndex];
            if (currentWeapon == null || currentWeapon.itemID == WorldItemDatabase.instance.unarmedWeapon.itemID)
            {
                SwitchRightWeapon();
            }
            else
            {
                // Set the NetworkVariable to trigger the change
                player.playerNetworkManager.currentRightHandWeaponID.Value = currentWeapon.itemID;
            }
        }
    }
    public void LoadLeftWeapon()
    {
        if (player.playerInventoryManager.currentLeftHandWeapon != null)
        {
            leftHandSlot.UnloadWeapon();
            leftHandWeaponModel = Instantiate(player.playerInventoryManager.currentLeftHandWeapon.weaponModel);
            leftHandSlot.LoadWeapon(leftHandWeaponModel);
            leftWeaponManager = leftHandWeaponModel.GetComponent<WeaponManager>();
            leftWeaponManager.SetWeaponDamage(player, player.playerInventoryManager.currentLeftHandWeapon);
        }
    }
    public void SwitchLeftWeapon()
    {
        if (!player.IsOwner)
            return;

        player.playerAnimationManager.PlayActionAnimation("Swap_Left_Weapon_01", false);

        player.playerInventoryManager.leftHandWeaponIndex++;

        if (player.playerInventoryManager.leftHandWeaponIndex < 0 || player.playerInventoryManager.leftHandWeaponIndex > 2)
        {
            player.playerInventoryManager.leftHandWeaponIndex = 0;

            // Find first weapon that's not unarmed
            int firstWeaponPosition = -1;
            for (int i = 0; i < player.playerInventoryManager.weaponInLeftHandSlots.Length; i++)
            {
                WeaponItem weapon = player.playerInventoryManager.weaponInLeftHandSlots[i];
                if (weapon != null && weapon.itemID != WorldItemDatabase.instance.unarmedWeapon.itemID)
                {
                    firstWeaponPosition = i;
                    break;
                }
            }

            if (firstWeaponPosition == -1)
            {
                // Equip unarmed
                player.playerInventoryManager.leftHandWeaponIndex = -1;
                player.playerNetworkManager.currentLeftHandWeaponID.Value = WorldItemDatabase.instance.unarmedWeapon.itemID;
            }
            else
            {
                // Equip weapon and send id to network
                player.playerInventoryManager.leftHandWeaponIndex = firstWeaponPosition;
                WeaponItem selectedWeapon = player.playerInventoryManager.weaponInLeftHandSlots[firstWeaponPosition];
                player.playerNetworkManager.currentLeftHandWeaponID.Value = selectedWeapon.itemID;
            }
            return;
        }

        // If null or unarmed, recursion
        if (player.playerInventoryManager.leftHandWeaponIndex <= 2)
        {
            SwitchLeftWeapon();
        }
    }
    public void OpenDamageCollider()
    {
        if (player.playerNetworkManager.isUsingRightHand.Value)
        {
            rightWeaponManager.meleeDamageCollider.EnableDamageCollider();
        }
        else if (player.playerNetworkManager.isUsingLeftHand.Value)
        {
            leftWeaponManager.meleeDamageCollider.EnableDamageCollider();
        }
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

}
