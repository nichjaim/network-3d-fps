using MoreMountains.Tools;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AIDecisionThreatManager : AIDecision
{
    #region Class Variables

    private Dictionary<CharacterMasterController, int> characterToThreatPoints = new 
        Dictionary<CharacterMasterController, int>();

    #endregion




    #region MonoBehaviour Functions

    private void OnEnable()
    {
        // remove all characters from the threat system
        RemoveAllThreats();
    }

    #endregion




    #region General AI Functions

    /// <summary>
    /// Sets the character that the AI brain is currently targeting.
    /// </summary>
    /// <param name="charToTargetArg"></param>
    private void SetBrainTarget(CharacterMasterController charToTargetArg)
    {
        //if given valid existing character
        if (charToTargetArg != null)
        {
            //set brain target to given char's trans
            _brain.Target = charToTargetArg.transform;
        }
        //else given invalid non-existing character
        else
        {
            //set brain target to a NULL trans
            _brain.Target = null;
        }
    }

    #endregion




    #region Threat Functions

    /// <summary>
    /// Adds threat points to the given character's asscoiated threat.
    /// </summary>
    /// <param name="threateningCharArg"></param>
    /// <param name="threatPointsArg"></param>
    public void AddThreat(CharacterMasterController threateningCharArg, int threatPointsArg)
    {
        // if the given threatenting character is NULL
        if (threateningCharArg == null)
        {
            //DONT continue code
            return;
        }

        // if the threat system DOES have the given character already as a threat
        if (characterToThreatPoints.ContainsKey(threateningCharArg))
        {
            // add the given threat points to the character's exisitng threat points
            characterToThreatPoints[threateningCharArg] += threatPointsArg;
        }
        // else the threat system does NOT have the given character already as a threat
        else
        {
            // add the character to the threat system starting them with the given threat points
            characterToThreatPoints[threateningCharArg] = threatPointsArg;
        }
    }

    /// <summary>
    /// Returns the character with the highest threat. 
    /// Returns NULL is no threatening character found.
    /// </summary>
    /// <returns></returns>
    private CharacterMasterController GetMostThreateningCharacter()
    {
        // if there ARE threatening characters
        if (characterToThreatPoints.Count > 0)
        {
            // return the character with the most threat points
            return characterToThreatPoints.Aggregate((x, y) => x.Value > y.Value ? x : y).Key;
        }
        // else there are NO threatening characters
        else
        {
            // return a NULL to denote that no character was found.
            return null;
        }
    }

    /// <summary>
    /// Removes all characters from the threat system.
    /// </summary>
    private void RemoveAllThreats()
    {
        // set threat dict to fresh empty dict
        characterToThreatPoints = new Dictionary<CharacterMasterController, int>();
    }

    /// <summary>
    /// Makes manager aware of a threat but do not increase the actual threat they pose.
    /// </summary>
    public void MakeAwareOfThreat(CharacterMasterController threateningCharArg)
    {
        AddThreat(threateningCharArg, 0);
    }

    #endregion




    #region Override Functions

    public override bool Decide()
    {
        // get the char with highest threat points
        CharacterMasterController mostThreateningChar = GetMostThreateningCharacter();

        // if threatening target found AND that character to target was not previously being targeted (i.e. is a new target)
        if ((mostThreateningChar != null) && (mostThreateningChar.transform != _brain.Target))
        {
            Debug.Log("NEED IMPL: Play aggro sound effect"); // NEED IMPL!!!
        }

        // set the AI brain's target to the highest threat char's object
        SetBrainTarget(mostThreateningChar);

        // return bool denoting whether a threatening char was found
        return mostThreateningChar != null;
    }

    #endregion


}
