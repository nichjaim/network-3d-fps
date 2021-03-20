using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickupInteractableController : InteractableController
{
    #region Class Variables

    [SerializeField]
    private ItemPickupController _itemPickup = null;

    #endregion




    #region Override Functions

    public override void OnInteractionTargetingStart(CharacterMasterController interactorArg)
    {
        base.OnInteractionTargetingStart(interactorArg);

        // turn ON weapon preview if it's setup
        _itemPickup.SetWeaponPreviewActivation(true);
    }

    public override void OnInteractionTargetingEnd(CharacterMasterController interactorArg)
    {
        base.OnInteractionTargetingEnd(interactorArg);

        // turn OFF weapon preview if it's setup
        _itemPickup.SetWeaponPreviewActivation(false);
    }

    public override void Interact(CharacterMasterController interactorArg)
    {
        base.Interact(interactorArg);

        _itemPickup.Pickup(interactorArg);
    }

    public override void OnLocalPlayerEnterProximity(CharacterMasterController interactorArg)
    {
        base.OnLocalPlayerEnterProximity(interactorArg);

        if (_itemPickup.IsAutomaticPickup)
        {
            _itemPickup.Pickup(interactorArg);
        }
    }

    #endregion


}
