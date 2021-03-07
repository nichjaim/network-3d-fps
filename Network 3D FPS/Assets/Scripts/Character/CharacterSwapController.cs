using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class CharacterSwapController : MonoBehaviour
{
    #region Class Variables

    [Header("Component References")]

    [SerializeField]
    private PlayerCharacterMasterController _playerCharacterMasterController = null;

    /// the character that the char is underneath all the char swapping. Uses this 
    /// char's inventory through all swapping
    private CharacterData primaryCharacter = null;

    private List<CharacterData> swappableCharacters = new List<CharacterData>();

    #endregion




    #region MonoBehaviour Functions

    private void OnEnable()
    {
        // starts listening for all relevant events
        StartAllEventListening();

        // setup the swap characters based on current game session conditions
        RefreshSwapCharacters();
    }

    private void OnDisable()
    {
        // stops listening for all relevant events
        StopAllEventListening();
    }

    #endregion




    #region Swap Functions

    /// <summary>
    /// Sets up the swap characters based on current game session conditions.
    /// </summary>
    private void RefreshSwapCharacters()
    {
        // if playing solo
        if (((NetworkManagerCustom)NetworkManager.singleton).numPlayers <= 1)
        {
            // get entire party
            List<CharacterData> partyChars = GameManager.Instance.GetPartyCharacters();

            // if party has at least one member
            if (partyChars.Count >= 1)
            {
                // set primary char to first party member (the MC)
                primaryCharacter = partyChars[0];
            }
            // else party is empty
            else
            {
                // set primary char to a null char
                primaryCharacter = null;
            }

            // set swapping chars from entire party
            SetupSwappableCharacterFromCharList(partyChars);
        }
        // else playing with multiple people
        else
        {
            // set primary char to the party char associated with this char's player number
            primaryCharacter = GameManager.Instance.GetPartyCharacterInPartySlot(
                _playerCharacterMasterController.GetPlayerNumber());
            // remove all swap chars from list as will only use the default char in multiplayer
            swappableCharacters = new List<CharacterData>();
        }

        // sets the main and secondary character data based on changes made
        RefreshCharData();
        RefreshCharDataSecondary();
    }

    /// <summary>
    /// Sets up swappable character list based on given character list.
    /// </summary>
    /// <param name="charListArg"></param>
    private void SetupSwappableCharacterFromCharList(List<CharacterData> charListArg)
    {
        // reset list of swapping characters to fresh empty list
        swappableCharacters = new List<CharacterData>();

        /// get the character's primary data (which is the data used for inventory, not the one for 
        /// models/heigth/stats/etc.)
        CharacterData characterData = _playerCharacterMasterController.GetCharData();
        // if no char data found
        if (characterData == null)
        {
            // DONT continue code
            return;
        }

        // get inventory of char's primary data
        CharacterInventory charInv = characterData.characterInventory;

        // initialize var for upcoming loop
        WeaponSlotData iterWepSlot;
        // loop through all potential swapping characters
        for (int i = 0; i < charListArg.Count; i++)
        {
            // get the iterating weapon slot
            iterWepSlot = charInv.GetWeaponSlot(i + 1);
            // if iterating slot NOT found
            if (iterWepSlot == null)
            {
                // skip to next iteration
                continue;
            }

            /// get the potential swapping char whose favored weapon types matches the iterating weapon 
            /// slot's weapon type set restriction
            CharacterData slotChar = charListArg.Find(
                iterChar => iterChar.favoredWeaponTypes.setId == iterWepSlot.requiredWeaponTypeSet.setId);
            // if a such a character was found
            if (slotChar != null)
            {
                // add the char to the swappable chars to lock them into iterating weapon slot
                swappableCharacters.Add(slotChar);
            }
        }
    }

    /// <summary>
    /// Returns character associated with given character slot.
    /// </summary>
    /// <param name="charSlotArg"></param>
    /// <returns></returns>
    private CharacterData GetSwappingCharacter(int charSlotArg)
    {
        // if given invalid char slot number OR slot that does not currently have an entry
        if (charSlotArg <= 0 || charSlotArg > swappableCharacters.Count)
        {
            // return the primary character
            return primaryCharacter;
        }
        // else given char slot number for slot with existing entry
        else
        {
            // return associated character
            return swappableCharacters[charSlotArg - 1];
        }
    }

    /// <summary>
    /// Sets this character's main data based on the primary character.
    /// </summary>
    private void RefreshCharData()
    {
        _playerCharacterMasterController.SetCharData(primaryCharacter);
    }

    /// <summary>
    /// Sets this character's secondary data based on their current equipped weapon slot.
    /// </summary>
    private void RefreshCharDataSecondary()
    {
        /// set this character's secondary char data (one used for models/heigth/stats/etc. to 
        /// the swapping character associated with the weapon slot they currently have equipped)
        _playerCharacterMasterController.SetCharDataSecondary(GetSwappingCharacter(
            _playerCharacterMasterController.GetEquippedWeaponSlotNum()));
    }

    #endregion




    #region Event Functions

    /// <summary>
    /// Starts listening for all relevant events. 
    /// Call in OnEnable().
    /// </summary>
    private void StartAllEventListening()
    {
        //this.MMEventStartListening<ExitGameSessionEvent>();
        //this.MMEventStartListening<NetworkServerStartEvent>();

        GameManager.Instance.OnPartyCharactersChangedAction += RefreshSwapCharacters;
        GameManager.Instance.OnPartyOrderChangedAction += RefreshSwapCharacters;
        _playerCharacterMasterController.OnEquippedWeaponSlotNumChangedAction += RefreshCharDataSecondary;
    }

    /// <summary>
    /// Stops listening for all relevant events. 
    /// Call in OnDisable().
    /// </summary>
    private void StopAllEventListening()
    {
        //this.MMEventStopListening<ExitGameSessionEvent>();
        //this.MMEventStopListening<NetworkServerStartEvent>();

        GameManager.Instance.OnPartyCharactersChangedAction -= RefreshSwapCharacters;
        GameManager.Instance.OnPartyOrderChangedAction -= RefreshSwapCharacters;
        _playerCharacterMasterController.OnEquippedWeaponSlotNumChangedAction -= RefreshCharData;
    }

    #endregion


}
