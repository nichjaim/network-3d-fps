using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System.Linq;

public class ItemPickupController : NetworkBehaviour
{
    #region Class Variables

    [Header("Pickup Properties")]

    [SerializeField]
    private ItemPickupType pickupType = ItemPickupType.None;

    // the number of items in pickup
    [SerializeField]
    private int pickupQuantity = 1;

    // will be picked up on contact
    [SerializeField]
    private bool isAutomaticPickup = false;
    public bool IsAutomaticPickup
    {
        get { return isAutomaticPickup; }
    }

    [Header("Ammo Properties")]

    // the weapons that the ammo pickup correspond to
    [SerializeField]
    private WeaponTypeSetTemplate ammoType = null;
    // randomizes the wep set template that the ammo pickup is based on
    [SerializeField]
    private bool randomizeAmmoType = false;

    [Header("Weapon Proeprties")]

    [SerializeField]
    private WeaponDataTemplate weaponPickupTemplate = null;
    private WeaponData _weaponPickup = null;

    [SerializeField]
    private WeaponPreviewController weaponPreview = null;

    #endregion




    #region MonoBehaviour Functions

    private void Awake()
    {
        // setup the properties related to the pickup weapon
        InitializePickupWeapon();
        // setup the ammo type. 
        InitializeAmmoType();
    }

    /*private void OnTriggerEnter(Collider other)
    {
        // if colliding obj is NOT a player
        if (!other.CompareTag("Player"))
        {
            // DONT continue code
            return;
        }

        // if supposed to automatically be picked up
        if (isAutomaticPickup)
        {
            Pickup();
        }
        *//*// else needs to be manually picked up
        else
        {
            if (GeneralMethods.IsNetworkConnectedButNotLocalClient(
                other.GetComponent<NetworkIdentity>()))
            {

            }
        }*//*
    }*/

    #endregion




    #region Initialization Functions

    /// <summary>
    /// Sets up the properties related to the pickup weapon. 
    /// Call in Awake().
    /// </summary>
    private void InitializePickupWeapon()
    {
        // if pickup is NOT a weapon
        if (pickupType != ItemPickupType.Weapon)
        {
            // DONT continue code
            return;
        }

        // if wep template IS set
        if (weaponPickupTemplate != null)
        {
            _weaponPickup = new WeaponData(weaponPickupTemplate);
            weaponPreview.SetupPreview(_weaponPickup);
        }
        // else wep template is NOT set
        else
        {
            // print warning to console
            Debug.LogWarning("weapon pickup does NOT have a template!");
        }
    }

    /// <summary>
    /// Sets up the ammo type. 
    /// Call in Awake().
    /// </summary>
    private void InitializeAmmoType()
    {
        // if should randomize the ammo type
        if (randomizeAmmoType)
        {
            // get all weapon type set templates
            WeaponTypeSetTemplate[] wepTypes = AssetRefMethods.LoadAllBundleAssetWeaponTypeSetTemplate();
            // get all weapoon types that need ammo to be used
            List<WeaponTypeSetTemplate> ammoBasedWepTypes = wepTypes.Where
                (iterSet => iterSet.template.doesNeedAmmo).ToList();

            // get random index of ammo wep type list
            int randomIndex = Random.Range(0, ammoBasedWepTypes.Count);
            // set ammo type to random list entry
            ammoType = ammoBasedWepTypes[randomIndex];
        }

        // MAYBE TODO: change pickup model based on ammo type
    }

    #endregion




    #region Item Functions

    /// <summary>
    /// Turns on/off the weapon preview.
    /// </summary>
    /// <param name="activateArg"></param>
    public void SetWeaponPreviewActivation(bool activateArg)
    {
        // if weapon preview set
        if (weaponPreview != null)
        {
            // turn on/off based on given bool
            weaponPreview.gameObject.SetActive(activateArg);
        }
    }

    #endregion




    #region Pickup Functions

    public void Pickup(CharacterMasterController pickerArg)
    {
        if (NetworkClient.isConnected)
        {
            CmdPickup();
        }
        else
        {
            PickupMain();
        }

        // unspawn object
        GetComponent<PooledObjectController>().UnspawnObject();
    }

    [Command(ignoreAuthority = true)]
    private void CmdPickup()
    {
        PickupMain();
        RpcPickup();
    }

    [ClientRpc]
    private void RpcPickup()
    {
        PickupMain();
    }

    private void PickupMain()
    {
        //TODO: trigger event to denote pickup
        Debug.Log("Item picked up!"); // TEST LINE
    }

    #endregion


}
