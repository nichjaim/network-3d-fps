using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Character Data Template", menuName = "Scriptable Objects/Character/Character Data Template")]
public class CharacterDataTemplate : ScriptableObject
{
    [Header("Template Data Properties")]

    public Color characterInfoColor;

    public WeaponTypeSetTemplate favoredWeaponTypesTemplate;
    public CharacterInventoryTemplate characterInventoryTemplate;

    public FactionReputationTemplate factionReputationTemplate;

    public List<ActiveAbilityTemplate> activeAbilityTemplates;
    public List<CharacterOutfitTemplate> outfitTemplates;

    [Header("Base Data Properties")]

    public CharacterData template;
}
