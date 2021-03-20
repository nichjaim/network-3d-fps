using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Rarity Tier Template", menuName = "Scriptable Objects/General/Rarity Tier Template")]
public class RarityTierTemplate : ScriptableObject
{
    public Color rarityColor;
    public List<string> potentialContentPrefixNames;

    public RarityTier template;
}
