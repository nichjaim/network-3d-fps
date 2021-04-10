using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class CharacterStatusEffectsController : MonoBehaviour
{
    #region Class Variables

    [Header("Component References")]

    [SerializeField]
    private NetworkIdentity _networkIdentity = null;
    [SerializeField]
    private CharacterMasterController _charMaster = null;
    public CharacterMasterController CharMaster
    {
        get { return _charMaster; }
    }

    private Dictionary<StatusEffect, StopTimer> statusEffectToDurationTimer = new
        Dictionary<StatusEffect, StopTimer>();

    #endregion




    #region MonoBehaviour Functions

    private void Update()
    {
        // if player is online but NOT associated with the machine running this
        if (GeneralMethods.IsNetworkConnectedButNotLocalClient(_networkIdentity))
        {
            // DONT continue code
            return;
        }

        // increases all duration timers by time passed
        IncreaseDurationTimersByTimePassed();
    }

    #endregion




    #region Status Effects Functions

    /// <summary>
    /// Increases all duration timers by time passed. 
    /// Call in Update().
    /// </summary>
    private void IncreaseDurationTimersByTimePassed()
    {
        /// initialize list of dict keys whose timers have expired 
        /// NOTE: have to do it this way because can't remove from dict while looping through it
        List<StatusEffect> expiredKeys = new List<StatusEffect>();

        // loop through all timer entries
        foreach (StatusEffect iterKey in statusEffectToDurationTimer.Keys)
        {
            // increase iterating timer by time passed
            statusEffectToDurationTimer[iterKey].IncreaseCurrentTimeByTimePassed();
            // if iterating timer has finished
            if (statusEffectToDurationTimer[iterKey].IsTimerFinished())
            {
                // add iterating key to expired keys list
                expiredKeys.Add(iterKey);
            }
        }

        // loop through all expired timer entries
        foreach (StatusEffect iterKey in expiredKeys)
        {
            // execute effect ENDING processes
            iterKey.OnStatusEffectEnd(this);

            // remove timer to denote that it is finished
            statusEffectToDurationTimer.Remove(iterKey);
        }
    }

    /// <summary>
    /// Adds status effect with given duration.
    /// </summary>
    /// <param name="statusEffectArg"></param>
    /// <param name="effectDurationArg"></param>
    public void AddStatusEffect(StatusEffect statusEffectArg, float effectDurationArg)
    {
        // start duration timer for given status effect
        statusEffectToDurationTimer[statusEffectArg] = new StopTimer(effectDurationArg);

        // execute effect STARTING processes
        statusEffectArg.OnStatusEffectStart(this);
    }

    #endregion


}
