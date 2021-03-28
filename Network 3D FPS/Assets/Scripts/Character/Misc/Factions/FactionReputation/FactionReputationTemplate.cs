using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Faction Reputation Template", menuName = "Scriptable Objects/Character/Faction Reputation Template")]
public class FactionReputationTemplate : ScriptableObject
{
    public FactionDataTemplate homeFactionTemplate;

    public List<SerializableDataFactionDataTemplateAndInt> factionTemplateAndReputationPoints;
}
