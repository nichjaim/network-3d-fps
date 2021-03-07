using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Character Inventory Template", menuName = "Scriptable Objects/Inventory/Character Inventory Template")]
public class CharacterInventoryTemplate : ScriptableObject
{
    public List<WeaponSlotDataTemplate> weaponSlotDataTemplates;
}
