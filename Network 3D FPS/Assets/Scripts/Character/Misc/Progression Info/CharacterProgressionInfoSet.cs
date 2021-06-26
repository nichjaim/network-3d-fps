using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Used to denote what a level up will give a character, among other purposes.
/// </summary>
[CreateAssetMenu(fileName = "New Character Progression Info Set", menuName = "Scriptable Objects/Character/Character Progression Info Set")]
public class CharacterProgressionInfoSet : ScriptableObject
{
    public List<SerializableDataCharacterProgressionTypeAndFloat> progressionTypeAndProgressionAmount = 
        new List<SerializableDataCharacterProgressionTypeAndFloat>();
}
