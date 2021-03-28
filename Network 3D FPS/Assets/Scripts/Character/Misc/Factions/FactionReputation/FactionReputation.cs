using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class FactionReputation
{
    #region Class Variables

    public FactionData homeFaction;

    //public SerializableDictionaryFactionDataToInt factionsToReputationPoints;
    public List<SerializableDataFactionDataAndInt> factionsAndReputationPoints;

    #endregion




    #region Constructors

    public FactionReputation()
    {
        Setup();
    }

    public FactionReputation(FactionReputation templateArg)
    {
        Setup(templateArg);
    }

    public FactionReputation(FactionReputationTemplate templateArg)
    {
        homeFaction = new FactionData(templateArg.homeFactionTemplate);

        /*factionsToReputationPoints = new SerializableDictionaryFactionDataToInt();
        foreach (FactionDataTemplate iterDictKey in templateArg.factionTemplateToReputationPoints.Keys)
        {
            factionsToReputationPoints[new FactionData(iterDictKey)] = templateArg.
                factionTemplateToReputationPoints[iterDictKey];
        }*/
        factionsAndReputationPoints = new List<SerializableDataFactionDataAndInt>();
        foreach (SerializableDataFactionDataTemplateAndInt iterData in templateArg.factionTemplateAndReputationPoints)
        {
            factionsAndReputationPoints.Add(new SerializableDataFactionDataAndInt(
                new FactionData(iterData.valueFactionDataTemplate), iterData.valueInt));
        }
    }

    #endregion




    #region Setup Functions

    private void Setup()
    {
        //factionsToReputationPoints = new SerializableDictionaryFactionDataToInt();
        factionsAndReputationPoints = new List<SerializableDataFactionDataAndInt>();
    }

    private void Setup(FactionReputation templateArg)
    {
        /*factionsToReputationPoints = new SerializableDictionaryFactionDataToInt();
        foreach (FactionData iterDictKey in templateArg.factionsToReputationPoints.Keys)
        {
            factionsToReputationPoints[iterDictKey] = templateArg.
                factionsToReputationPoints[iterDictKey];
        }*/
        factionsAndReputationPoints = new List<SerializableDataFactionDataAndInt>();
        foreach (SerializableDataFactionDataAndInt iterData in factionsAndReputationPoints)
        {
            factionsAndReputationPoints.Add(new SerializableDataFactionDataAndInt(iterData));
        }
    }

    #endregion




    #region Reputation Functions

    /// <summary>
    /// Returns bool that denotes if given reputation warrant hostility towards reputation holder.
    /// </summary>
    /// <param name="factionRepArg"></param>
    /// <returns></returns>
    public bool DoesReputationWarrantHostility(FactionReputation factionRepArg)
    {
        // if aligned to home faction (i.e. is a comrade or ally)
        if (factionRepArg.homeFaction.factionId == homeFaction.factionId)
        {
            // would NOT be hostile towards allies
            return false;
        }

        // if they have a repuation with home faction
        if (factionRepArg.AreFamiliarWithFaction(homeFaction))
        {
            // return whether their reputation with home faction warrants hostility
            return DoesReputationPointsWarrantHostility(factionRepArg.
                GetReputationPointsAssociatedWithFaction(homeFaction));
        }
        // else they are NOT familiar with home faction
        else
        {
            /// return whether the home faction is hostile towards those they aren't 
            /// familiar with
            return homeFaction.inherentlyHostile;
        }
    }

    /// <summary>
    /// Returns bool that denotes if given reputation allows this reputation's holder 
    /// to harm the given reputation's holder.
    /// </summary>
    /// <param name="factionRepArg"></param>
    /// <returns></returns>
    public bool DoesReputationAllowHarm(FactionReputation factionRepArg)
    {
        // if aligned to home faction (i.e. is a comrade or ally)
        if (factionRepArg.homeFaction.factionId == homeFaction.factionId)
        {
            // return whether this faction has friendly fire
            return homeFaction.canFriendlyFire;
        }
        // else they are a different faction
        else
        {
            // if you are hostile towards them OR they are hostile towards you then you can hurt them
            return DoesReputationWarrantHostility(factionRepArg) || 
                factionRepArg.DoesReputationWarrantHostility(this);
        }
    }

    /// <summary>
    /// Returns bool that denotes if given reputation points warrant hostility.
    /// </summary>
    /// <param name="repPtsArg"></param>
    /// <returns></returns>
    private bool DoesReputationPointsWarrantHostility(int repPtsArg)
    {
        return repPtsArg < -20;
    }

    /// <summary>
    /// Returns bool that denotes if reputation has a standing with the given faction.
    /// </summary>
    /// <param name="factionDataArg"></param>
    /// <returns></returns>
    private bool AreFamiliarWithFaction(FactionData factionDataArg)
    {
        return factionsAndReputationPoints.Exists(
            iterData => iterData.valueFactionData.factionId == factionDataArg.factionId);
    }

    /// <summary>
    /// Returns the reputation points associated with givenf action. 
    /// Returns zero if reputation has NO standing with given faction.
    /// </summary>
    /// <param name="factionDataArg"></param>
    /// <returns></returns>
    private int GetReputationPointsAssociatedWithFaction(FactionData factionDataArg)
    {
        // get standing list entry from given faction
        SerializableDataFactionDataAndInt matchingData = GetDataEntryAssociatedWithFaction(factionDataArg);
        // if found a match
        if (matchingData != null)
        {
            return matchingData.valueInt;
        }
        // else NO match found
        else
        {
            // return a default int value
            return 0;
        }
    }

    /// <summary>
    /// Returns rep standing list entry associated with given faction.
    /// </summary>
    /// <param name="factionDataArg"></param>
    /// <returns></returns>
    private SerializableDataFactionDataAndInt GetDataEntryAssociatedWithFaction(FactionData factionDataArg)
    {
        return factionsAndReputationPoints.First(
            iterData => iterData.valueFactionData.factionId == factionDataArg.factionId);
    }

    /// <summary>
    /// Increase/decrease faction reputation standing.
    /// </summary>
    /// <param name="factionArg"></param>
    /// <param name="repPtsArg"></param>
    private void ChangeFactionReputation(FactionData factionArg, int repPtsArg)
    {
        // if familiar with given faction
        if (AreFamiliarWithFaction(factionArg))
        {
            // add/decrease faction's rep standing
            GetDataEntryAssociatedWithFaction(factionArg).valueInt += repPtsArg;
        }
        // else no knowledge of given faction
        else
        {
            // add new rep standing entry for given faction
            factionsAndReputationPoints.Add(new SerializableDataFactionDataAndInt(factionArg, repPtsArg));
        }
    }

    #endregion


}
