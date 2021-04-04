using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class ActiveAbilityLoadout
{
    #region Class Variables

    // all acquired active abilities
    public List<ActiveAbility> activeAbilities;

    /// list of quick slot indeices which denote which quick slot is associated 
    /// with which active ability
    public List<int> quickSlotsAbilityIndices;

    #endregion




    #region Constructors

    public ActiveAbilityLoadout()
    {
        Setup();
    }

    public ActiveAbilityLoadout(ActiveAbilityLoadout templateArg)
    {
        Setup(templateArg);
    }

    #endregion




    #region Setup Functions

    private void Setup()
    {
        activeAbilities = new List<ActiveAbility>();
        quickSlotsAbilityIndices = new List<int>();

        // initialize number of quick slots the game has
        int NUMBER_OF_QUICKSLOTS = 3;
        // loop for as many quick slots there are
        for (int i = 0; i < NUMBER_OF_QUICKSLOTS; i++)
        {
            // add a quick slot to the quick slot list index that points to an invalid active ability
            quickSlotsAbilityIndices.Add(-1);
        }
    }

    private void Setup(ActiveAbilityLoadout templateArg)
    {
        activeAbilities = new List<ActiveAbility>();
        foreach (ActiveAbility iteratingAbility in templateArg.activeAbilities)
        {
            activeAbilities.Add(new ActiveAbility(iteratingAbility));
        }

        quickSlotsAbilityIndices = new List<int>();
        foreach (int iteratingAbilityIndex in templateArg.quickSlotsAbilityIndices)
        {
            quickSlotsAbilityIndices.Add(iteratingAbilityIndex);
        }
    }

    #endregion




    #region Active Ability Functions

    /// <summary>
    /// Returns the active ability associated with the given quick slot.
    /// </summary>
    /// <param name="quickSlotNumberArg"></param>
    /// <returns></returns>
    public ActiveAbility GetActiveAbility(int quickSlotNumberArg)
    {
        // get appropriate index for quick slot list
        int quickSlotIndex = quickSlotNumberArg - 1;
        // if quick slot list index is invalid
        if (quickSlotIndex < 0 || quickSlotIndex >= quickSlotsAbilityIndices.Count)
        {
            // print warning to console
            Debug.LogWarning("Invalid quick slot number given of: " + quickSlotNumberArg);
            // return NULL active ability
            return null;
        }

        // get appropriate index for ability list
        int abilityIndex = quickSlotsAbilityIndices[quickSlotIndex];
        // if active ability list index is invalid
        if (abilityIndex < 0 || quickSlotIndex >= activeAbilities.Count)
        {
            // return NULL active ability
            return null;
        }

        // return active ability that is being pointed at by the given quick slot
        return activeAbilities[abilityIndex];
    }

    public void QuickSlotChangeNextAbility(int quickSlotNumberArg)
    {
        // change quick slot's ability to next ability
        Debug.LogWarning("NEEDS IMPL: change quick slot's ability to next ability"); //NEEDS IMPL
    }

    public void QuickSlotChangePreviousAbility(int quickSlotNumberArg)
    {
        //change quick slot's ability to previous ability
        Debug.LogWarning("NEEDS IMPL: change quick slot's ability to previous ability"); //NEEDS IMPL
    }

    /// <summary>
    /// Add given ability to list of available abilities. 
    /// If given ability is a duplicate of an already available ability then that 
    /// ability has it's rank increased
    /// </summary>
    /// <param name="activeAbilityToAddArg"></param>
    public void AddActiveAbility(ActiveAbility activeAbilityToAddArg)
    {
        //get acquired ability that matches given ability
        ActiveAbility matchedAcquiredAbility = activeAbilities.FirstOrDefault(
            iterAbility => iterAbility.abilityId == activeAbilityToAddArg.abilityId);
        //if already have acquired the given ability
        if (matchedAcquiredAbility != null)
        {
            //increase rank of matched ability
            matchedAcquiredAbility.PromoteAbilityRank();
        }
        //else does NOT yet have the given ability
        else
        {
            //add ability to list of available abilities
            activeAbilities.Add(activeAbilityToAddArg);
            //equip new ability to a quick slot that does NOT already have an ability assigned to it
            EquipAbilityToVacantQuickSlot(activeAbilities.Count - 1);
        }
    }

    /// <summary>
    /// Equip given ability to a quick slot that does NOT already have an ability assigned to it.
    /// </summary>
    /// <param name="abilitySlotIndexArg"></param>
    private void EquipAbilityToVacantQuickSlot(int abilitySlotIndexArg)
    {
        //loop through all quick slots
        for (int i = 0; i < quickSlotsAbilityIndices.Count; i++)
        {
            //if iterating quick slot does NOT have an ability assigned to it
            if (quickSlotsAbilityIndices[i] < 0)
            {
                //assign the given ability to the iterating quick slot
                quickSlotsAbilityIndices[i] = abilitySlotIndexArg;
                //DONT continue code, because don't want to assign to more than one vacant quick slot
                return;
            }
        }
    }

    #endregion

}
