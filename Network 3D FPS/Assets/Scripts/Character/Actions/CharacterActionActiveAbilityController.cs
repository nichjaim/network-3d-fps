using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System;

public class CharacterActionActiveAbilityController : NetworkBehaviour
{
    #region Class Variables

    [Header("Component References")]

    [SerializeField]
    private NetworkIdentity _networkIdentity = null;
    [SerializeField]
    private CharacterMasterController _characterMasterController = null;

    [SerializeField]
    private Transform abilitySpawnPoint = null;

    private SpawnManager _spawnManager = null;

    private Dictionary<string, StopTimer> abilityIdToCooldownTimer = new 
        Dictionary<string, StopTimer>();

    public Action OnActiveAbilityUseAction;

    #endregion




    #region MonoBehaviour Functions

    private void Start()
    {
        // setup all variables that reference singleton instance related components
        InitializeSingletonReferences();
    }

    private void Update()
    {
        // if player is online but NOT associated with the machine running this
        if (GeneralMethods.IsNetworkConnectedButNotLocalClient(_networkIdentity))
        {
            // DONT continue code
            return;
        }

        // increases all cooldown timers by time passed
        IncreaseCooldownTimersByTimePassed();

        // uses the equipped active ability if the appropriate player input was made
        UseActiveAbilityIfPlayerInputted();
    }

    #endregion




    #region Initialization Functions

    /// <summary>
    /// Setup all variables that reference singleton instance related components. 
    /// Call in Start(), needs to be start to give the instances time to be created.
    /// </summary>
    private void InitializeSingletonReferences()
    {
        _spawnManager = GameManager.Instance.SpawnManager;
    }

    #endregion




    #region Ability Functions

    /// <summary>
    /// Returns the active ability that is currently equipped.
    /// </summary>
    /// <returns></returns>
    public ActiveAbility GetCurrentActiveAbility()
    {
        // get current surface-level character data
        CharacterData frontFacingCharData = GeneralMethods.GetFrontFacingCharacterDataFromCharMaster(
            _characterMasterController);

        // if char data found
        if (frontFacingCharData != null)
        {
            // return ability in slot 1
            return frontFacingCharData.activeAbilityLoadout.GetActiveAbility(1);
        }
        // else no valid char data retrieved
        else
        {
            // return a NULL ability
            return null;
        }
    }

    /// <summary>
    /// Returns the cooldown time associated with the equipped ability.
    /// </summary>
    /// <returns></returns>
    public StopTimer GetCurrentAbilityCooldownTime()
    {
        // get currently equipped ability
        ActiveAbility currentAbility = GetCurrentActiveAbility();

        // if ability found
        if (currentAbility != null)
        {
            // if there is a cooldown timer for equipped ability
            if (abilityIdToCooldownTimer.ContainsKey(currentAbility.abilityId))
            {
                return abilityIdToCooldownTimer[currentAbility.abilityId];
            }
            // else no cooldown timer for current ability
            else
            {
                // return NULL timer
                return null;
            }
        }
        // else NO ability equipped
        else
        {
            // return NULL timer
            return null;
        }
    }

    /// <summary>
    /// Returns bool that denotes if current ability is currently on cooldown.
    /// </summary>
    /// <returns></returns>
    private bool IsCurrentAbilityOnCooldown()
    {
        return GetCurrentAbilityCooldownTime() != null;
    }

    /// <summary>
    /// Executes the active ability that is currently equipped.
    /// </summary>
    private void UseCurrentAbility()
    {
        // if equipped ability on cooldown
        if (IsCurrentAbilityOnCooldown())
        {
            // DONT continue code
            return;
        }

        // get currently equipped ability
        ActiveAbility currentAbility = GetCurrentActiveAbility();

        // if have an ability equipped
        if (currentAbility != null)
        {
            // start cooldown timer for current ability
            abilityIdToCooldownTimer[currentAbility.abilityId] = new StopTimer(
                currentAbility.GetTrueActiveAbilityCooldownTime());

            // get current surface-level character data
            CharacterData frontFacingCharData = GeneralMethods.GetFrontFacingCharacterDataFromCharMaster(
                _characterMasterController);

            // spawn and setup an active ability object
            SpawnAbilityPrefab(currentAbility, frontFacingCharData);

            // call ability use actions if NOT null
            OnActiveAbilityUseAction?.Invoke();
        }
    }

    /// <summary>
    /// Increases all cooldown timers by time passed. 
    /// Call in Update().
    /// </summary>
    private void IncreaseCooldownTimersByTimePassed()
    {
        /// initialize list of dict keys whose timers have expired 
        /// NOTE: have to do it this way because can't remove from dict while looping through it
        List<string> expiredKeys = new List<string>();

        // loop through all ability cooldown entries
        foreach (string iterKey in abilityIdToCooldownTimer.Keys)
        {
            // increase iterating ability cooldown timer by time passed
            abilityIdToCooldownTimer[iterKey].IncreaseCurrentTimeByTimePassed();
            // if iterating timer has finished
            if (abilityIdToCooldownTimer[iterKey].IsTimerFinished())
            {
                // remove ability cooldown timer to denote that it is done
                //abilityIdToCooldownTimer.Remove(iterKey);
                // add iterating key to expired keys list
                expiredKeys.Add(iterKey);
            }
        }

        // loop through all expired cooldown entries
        foreach (string iterKey in expiredKeys)
        {
            // remove ability cooldown timer to denote that it is done
            abilityIdToCooldownTimer.Remove(iterKey);
        }
    }

    #endregion




    #region Input Functions

    /// <summary>
    /// Uses the equipped active ability if the appropriate player input was made. 
    /// Call in Update().
    /// </summary>
    private void UseActiveAbilityIfPlayerInputted()
    {
        // if appropriate input given
        if (Input.GetMouseButtonDown(1))
        {
            // executes the active ability that is currently equipped
            UseCurrentAbility();
        }
    }

    #endregion




    #region Spawn Ability Prefab Functions

    /// <summary>
    /// Spawn and setup an active ability object.
    /// </summary>
    /// <param name="abilityArg"></param>
    private void SpawnAbilityPrefab(ActiveAbility abilityArg, CharacterData charDataArg)
    {
        if (NetworkClient.isConnected)
        {
            CmdSpawnAbilityPrefab(abilityArg, charDataArg);
        }
        else
        {
            SpawnAbilityPrefabInternal(abilityArg, charDataArg);
        }
    }

    [Command]
    private void CmdSpawnAbilityPrefab(ActiveAbility abilityArg, CharacterData charDataArg)
    {
        SpawnAbilityPrefabInternal(abilityArg, charDataArg);
    }

    private void SpawnAbilityPrefabInternal(ActiveAbility abilityArg, CharacterData charDataArg)
    {
        // get object pooler associated with given ability
        NetworkObjectPooler netPooler = _spawnManager.GetAbilityPrefabPooler(abilityArg);

        // get a pooled object from pooler
        GameObject pooledAbilityObj = netPooler.GetFromPool(abilitySpawnPoint.position, abilitySpawnPoint.rotation);

        // spawn pooled object on network
        NetworkServer.Spawn(pooledAbilityObj);

        // get ability from pooled object
        ActiveAbilityController activeAbility = pooledAbilityObj.GetComponent<ActiveAbilityController>();

        // if no such component found
        if (activeAbility == null)
        {
            // print warning to console
            Debug.LogWarning("Ability object does NOT have ActiveAbilityController component! " +
                $"Object name: {pooledAbilityObj.name}");

            // DONT continue code
            return;
        }

        // setup pooled active ability
        activeAbility.SetupActiveAbility(abilityArg, _characterMasterController, charDataArg);
    }

    #endregion


}
