using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Character Data Template", menuName = "Scriptable Objects/Character/Character Data Template")]
public class CharacterDataTemplate : ScriptableObject
{
    public Color characterInfoColor;
    public WeaponTypeSetTemplate favoredWeaponTypesTemplate;
    public CharacterInventoryTemplate characterInventoryTemplate;

    public CharacterData template;
}
